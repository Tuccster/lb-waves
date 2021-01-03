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
        public PhotonPlayerECSO m_PlayerDisconnectingEvent;
        public PlayerNetworking m_PlayerNetworking;
        private CameraController m_CameraController;

        private void Awake()
        {
            m_CameraController = gameObject.GetComponent<CameraController>();

            m_PlayerDisconnectingEvent.PhotonPlayerEvent += OnDisconnecting;
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

        public void OnDisconnecting(object sender, PlayerEventArgs e)
        {
            if (m_PlayerNetworking.photonView.Owner.UserId == e.m_Player.UserId)
                LockMouseToCamera(false);
        }
    }
}
