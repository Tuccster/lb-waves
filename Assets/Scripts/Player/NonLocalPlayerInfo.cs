using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lemon
{
    public class NonLocalPlayerInfo : MonoBehaviour
    {
        [Header("Settings")]
        public Vector3 offset;

        [Header("Resources")]
        public Text usernameDisplay;

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

        public void SetTarget(string _targetUsername, Transform _targetTransform)
        {
            usernameDisplay.text = _targetUsername;
            targetTransform = _targetTransform;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 64, 512, 32), $"targetTransform.position -> {targetTransform.position}");
        }
    }
}

