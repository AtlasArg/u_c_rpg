using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 3f;

        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
         
        private Fighter fighter;
        private Health health;
        private GameObject player;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceLastArriveAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.player = GameObject.FindWithTag("Player");
            this.fighter = GetComponent<Fighter>();
            this.health = this.GetComponent<Health>();
            this.mover = this.GetComponent<Mover>();
            this.actionScheduler = this.GetComponent<ActionScheduler>();
            this.guardPosition = this.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.health.IsDead())
            {
                if (this.InAttackRange() && fighter.CanAttack(player))
                {
                    timeSinceLastSawPlayer = 0;
                    this.AttackBehaviour();
                }
                else if (timeSinceLastSawPlayer < this.suspicionTime)
                {
                    SuspicionBehaviour();
                }
                else
                {
                    PatrolBehaviour();
                }

                UpdateTimers();
            }
        }

        private void UpdateTimers()
        {
            this.timeSinceLastSawPlayer += Time.deltaTime;
            this.timeSinceLastArriveAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    this.timeSinceLastArriveAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (this.timeSinceLastArriveAtWaypoint > this.waypointDwellTime)
            {
                this.mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return this.patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            this.actionScheduler.CancelCurrentAction();
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, this.chaseDistance);
        }

        private void AttackBehaviour()
        {
            this.fighter.Attack(player);
        }

    }
}

