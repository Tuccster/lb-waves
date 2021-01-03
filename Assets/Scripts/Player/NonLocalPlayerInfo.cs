using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Attributes;
using Lemon.EventChannels;

namespace Lemon
{
    public class NonLocalPlayerInfo : MonoBehaviour
    {
        [Header("Settings")]
        public Vector3 m_Offset;

        [Header("Resources")]
        public RectTransform m_InfoContainer;
        public Text m_UsernameDisplay;
        public Text m_HealthDisplay;

        private Camera m_LocalPlayerCamera;
        private Transform m_Target;
        private Photon.Realtime.Player m_TargetPlayer;
        private HealthAttribute m_TargetHealthAttribute;

        private void Awake()
        {
            m_LocalPlayerCamera = Camera.main;
        }

        private void Update()
        {
            if(m_Target != null)
            {
                // Check if username position is onscreen; if not hold it still offscreen
                if (Maths.PointInCameraFrustum(m_LocalPlayerCamera, m_Target.position + m_Offset) && m_Target != null)
                    m_InfoContainer.position = m_LocalPlayerCamera.WorldToScreenPoint(m_Target.position + m_Offset);
                else
                    m_InfoContainer.position = new Vector3(-1000, -1000, 0);
            }
        }

        public void SetTarget(Photon.Realtime.Player targetPlayer, Transform targetTransform)
        {
            m_Target = targetTransform;
            m_TargetPlayer = targetPlayer;

            m_TargetHealthAttribute = m_Target.GetComponent<HealthAttribute>();
            m_TargetHealthAttribute.HealthChanged += OnHealthChanged;
            m_TargetHealthAttribute.ForceUpdate();

            m_UsernameDisplay.text = targetPlayer.NickName;
        }

        public void RemoveSetTarget()
        {
            m_TargetHealthAttribute.HealthChanged -= OnHealthChanged;
            Destroy(gameObject);
        }

        // Subscribed to the target player's HealthChanged event in order to update health when the player's health changes
        public void OnHealthChanged(HealthAttribute source, HealthDeltaEventArgs e)
        {
            //m_HealthDisplay.fillAmount = e.Health / e.MaxHealth; // <= Used if health is displayed as a bar
            m_HealthDisplay.text = $"Health: {e.Health}";
        }
    }
}

