using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.Testing
{
    public class TestingFrustumChecker : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = GameObject.FindObjectOfType<Camera>();
        }

        private void OnGUI()
        {
            if (mainCamera != null)
                GUI.Label(new Rect(10, 64, 512, 32), $"{transform.name} within Camera.main frustum -> {Maths.PointInCameraFrustum(mainCamera, transform.position)}");
        }
    }
}
