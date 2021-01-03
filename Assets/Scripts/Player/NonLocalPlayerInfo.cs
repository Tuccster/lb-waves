using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Lemon.Attributes;
using Lemon.EventChannels;
using ExitGames.Client.Photon;

namespace Lemon
{
    public class NonLocalPlayerInfo : MonoBehaviour, IOnEventCallback
    {
        [Header("Settings")]
        public Vector3 offset;

        [Header("Resources")]
        public PhotonPlayerECSO m_PlayerDisconnectingEvent;
        public RectTransform m_InfoContainer;
        public Text m_UsernameDisplay;
        public Text m_HealthDisplay;

        private Camera m_LocalPlayerCamera;
        private Transform m_Target;
        private Photon.Realtime.Player m_TargetPlayer;

        private void Awake()
        {
            m_LocalPlayerCamera = Camera.main;
            m_PlayerDisconnectingEvent.PhotonPlayerEvent += OnPlayerDisconnecting;
        }

        private void Update()
        {
            if(m_Target != null)
            {
                // Check if username position is onscreen; if not hold it still offscreen
                if (Maths.PointInCameraFrustum(m_LocalPlayerCamera, m_Target.position + offset) && m_Target != null)
                    m_InfoContainer.position = m_LocalPlayerCamera.WorldToScreenPoint(m_Target.position + offset);
                else
                    m_InfoContainer.position = new Vector3(-1000, -1000, 0);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == PhotonPlayerECSO.PHOTON_PLAYER_ESCO)
            {
                object[] data = (object[])photonEvent.CustomData;
                OnPlayerDisconnecting(data[0], (PlayerEventArgs)data[1]);
            }
        }

        public void SetTarget(Photon.Realtime.Player targetPlayer, Transform targetTransform)
        {
            m_UsernameDisplay.text = targetPlayer.NickName;
            m_Target = targetTransform;
            m_TargetPlayer = targetPlayer;

            HealthAttribute healthAtt = m_Target.GetComponent<HealthAttribute>();
            healthAtt.HealthChanged += OnHealthChanged;
            healthAtt.ForceUpdate();
        }

        public void RemoveSetTarget()
        {
            HealthAttribute healthAtt = m_Target.GetComponent<HealthAttribute>();
            healthAtt.HealthChanged -= OnHealthChanged;

            Destroy(gameObject);
        }

        public void OnPlayerDisconnecting(object sender, PlayerEventArgs e)
        {
            Debug.Log($"OnPlayerDisconnecting({sender}, {e}) {m_TargetPlayer.UserId} / {e.m_Player.UserId}");
            if (m_TargetPlayer.UserId == e.m_Player.UserId)
            {
                m_Target.GetComponent<HealthAttribute>().HealthChanged -= OnHealthChanged;
                Destroy(gameObject);
            }
        }

        // Subscribed to the target player's HealthChanged event in order to update health when the player's health changes
        public void OnHealthChanged(HealthAttribute source, HealthDeltaEventArgs e)
        {
            //m_HealthDisplay.fillAmount = e.Health / e.MaxHealth; // <= Used if health is displayed as a bar
            m_HealthDisplay.text = $"Health: {e.Health}";
        }
    }
}

