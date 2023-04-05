using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BananaSoup
{
    //Requirements
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBase : MonoBehaviour, IThrowReactable
    {
        [HideInInspector]
        protected Animator anim;
        private PlayerBase playerCharacter;
        protected NavMeshAgent navMeshAgent; // assign navmesh agent
        protected Vector3 playerPosition;

        //Serialized
        [Header("Layer masks")]
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected LayerMask whatIsPlayer;

        [Header("Vision")]
        [SerializeField] private float sightRange = 6;
        [SerializeField] private float attackRange = 1.5f;

        [Header("Stun")]
        [SerializeField] private float stunTime = 2.0f;
        internal Coroutine enemyStunnedRoutine;

        //Turning
        protected float _damp = 3f; //Changes the dampening value of enemy's turning

        //Updating Variables
        //protected float _lastDidSomething; //refreshing timer to prevent non-stop actions
        //private readonly float _pauseTime = 1.5f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;
        [SerializeField] private float patrolRange = 8;

        //Attack
        protected float _timeBetweenAttacks;
        protected bool alreadyAttacked;
        private Vector3 _whereIsPlayer;
        private float _angle; //view angle between enemy and player

        private bool canAttack = true;

        public bool CanAttack
        {
            get => canAttack;
            set => canAttack = value;
        }

        //states
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _stunned;

        public const string attack = "Attack";
        public const string patrol = "Patrol";
        public const string chase = "Chase";

        private EnemyState currentState = EnemyState.Patrol;

        private enum EnemyState
        {
            Patrol,
            Attack,
            Chase
        }

        // Ensure that the Coroutine will be stopped and nulled when GameObject gets disabled.
        private void OnDisable()
        {
            TryEndingRunningCoroutine(ref enemyStunnedRoutine);
        }

        public virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            playerCharacter = PlayerBase.Instance;
            playerPosition = playerCharacter.transform.position;
        }

        public void OnThrowAbility(ParticleProjectile.Type projectileType)
        {
            if ( projectileType == ParticleProjectile.Type.Sand )
            {
                // Check an try end an old stun Coroutine if there one already running.
                // The enemy will get strange behaviours if there are multiple stun Coroutines running.
                TryEndingRunningCoroutine(ref enemyStunnedRoutine);

                enemyStunnedRoutine = StartCoroutine(StunEnemy());
            }
        }

        private void TryEndingRunningCoroutine(ref Coroutine routine)
        {
            if ( routine != null )
            {
                StopCoroutine(routine);
                routine = null;
            }
        }

        private IEnumerator StunEnemy()
        {
            _stunned = true;
            yield return new WaitForSeconds(stunTime);
            _stunned = false;
            enemyStunnedRoutine = null;
        }

        // Update is called once per frame
        private void Update()
        {
            // Check is the enemy stunned. If it is, don't continue Update() method.
            if ( _stunned ) return;

            //Variables
            var position = transform.position;
            _whereIsPlayer = playerPosition - position;
            _angle = Vector3.Angle(_whereIsPlayer, transform.forward);

            _playerInSightRange = Physics.CheckSphere(position, sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(position, attackRange, whatIsPlayer);

            if ( _playerInAttackRange )
                //Smoothed turning towards player. _damp changes the speed of turning
                if ( _angle > 2.5f )
                {
                    var rotate = Quaternion.LookRotation(playerPosition - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * _damp);
                }

            // TODO: Get rid of this.
            //if ( Time.time < _lastDidSomething + _pauseTime ) return;

            SetState();
            StateActions();
        }

        private void SetState()
        {
            Debug.Log("Trying to set state for " + gameObject.name);
            if ( !_playerInSightRange && !_playerInAttackRange )
            {
                currentState = EnemyState.Patrol;
            }

            if ( _playerInSightRange && !_playerInAttackRange )
            {
                currentState = EnemyState.Chase;
            }

            if ( _playerInSightRange && _playerInAttackRange )
            {
                currentState = EnemyState.Attack;
            }

            Debug.Log(gameObject.name + " currentState: " + currentState.ToString());
        }

        private void StateActions()
        {
            switch ( currentState )
            {
                case EnemyState.Patrol:
                    {
                        anim.SetTrigger(patrol);
                        Patrol();
                        break;
                    }
                case EnemyState.Chase:
                    {
                        //anim.SetTrigger(chase);
                        Chase();
                        break;
                    }
                case EnemyState.Attack:
                    {
                        anim.SetTrigger(patrol);
                        Patrol();
                        break;
                    }
            }
        }

        //Patrol method searches random waypoints and moves to them
        private void Patrol()
        {
            if ( !_waypointSet ) SearchWaypoint();

            if ( _waypointSet ) navMeshAgent.SetDestination(waypoint);

            var distanceToWayPoint = transform.position - waypoint;

            //Waypoint reached
            if ( distanceToWayPoint.magnitude < 1f ) _waypointSet = false;

            //_lastDidSomething = Time.time;
        }

        //Looks for a new waypoint through navmesh
        private void SearchWaypoint()
        {
            var randomZ = Random.Range(-patrolRange, patrolRange);
            var randomX = Random.Range(-patrolRange, patrolRange);
            var position = transform.position;

            waypoint = new Vector3(position.x + randomX, position.y,
                position.z + randomZ);

            if ( Physics.Raycast(waypoint, -transform.up, 2f, whatIsGround) ) _waypointSet = true;
            //_lastDidSomething = Time.time;
        }

        // follows the player through navmesh, until the player is within attack range
        private void Chase()
        {
            navMeshAgent.SetDestination(playerPosition);
        }

        public virtual void Attack()
        {
        }

        public void ResetAttack()
        {
            CanAttack = true;
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
    }
}
