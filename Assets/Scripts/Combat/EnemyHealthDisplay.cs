using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        TextMeshProUGUI textValue;
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            textValue = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (fighter.GetTarget() == null)
            {
                textValue.text = "N/A";
            }
            else
            {
                textValue.text = String.Format("{0:0.0}%", fighter.GetTarget().GetPercentage());
            }
        }
    }
}
