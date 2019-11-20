using System;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private const string ForwardSpeed = "ForwardSpeed";

        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 6f;

        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            this.navMeshAgent.destination = destination;
            this.navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            this.navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            this.navMeshAgent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public object CaptureState()
        {
            // the things have to be serializad
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // its called between awake and start
            SerializableVector3 position = (SerializableVector3)state;
            this.navMeshAgent.enabled = false;
            this.transform.position = position.ToVector();
            this.navMeshAgent.enabled = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            animator = this.GetComponent<Animator>();
            health = this.GetComponent<Health>();
        }

        private void Awake()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            animator = this.GetComponent<Animator>();
            health = this.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !this.health.IsDead();
            this.UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = this.navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            this.animator.SetFloat(ForwardSpeed, speed);
        }
    }
}

