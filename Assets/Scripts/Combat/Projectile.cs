using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10.0f;
        [SerializeField] float lifeAfterImpact = 2.0f;
        [SerializeField] GameObject[] destroyOnHit = null;

        private Health target;
        private float damage = 0;

        public void SetTarget(Health newTarget, float damage)
        {
            this.target = newTarget;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        void Start()
        {
            transform.LookAt(this.GetAimLocation());
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                if (isHoming && !target.IsDead())
                {
                    transform.LookAt(this.GetAimLocation());
                }

                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.gameObject.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * (targetCapsule.height / 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target)
                return;

            if (this.target.IsDead())
            {
                return;
            }

            this.target.TakeDamage(this.damage);
            this.speed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}


