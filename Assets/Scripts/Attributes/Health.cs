using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float healthPoints = 100f;

        private bool isDead = false;
        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max (healthPoints - damage, 0);
            if (healthPoints == 0 && !isDead)
            {
                this.Die();
                this.AwardExperience(instigator);
            }
        }

        private void Die()
        {
            isDead = true;
            this.GetComponent<Animator>().SetTrigger("die");
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
            }
        }

        public bool IsDead()
        {
            return this.isDead;
        }

        public float GetPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetHealth()) * 100;
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

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();     
        }
    }
}


