using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;

        LazyValue<Vector3> guardPosition;
        float _timeSinceLastSawPlayer = Mathf.Infinity;
        float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float _timeSinceAggrevated = Mathf.Infinity;
        float _suspiciousTime = 3f;
        float _agroCoolDownTime = 5f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float _wayPointTolerance = 1f;
        [SerializeField] float _waypointDwellTime = 1f;
        [SerializeField] float shoutDistance = 5f;
        int _currentWaypointIndex = 0;
        [Range (0,1)]
        [SerializeField] float _speedPatrolFraction = 0.2f;


        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }
        void Start()
        {
            guardPosition.ForceInit();
        }

        // Update is called once per frame
        void Update()
        {
            if (_health.IsDead()) { return; }
            if (IsAggrevated() && _fighter.CanAttack(_player))
            {
                _timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevated += Time.deltaTime;
        }

        public void Aggrevate()
        {
            Debug.Log("aggrevated");
            _timeSinceAggrevated = 0;
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);

            AgrevateNearbyEnemies();
        }

        private void AgrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                
                if (ai == null) continue;
  
                ai.Aggrevate();
            }
        }

        bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer <= _chaseDistance || _timeSinceAggrevated <_agroCoolDownTime;
        }
        private void PatrolBehaviour() 
        {
            Vector3 nextPosition = guardPosition.value;

            if(_patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition,_speedPatrolFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position,GetCurrentWayPoint());
            return distanceToWaypoint < _wayPointTolerance;
        }

        private void CycleWayPoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }
        private Vector3 GetCurrentWayPoint()
        {
            return _patrolPath.GetWayPoint(_currentWaypointIndex);
        }
        //called by unity 
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,5f);
        }
    }
}
