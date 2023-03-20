using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BananaSoup
{
    //Requirements
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class MeleeRaycast : MonoBehaviour
    {
        //Definitions
        protected NavMeshAgent _enemy; // assign navmesh agent
        protected Transform _playerTarget; // reference to player's position
        protected Vector3 _playerDirection;

        //Serialized
        [Header("Layer masks")] [SerializeField]
        private LayerMask whatIsGround;

        [SerializeField] private LayerMask whatIsPlayer;

        [Header("Combat")] [SerializeField] private float health;
        [SerializeField] private float damage = 1;

        [Header("Vision")] [SerializeField] private float _sightRange;
        [SerializeField] private float _attackRange;
        protected Transform _lookAtTarget;
        protected float _damp = 6f;

        //Updating Variables
        protected float _lastDidSomething; //refreshing timer to prevent non-stop actions
        private readonly float _pauseTime = 1f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;
        private float _waypointRange;

        //Attack
        protected float _timeBetweenAttacks;
        protected bool _alreadyAttacked;
        private Vector3 whereIsPlayer;
        private float angle;

        //states
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _stunned;


        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            var transform1 = PlayerBase.Instance.transform;
            _playerTarget = transform1;
            _playerDirection = transform1.position;
        }

        private void Update()
        {
            //Variables
            var position = transform.position;
            whereIsPlayer = _playerDirection - position;
            angle = Vector3.Angle(whereIsPlayer, transform.forward);

            _playerInSightRange = Physics.CheckSphere(position, _sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(position, _attackRange, whatIsPlayer);


            if (_playerInSightRange)
                //Smoothed turning towards player
                if (angle > 2.5f)
                {
                    var rotate = Quaternion.LookRotation(_playerTarget.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * _damp);
                }

            if (Time.time < _lastDidSomething + _pauseTime) return;



            if (!_stunned)
            {
                
                //compressed if statements for clarity
                if (!_playerInSightRange && !_playerInAttackRange) Patrol();
                if (_playerInSightRange && !_playerInAttackRange) Chase();
                if (_playerInSightRange && _playerInAttackRange) Attack();
                
            }
            
        }

        private void Patrol()
        {
            if (!_waypointSet) SearchWaypoint();

            if (_waypointSet) _enemy.SetDestination(waypoint);

            var distanceToWayPoint = transform.position - waypoint;

            //Waypoint reached
            if (distanceToWayPoint.magnitude < 1f) _waypointSet = false;

            _lastDidSomething = Time.time;
        }

        
        //Looks for a new waypoint through navmesh
        private void SearchWaypoint()
        {
            var randomZ = Random.Range(-_waypointRange, _waypointRange);
            var randomX = Random.Range(-_waypointRange, _waypointRange);
            var position = transform.position;

            waypoint = new Vector3(position.x + randomX, position.y,
                position.z + randomZ);

            if (Physics.Raycast(waypoint, -transform.up, 2f, whatIsGround)) _waypointSet = true;
            _lastDidSomething = Time.time;
        }

        private void Chase()
        {
            _enemy.SetDestination(_playerTarget.position);
        }

        public virtual void Attack()
        {
            //Stop enemy movement
            _enemy.SetDestination(transform.position);


            //transform.LookAt(_playerTarget);

            if (!_alreadyAttacked)
            {
                //TODO Attack code here
                Debug.Log("Enemy Swings");

                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }

            _lastDidSomething = Time.time;
        }

        protected void ResetAttack()
        {
            _alreadyAttacked = false;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        //Display sight and attack ranges
        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, _attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, _sightRange);
        }
    }
}