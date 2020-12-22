using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public class CopyTransform : MonoBehaviour
    {
        [Header("Settings")]
        public bool copyRotation;
        public UpdateType updateType;

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

        private void Update() { if (updateType == UpdateType.Update) { UpdateCurrentTransform(); }}

        private void LateUpdate() { if (updateType == UpdateType.LateUpdate) { UpdateCurrentTransform(); }}

        private void UpdateCurrentTransform()
        {
            transform.rotation = target.transform.rotation;
        }
    }
}
