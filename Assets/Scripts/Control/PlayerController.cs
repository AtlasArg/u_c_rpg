using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private RaycastHit[] hits;
        private CombatTarget combatTarget;
        private Fighter fighter;
        private Health health;
        // Start is called before the first frame update
        void Start()
        {
            this.mover = this.GetComponent<Mover>();
            this.fighter = this.GetComponent<Fighter>();
            this.health = this.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.health.IsDead())
            {
                if (InteractWithCombat())
                {
                    return;
                }

                if (InteractWithMovement())
                {
                    return;
                }
            }
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    this.mover.StartMoveAction(hit.point, 1f);
                }

                return true;
            }

            return false;
        }

        private bool  InteractWithCombat()
        {
            hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                combatTarget = hit.transform.GetComponent<CombatTarget>();
                
                if (combatTarget == null || !this.fighter.CanAttack(combatTarget.gameObject))
                {
                    continue;
                } 
           
                if (Input.GetMouseButton(0))
                {
                    this.fighter.Attack(combatTarget.gameObject);
                }

                return true;
            }

            return false; 
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

