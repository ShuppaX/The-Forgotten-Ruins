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
        
        //Serialized
        [Header("Layer masks")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;
        
        [Header("Combat")]
        [SerializeField] private float health;
        [SerializeField] private float damage = 1;
        
        [Header("Vision")]
        [SerializeField] private float _sightRange;
        [SerializeField] private float _attackRange;
        
        //Updating Variables
        protected float _lastDidSomething; //refreshing timer to prevent non-stop actions
        private readonly float _pauseTime = 3f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;
        private float _waypointRange;

        //Attack
        protected float _timeBetweenAttacks;
        protected bool _alreadyAttacked;

        //states
        private bool _playerInSightRange;
        private bool _playerInAttackRange;


        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _playerTarget = PlayerBase.Instance.transform;
        }

        private void Update()
        {
            if (Time.time < _lastDidSomething + _pauseTime) return;

            _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);


            //compressed if statements for clarity
            if (!_playerInSightRange && !_playerInAttackRange) Patrol();
            if (_playerInSightRange && !_playerInAttackRange) Chase();
            if (_playerInSightRange && _playerInAttackRange) Attack();
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

        private void SearchWaypoint()
        {
            var randomZ = Random.Range(-_waypointRange, _waypointRange);
            var randomX = Random.Range(-_waypointRange, _waypointRange);
            var position = transform.position;
            
            waypoint = new Vector3(position.x + randomX, position.y,
                position.z + randomZ);

            if (Physics.Raycast(waypoint, -transform.up, 2f, whatIsGround))
            {
                _waypointSet = true;
            }
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

            transform.LookAt(_playerTarget);

            if (!_alreadyAttacked)
            {
                //TODO Attack code here

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