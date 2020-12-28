using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Lemon
{
    public class PlayerMouseController : MonoBehaviourPunCallbacks
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
            if (!photonView.IsMine) return; // <= The absence of this line caused so much grief
            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
            cameraController.enabled = state;
        }
    }
}
