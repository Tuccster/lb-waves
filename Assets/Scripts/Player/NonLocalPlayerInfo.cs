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
        public Text usernameDisplay;
        public Image healthBar;

        private Camera localPlayerCamera;
        private Transform targetTransform;


        private void Awake()
        {
            localPlayerCamera = Camera.main;
        }

        private void Update()
        {
            // Check if username position is onscreen; if not hold it still offscreen
            if (Maths.PointInCameraFrustum(localPlayerCamera, targetTransform.position + offset))
                usernameDisplay.rectTransform.position = localPlayerCamera.WorldToScreenPoint(targetTransform.position + offset);
            else
                usernameDisplay.rectTransform.position = new Vector3(-1000, -1000, 0);


            if (targetTransform == null) Destroy(gameObject);
        }

        private void OnDestroy()
        {
            targetTransform.GetComponent<HealthAttribute>().HealthChanged -= OnHealthChanged;
        }

        public void SetTarget(string _targetUsername, Transform _targetTransform)
        {
            usernameDisplay.text = _targetUsername;
            targetTransform = _targetTransform;

            targetTransform.GetComponent<HealthAttribute>().HealthChanged += OnHealthChanged;
        }

        // Subscribed to the target player's HealthChanged event in order to update health bar when the player's health changes
        public void OnHealthChanged(HealthAttribute source, HealthDeltaEventArgs e)
        {
            healthBar.fillAmount = e.Health / source.m_maxHealth;
        }
    }
}

