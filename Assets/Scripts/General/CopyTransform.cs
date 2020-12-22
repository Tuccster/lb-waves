using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public class CopyTransform : MonoBehaviour
    {
        [Header("Settings")]
        public UpdateType updateType;

        [Header("Rotation Settings")]
        public bool copyRotationX = true;
        public bool copyRotationY = true;
        public bool copyRotationZ = true;

        [Header("Resources")]
        public Transform target;

        public enum UpdateType { Update, LateUpdate };

        private void Awake()
        {
            if (target == null)
            {
                Debug.LogError("Target was not assigned");
                this.enabled = false;
            }
        }

        private void Update() { if (updateType == UpdateType.Update) { UpdateCurrentTransform(); } }

        private void LateUpdate() { if (updateType == UpdateType.LateUpdate) { UpdateCurrentTransform(); } }

        private void UpdateCurrentTransform()
        {
            transform.rotation = Quaternion.Euler(
                copyRotationX ? target.rotation.x : transform.rotation.x,
                copyRotationX ? target.rotation.y : transform.rotation.y,
                copyRotationX ? target.rotation.z : transform.rotation.z);
        }
    }
}
