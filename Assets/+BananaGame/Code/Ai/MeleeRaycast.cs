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
        protected Transform playerTarget; // reference to player's position
        protected Vector3 playerDirection;
        public EnemyMeleeDamage meleeScript; // reference to enemy's melee damage script
        private Animator anim;

        //Serialized
        [Header("Layer masks")] [SerializeField]
        private LayerMask whatIsGround;

        [SerializeField] private LayerMask whatIsPlayer;

        [Header("Vision")] [SerializeField] private float sightRange = 6;
        [SerializeField] private float attackRange = 1.5f;

        [Header("Stun")] [SerializeField] private float stunTime = 2.0f;
        internal Coroutine enemyStunnedRoutine;

        //Turning
        protected float _damp = 3f; //Changes the dampening value of enemy's turning

        //Updating Variables
        protected float _lastDidSomething; //refreshing timer to prevent non-stop actions
        private readonly float _pauseTime = 1.5f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;
        [SerializeField] private float patrolRange = 8;

        //Attack
        private float _timeBetweenAttacks;
        protected bool alreadyAttacked;
        private Vector3 _whereIsPlayer;
        private float _angle; //view angle between enemy and player
        private Collider _weaponCollider;
        private Coroutine _weaponColliderCD;

        //states
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _stunned;

        private const string attack = "Attack";
        private const string patrol = "Patrol";

        /// <summary>
        /// for animator triggers
        /// 1 for patrol
        /// 2 for chase
        /// 3 for attack
        /// </summary>
        private int state;

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
        }

        private void Update()
        {
            // Check is the enemy stunned. If it is, don't continue Update() method.
            if (_stunned) return;

            //Variables
            var position = transform.position;
            _whereIsPlayer = playerDirection - position;
            _angle = Vector3.Angle(_whereIsPlayer, transform.forward);

            _playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);


            if (_playerInAttackRange)
                //Smoothed turning towards player. _damp changes the speed of turning
                if (_angle > 2.5f)
                {
                    var rotate = Quaternion.LookRotation(playerTarget.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * _damp);
                }

            // TODO: Get rid of this.
            if (Time.time < _lastDidSomething + _pauseTime) return;

            //Enemy AI states
            if (!_playerInSightRange && !_playerInAttackRange)
            {
                state = 1;
                anim.SetTrigger(patrol);
                Patrol();
            }

            if (_playerInSightRange && !_playerInAttackRange)
            {
                state = 2;
                anim.SetTrigger(patrol);
                Chase();
            }

            if (_playerInSightRange && _playerInAttackRange)
            {
                state = 3;
                Attack();
            }
        }

        //Patrol method searches random waypoints and moves to them
        public void Patrol()
        {
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
            _lastDidSomething = Time.time;
        }

        //follows the player through navmesh, until the player is within attack range
        private void Chase()
        {
            enemy.SetDestination(playerTarget.position);
        }

        public virtual void Attack()
        {
            //Stop enemy movement
            enemy.SetDestination(transform.position);

            //prevent weapon doing damage while not in animation


            if (meleeScript.CanDealDamage)
            {
                meleeScript.CanDealDamage = false;
                anim.SetTrigger(attack);
                _weaponColliderCD = StartCoroutine(WeaponColliderCycle());
                meleeScript.MeleeAttack();
            }

            _lastDidSomething = Time.time;
        }


        //Display sight and attack ranges
        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, sightRange);
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            if (projectileType == ParticleProjectile.Type.Sand)
            {
                // Check an try end an old stun Coroutine if there one already running.
                // The enemy will get strange behaviours if there are multiple stun Coroutines running.
                TryEndingRunningCoroutine(ref enemyStunnedRoutine);

                enemyStunnedRoutine = StartCoroutine(StunEnemy());
            }
        }

        private IEnumerator StunEnemy()
        {
            _stunned = true;
            yield return new WaitForSeconds(stunTime);
            _stunned = false;
            enemyStunnedRoutine = null;
        }

        // Ensure that the Coroutine will be stopped and nulled when GameObject gets disabled.
        private void OnDisable()
        {
            TryEndingRunningCoroutine(ref enemyStunnedRoutine);
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
    }
}