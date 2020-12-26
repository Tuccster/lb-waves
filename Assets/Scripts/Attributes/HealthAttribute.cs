using System;
using UnityEngine;

namespace Lemon.Attributes
{
    public class HealthDeltaEventArgs : EventArgs
    {
        public float Health { get; set; }
        public float Delta { get; set; }
    }

    public class HealthAttribute : MonoBehaviour
    {
        public float m_health;
        public float m_maxHealth;

        public delegate void HealthChangedEventHandler(HealthAttribute sender, HealthDeltaEventArgs args);
        public event HealthChangedEventHandler HealthChanged;
        public delegate void HealthDepletedEventHandler(object sender, EventArgs args);
        public event HealthDepletedEventHandler HealthDepleted;

        private void Awake()
        {
            m_health = Mathf.Clamp(m_health, 0, m_maxHealth);
        }

        public void ApplyHealthDelta(float delta)
        {
            m_health = Mathf.Clamp(m_health + delta, 0, m_maxHealth);

            if (m_health == 0) OnHealthDepleted();

            OnHealthChanged(m_health, delta);
        }

        protected virtual void OnHealthDepleted()
        {
            if (HealthDepleted != null)
                HealthDepleted.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHealthChanged(float health, float delta)
        {
            if (HealthChanged != null)
                HealthChanged.Invoke(this, new HealthDeltaEventArgs { Health = health, Delta = delta });
        }
    }
}


