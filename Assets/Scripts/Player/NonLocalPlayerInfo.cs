using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Attributes;

namespace Lemon
{
    public class NonLocalPlayerInfo : MonoBehaviour
    {
        [Header("Settings")]
        public Vector3 offset;

        [Header("Resources")]
        public RectTransform m_InfoContainer;
        public Text m_UsernameDisplay;
        public Text m_HealthDisplay;

        private Camera m_LocalPlayerCamera;
        private Transform m_Target;


        private void Start()
        {
            m_LocalPlayerCamera = Camera.main;
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

        public void SetTarget(string targetUsername, Transform targetTransform)
        {
            m_UsernameDisplay.text = targetUsername;
            m_Target = targetTransform;

            HealthAttribute healthAtt = m_Target.GetComponent<HealthAttribute>();
            healthAtt.HealthChanged += OnHealthChanged;
            healthAtt.ForceUpdate();
        }

        public void RemoveTarget()
        {
            Debug.Log("RemoveTarget called by " + transform.name);
            m_Target.GetComponent<HealthAttribute>().HealthChanged -= OnHealthChanged;
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

