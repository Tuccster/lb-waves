using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Lemon.Attributes;
using Lemon.EventChannels;

namespace Lemon
{
    /*
        This is a general networking class for the player. More specific networking
        implications such as the player's UI will be handled in a different class.
    */

    public class PlayerNetworking : MonoBehaviourPunCallbacks
    {
        [Header("Resources")]
        public PhotonPlayerECSO m_PlayerDisconnectingEvent;
        public MonoBehaviour[] localOnlyScripts;
        public GameObject[] localOnlyGameObjects;

        public static GameObject localPlayerInstance;

        [HideInInspector] public Rigidbody m_PlayerRigidbody;    // Aquired on Awake
        [HideInInspector] public PlayerUI m_PlayerUI;

        private void Awake()
        {
            if (photonView.IsMine)
                PlayerNetworking.localPlayerInstance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);

            m_PlayerRigidbody = GetComponent<Rigidbody>();
            m_PlayerUI = GetComponent<PlayerUI>();

            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                transform.name = "player_nonlocal";

                for (int i = 0; i < localOnlyScripts.Length; i++)
                    localOnlyScripts[i].enabled = false;

                for (int i = 0; i < localOnlyGameObjects.Length; i++)
                    localOnlyGameObjects[i].SetActive(false);
            }
            else
            {
                transform.name = "player_local";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) 
            {
                //m_PlayerDisconnectingEvent.RaiseEvent(this, new PlayerEventArgs(PhotonNetwork.LocalPlayer));
                //m_PlayerUI.Disconnecting(); // Tell the ui script for this player to handle what it needs to before we disconnect
                Destroy(m_PlayerUI.m_NonlocalPlayerInfo.gameObject);
                PhotonNetwork.Disconnect();
            }
        }
    }
}
