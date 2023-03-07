using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BananaSoup
{
    public class MeleeRaycast : MonoBehaviour
    {
        private NavMeshAgent _enemy; // assign navmesh agent
        private Transform _playerTarget; // reference to player's position

        public LayerMask whatIsGround, whatIsPlayer;
        public float health;
        private float _lastDidSomething; //refreshing timer to prevent non-stop actions
        private float _pauseTime = 3f; //Time to pause after action

        //patrol
        public Vector3 waypoint;
        private bool _waypointSet;
        private float _waypointRange;

        //Attack
        private float _timeBetweenAttacks;
        private bool _alreadyAttacked;

        //states
        private float _sightRange, _attackRange;
        private bool _playerInSightRange, _playerInAttackRange;

        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _playerTarget = PlayerBase.Instance.transform; //TODO Nullreffs 

        }

        private void Update()
        {
            if (Time.time < _lastDidSomething + _pauseTime) return;

            _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, whatIsPlayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);


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

            waypoint = new Vector3(transform.position.x + randomX, transform.position.y,
                transform.position.z + randomZ);

            if (Physics.Raycast(waypoint, -transform.up, 2f, whatIsGround)) _waypointSet = true;
            _lastDidSomething = Time.time;
        }

        private void Chase()
        {
            _enemy.SetDestination(_playerTarget.position);
        }

        private void Attack()
        {
            //Stop enemy movement
            _enemy.SetDestination(transform.position);

            transform.LookAt(_playerTarget);

            if (!_alreadyAttacked)
            {
                //Attack code here

                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }

            _lastDidSomething = Time.time;
        }

        private void ResetAttack()
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _sightRange);
        }
    }
}