using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float _maxSpeed = 5.46f;
        private NavMeshAgent _naveMeshAgent;
        Health _health;
        // Start is called before the first frame update
        private void Awake()
        {
            _naveMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            _naveMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _naveMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _naveMeshAgent.destination = destination;
            _naveMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _naveMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _naveMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;

            _naveMeshAgent.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            _naveMeshAgent.enabled = true;
        }
    }
}

