using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private float weaponDamage = 5f;
        [SerializeField]
        private float timeBetweenAttacks = 1f;

        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;
        private float timeSincesLastAttack = Mathf.Infinity;

        private const string StopAttackTrigger = "stopAttack";
        private const string AttackTrigger = "attack";

        private void Start()
        {
            this.mover = this.GetComponent<Mover>();
            this.actionScheduler = this.GetComponent<ActionScheduler>();
            this.animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSincesLastAttack += Time.deltaTime ;

            if (target == null || target.IsDead())
            {
                return;
            }

            if (!this.GetIsInRange())
            {
                this.mover.MoveTo(this.target.transform.position, 1f);
            }
            else 
            {
                this.mover.Cancel();
                this.AttackBehaviour();
               
            }
        }

        public void Attack(GameObject combatTarget)
        {
            this.actionScheduler.StartAction(this);
            this.target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }

            Health health = combatTarget.GetComponent<Health>();
            return health != null && !health.IsDead();
        }

        public void Cancel()
        {
            this.SetStopAttackTrigger();
            this.target = null;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (this.timeSincesLastAttack > timeBetweenAttacks)
            {
                //this will trigger the hit event
                SetAttackTrigger();
                this.timeSincesLastAttack = 0;
            }
        }

        // Animation event
        private void Hit()
        {
            if (target != null)
            {
                target.TakeDamage(this.weaponDamage);
            } 
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        private void SetAttackTrigger()
        {
            this.animator.ResetTrigger(StopAttackTrigger);
            this.animator.SetTrigger(AttackTrigger);
        }

        private void SetStopAttackTrigger()
        {
            this.animator.ResetTrigger(AttackTrigger);
            this.animator.SetTrigger(StopAttackTrigger);
        }
    }
}
