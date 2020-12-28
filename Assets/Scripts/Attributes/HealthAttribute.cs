using System;
using UnityEngine;

namespace Lemon.Attributes
{
    public class HealthDeltaEventArgs : EventArgs
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Delta { get; set; }
    }

    public class HealthAttribute : MonoBehaviour
    {
        [Header("Settings")]
        public float m_Health;
        public float m_MaxHealth;

        [Space(10)]
        public bool m_LogOnHealthChanged = false;
        public bool m_LogOnHealthDepleted = false;

        public delegate void HealthChangedEventHandler(HealthAttribute sender, HealthDeltaEventArgs args);
        public event HealthChangedEventHandler HealthChanged;
        public delegate void HealthDepletedEventHandler(object sender, EventArgs args);
        public event HealthDepletedEventHandler HealthDepleted;

        private void Awake()
        {
            m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
            ApplyHealthDelta(0);
        }

        public void ApplyHealthDelta(float delta)
        {
            m_Health = Mathf.Clamp(m_Health + delta, 0, m_MaxHealth);

            if (m_Health == 0) OnHealthDepleted();

            OnHealthChanged(m_Health, delta);
        }

        protected virtual void OnHealthDepleted()
        {
            if (HealthDepleted != null)
                HealthDepleted.Invoke(this, EventArgs.Empty);
            if (m_LogOnHealthDepleted)
                Debug.Log($"name:{transform.name} | instance_id:{transform.GetInstanceID()} | status:dead");
        }

        protected virtual void OnHealthChanged(float health, float delta)
        {
            if (HealthChanged != null)
                HealthChanged.Invoke(this, new HealthDeltaEventArgs { Health = health, MaxHealth = m_MaxHealth, Delta = delta });
            if (m_LogOnHealthChanged)
                Debug.Log($"name:{transform.name} | instance_id:{transform.GetInstanceID()} | health:{health} | delta:{delta}");
        }
    }
}


