using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Attributes;

namespace Lemon
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("Resources")]
        public HealthAttribute m_HealthAttribute;
        public Text m_HealthDisplay;

        private void Awake()
        {
            m_HealthAttribute.HealthChanged += OnHealthChanged;
            m_HealthAttribute.ForceUpdate(); // <= Force udpate
        }

        public void OnHealthChanged(object sender, HealthDeltaEventArgs e)
        {
            //m_HealthDisplay.fillAmount = e.Health / e.MaxHealth; // <= Used if health is displayed as a bar
            m_HealthDisplay.text = $"Health: {e.Health}";
        }
    }
}
