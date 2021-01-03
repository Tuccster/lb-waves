using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using CMF;
using Photon.Realtime;
using Lemon.EventChannels;

namespace Lemon
{
    // This class should not be networked. What should actually happen is that an PhotonViewEventChannelSO event should be called
    // and then we can look through all the players [which idealy would be stored some place]... [haha PhotonNetwork.playerList]

    public class PlayerMouseController : MonoBehaviourPunCallbacks
    {
        public PlayerNetworking m_PlayerNetworking;
        private CameraController m_CameraController;

        private void Awake()
        {
            m_CameraController = gameObject.GetComponent<CameraController>();
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
            if (!photonView.IsMine) return;
            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
            m_CameraController.enabled = state;
        }
    }
}
