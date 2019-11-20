using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float healthPoints = 100f;

        private bool isDead = false;
        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max (healthPoints - damage, 0);
            if (healthPoints == 0 && !isDead)
            {
                this.Die();
            }
        }

        private void Die()
        {
            isDead = true;
            this.GetComponent<Animator>().SetTrigger("die");
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDead()
        {
            return this.isDead;
        }

        public object CaptureState()
        {
            return this.healthPoints;
        }

        public void RestoreState(object state)
        {
            this.healthPoints = (float)state;
            if (healthPoints == 0)
            {
                this.Die();
            }
        }
    }
}


