using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class PlayerMouseController : MonoBehaviourPunCallbacks
    {
        public PlayerNetworking m_PlayerNetworking; // Should be dependency injected
        public CMF.CameraController m_CameraController;

        private void Awake()
        {
            m_PlayerNetworking.Disconnecting += OnDisconnecting;
        }

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
            m_CameraController.enabled = state;
        }

        public void OnDisconnecting(object sender, EventArgs e)
        {
            LockMouseToCamera(false);
        }
    }
}
