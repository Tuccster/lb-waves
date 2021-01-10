using System;
using UnityEngine;
using Photon.Pun;

namespace Lemon.Attributes
{
    public class HealthDeltaEventArgs : EventArgs
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Delta { get; set; }
    }

    public class HealthAttribute : MonoBehaviourPunCallbacks
    {
        [Header("Settings")]
        public bool m_SyncOnNetwork = true;
        public float m_Health;
        public float m_MaxHealth;
        public bool m_DestroyOnDeplete;

        [Space(10)]
        public bool m_LogOnHealthChanged = false;
        public bool m_LogOnHealthDepleted = false;

        public delegate void HealthChangedEventHandler(HealthAttribute sender, HealthDeltaEventArgs args);
        public event HealthChangedEventHandler HealthChanged;
        public delegate void HealthDepletedEventHandler(object sender, EventArgs args);
        public event HealthDepletedEventHandler HealthDepleted;

        [HideInInspector] public float m_LastSyncedHealth;

        private void Awake()
        {
            m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
        }

        // Used to call events to update subscribers
        public void ForceUpdate(bool sync = false)
        {
            ApplyHealthDelta(0, sync);
        }

        [PunRPC]
        public void SetHealth(float amount, bool sync = true)
        {
            ApplyHealthDelta(amount - m_Health, sync);
        }

        public void ApplyHealthDelta(float delta, bool sync = true)
        {
            // Only apply health delta if we own this photon view or we are recieving an update via RPC
            if (photonView.IsMine || !sync) // <= This might need to be replaced with information from a PhotonMessageInfo parameter added to this method
            {
                m_Health = Mathf.Clamp(m_Health + delta, 0, m_MaxHealth);
                if (m_Health == 0) OnHealthDepleted();
                OnHealthChanged(m_Health, delta);

                if (sync) photonView.RPC("SetHealth", RpcTarget.Others, m_Health, false);
            } 
        }

        protected virtual void OnHealthDepleted()
        {
            if (HealthDepleted != null)
                HealthDepleted.Invoke(this, EventArgs.Empty);
            if (m_LogOnHealthDepleted)
                Debug.Log($"name:{transform.name} | instance_id:{transform.GetInstanceID()} | status:dead");
            if (m_DestroyOnDeplete)
                Destroy(gameObject);
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


