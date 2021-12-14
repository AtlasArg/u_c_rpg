using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        TextMeshProUGUI textValue;
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            textValue = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            textValue.text = String.Format("{0:0.0}%", health.GetPercentage());
        }
    }
}

