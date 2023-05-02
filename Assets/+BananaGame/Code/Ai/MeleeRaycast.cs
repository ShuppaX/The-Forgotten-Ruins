using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BananaSoup
{
    //Requirements
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class MeleeRaycast : MonoBehaviour, IThrowReactable
    {
        //Definitions
        protected NavMeshAgent enemy; // assign navmesh agent
        private Transform playerTarget; // reference to player's position
        public EnemyMeleeDamage meleeScript; // reference to enemy's melee damage script
        protected Animator anim; // reference to animator
        private Vector3 playerDirection; // direction to player

        //Serialized
        [Header("Layer masks")] 
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;

        [Header("Vision")] 
        [SerializeField] private float sightRange = 6;
        [SerializeField] private float attackRange = 1.5f;

        [Header("Stun")]
        [SerializeField] private float stunTime = 2.0f;
        private Coroutine _enemyStunnedRoutine;

        //Turning
        [SerializeField] [Tooltip("Changes the dampening value of enemy's turn rate")]
        private float _damp = 3f;

        //Updating Variables
        protected float lastDidSomething; //refreshing timer to prevent non-stop actions
        private readonly float _pauseTime = 1.5f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;

        [SerializeField] [Tooltip("Range where AI searches a random waypoint to patrol from")]
        private float patrolRange = 8;

        //Attack
        private float _timeBetweenAttacks;
        private Vector3 _whereIsPlayer;
        private float _angle; //view angle between enemy and player
        private Collider _weaponCollider;
        private Coroutine _weaponColliderCD;

        //states
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _stunned;
        private static readonly int Speed = Animator.StringToHash("Speed");

        //Animator triggers
        protected const string attack = "Attack"; //RangedRaycast.cs uses this
        private const string patrol = "Patrol";
        private const string Idle = "Idle";
        private const string animChase = "Chase"; //might not be used. Added in case it's needed
        private const string animStunned = "Stun";
        private string _previousAnimation;
        private Ray _vision;
        private bool _canSeePlayer;


        public virtual void Awake()
        {
            enemy = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            _weaponCollider = meleeScript.GetComponent<Collider>();

        }

        private void Start()
        {
            var transform1 = PlayerBase.Instance.transform;
            playerTarget = transform1;
            playerDirection = transform1.position;
            
            SetTrigger(patrol); //Default animation
        }

        private void Update()
        {
            //Refresh updating variables like velocity for the animator
            RefreshAnimatorValues();
            // Check is the enemy stunned. If it is, don't continue Update() method.
            if (_stunned)
            {
                //Reset current trigger
                ClearTrigger();
                SetTrigger(animStunned);
                return;
            }


            //Variables
            var unitTransform = transform;
            var position = unitTransform.position;
            _whereIsPlayer = playerDirection - position;
            _angle = Vector3.Angle(_whereIsPlayer, unitTransform.forward);

            _playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);
            
            
            //TODO doesn't update when player is moving
            _vision = new Ray(position + new Vector3(0,0.5f,0), _whereIsPlayer);
            

            
            //Vision related statements
            
            
            //Smoothed turning towards player. _damp changes the speed of turning
            if (_playerInAttackRange)
                if (_angle > 2.5f)
                {
                    var rotate = Quaternion.LookRotation(playerTarget.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * _damp);
                }

            //Raycast to check if player is not obscured by obstacles
            //TODO FIX IT
            //Raycast in the direction of player within sightrange and check if it hits the player

            /*if (Physics.Raycast(_vision, out var sighted, 12f, LayerMask.GetMask("Ground")))
            {
                // If the ray hits something on the "Ground" layer, check if it's a wall
                if (sighted.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Debug.Log("There is a wall between the player and enemy.");
                    _canSeePlayer = false;
                }

            }
            else
            {
                _canSeePlayer = true;
            }*/


            if (Time.time < lastDidSomething + _pauseTime) return;

            //Enemy AI states
            
            //Patrol
            if (!_playerInSightRange && !_playerInAttackRange)
            {
                
                Patrol();
            }

            //Chase
            if (_playerInSightRange && !_playerInAttackRange)
            {
                
                Chase();
            }

            //Attack
            if (_playerInSightRange && _playerInAttackRange)
            {
              //  if (!_canSeePlayer) return;
                //Trigger for attack is in Attack() method due to inheritance
                Attack();
            }

            //Idle patrol
            else if (!_playerInSightRange && !_playerInAttackRange && Speed < 0.1 ) //Determines the threshold for idle animation
            {
                ClearTrigger();
                SetTrigger(Idle); 
            }
        }

        protected void SetTrigger(string currentAnimation)
        {
            _previousAnimation = currentAnimation;
            anim.SetTrigger(currentAnimation);
        }

        protected void ClearTrigger()
        {
            anim.ResetTrigger(_previousAnimation);
        }

        //Patrol method searches random waypoints and moves to them
        public void Patrol()
        {
            ClearTrigger();
            SetTrigger(patrol);
            if (!_waypointSet) SearchWaypoint();

            if (_waypointSet) enemy.SetDestination(waypoint);

            var distanceToWayPoint = transform.position - waypoint;

            //Waypoint reached
            if (distanceToWayPoint.magnitude < 1f) _waypointSet = false;

            // _lastDidSomething = Time.time;
        }


        //Looks for a new waypoint through navmesh
        private void SearchWaypoint()
        {
            var randomZ = Random.Range(-patrolRange, patrolRange);
            var randomX = Random.Range(-patrolRange, patrolRange);
            var position = transform.position;

            waypoint = new Vector3(position.x + randomX, position.y,
                position.z + randomZ);

            if (Physics.Raycast(waypoint, -transform.up, 2f, whatIsGround)) _waypointSet = true;
            lastDidSomething = Time.time;
        }

        //follows the player through navmesh, until the player is within attack range
        private void Chase()
        {
            ClearTrigger();
            SetTrigger(animChase); 
            enemy.SetDestination(playerTarget.position);
        }

        protected virtual void Attack()
        {
            //Stop enemy movement
            enemy.SetDestination(transform.position);

            //prevent weapon doing damage while not in animation
            if (meleeScript.CanDealDamage)
            {
                meleeScript.CanDealDamage = false;
                ClearTrigger();
                SetTrigger(attack);
                _weaponColliderCD = StartCoroutine(WeaponColliderCycle());
                meleeScript.MeleeAttack();
            }

            lastDidSomething = Time.time;
        }


        //Display sight and attack ranges
        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, attackRange); //Radius of attack range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, sightRange); //Radius of sight range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, patrolRange); //Radius of patrol range
            
           // Gizmos.color = Color.blue;
           // Gizmos.DrawRay(position + new Vector3(0,0.5f,0), _whereIsPlayer); //Line to player
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            if (projectileType == ParticleProjectile.Type.Sand)
            {
                // Check an try end an old stun Coroutine if there one already running.
                // The enemy will get strange behaviours if there are multiple stun Coroutines running.
                TryEndingRunningCoroutine(ref _enemyStunnedRoutine);
                _enemyStunnedRoutine = StartCoroutine(StunEnemy());
            }
        }

        private IEnumerator StunEnemy()
        {
            _stunned = true;
            yield return new WaitForSeconds(stunTime);
            _stunned = false;
            _enemyStunnedRoutine = null;
        }

        // Ensure that the Coroutine will be stopped and nulled when GameObject gets disabled.
        private void OnDisable()
        {
            TryEndingRunningCoroutine(ref _enemyStunnedRoutine);
            TryEndingRunningCoroutine(ref _weaponColliderCD);
        }

        private void TryEndingRunningCoroutine(ref Coroutine routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }
        }

        private IEnumerator WeaponColliderCycle()
        {
            yield return new WaitForSeconds(0.1f);
            _weaponCollider.enabled = true;
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(meleeScript.ResetCanDealDamage(1.5f));
            _weaponCollider.enabled = false;
            _weaponColliderCD = null;
        }

        private void RefreshAnimatorValues()
        {
            //Sends the velocity of the enemy to the animator
            anim.SetFloat(Speed, enemy.velocity.magnitude);
        }
    }
}