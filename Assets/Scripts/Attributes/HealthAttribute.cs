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

    public class HealthAttribute : MonoBehaviourPunCallbacks//, IPunObservable
    {
        [Header("Settings")]
        public bool m_SyncOnNetwork = true;
        public float m_Health;
        public float m_MaxHealth;

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
            //ForceUpdate(); // <= Other methods will call this when needed
        }

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
            m_Health = Mathf.Clamp(m_Health + delta, 0, m_MaxHealth);
            if (m_Health == 0) OnHealthDepleted();
            OnHealthChanged(m_Health, delta);
            
            if (sync) photonView.RPC("SetHealth", RpcTarget.Others, m_Health, false);
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

        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            return;
            //if (!m_SyncOnNetwork || m_Health == m_LastSyncedHealth) return;
            //Debug.Log($"Syncing m_Health ({UnityEngine.Random.Range(1000, 10000)})");
            //Debug.Log($"m_Health = {m_Health} | m_LastSyncedHealth = {m_LastSyncedHealth}");
            try
            {
                // Sync the health across the network
                if (stream.IsWriting)
                {
                    // Send order -> 0
                    Debug.Log($"stream.SendNext({m_Health})");
                    stream.SendNext(m_Health);
                }
                else
                {
                    // Receive order -> 0
                    m_Health = (float)stream.ReceiveNext();
                    Debug.Log($"(float)stream.ReceiveNext() = {m_Health}"); // <= Only gets called when mouse is moved>???????
                    //Debug.Log($"rFloat:{rFloat}");
                    //m_Health = rFloat;
                    //m_LastSyncedHealth = m_Health;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        */
    }
}


