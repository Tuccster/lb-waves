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
        public Image m_HealthDisplay;

        private void Awake()
        {
            m_HealthAttribute.HealthChanged += OnHealthChanged;
        }

        public void OnHealthChanged(object sender, HealthDeltaEventArgs e)
        {
            m_HealthDisplay.fillAmount = e.Health / e.MaxHealth;
        }
    }
}
