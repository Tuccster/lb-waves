using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public class PlayerMouseController : MonoBehaviour
    {
        public CMF.CameraController cameraController;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                LockMouseToCamera(false);
            if (Input.GetKeyDown(KeyCode.Mouse0))
                LockMouseToCamera(true);
        }

        public void LockMouseToCamera(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
            cameraController.enabled = state;
        }
    }
}
