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
        This class also handles data streaming for the player.
    */

    public class PlayerNetworking : MonoBehaviourPunCallbacks
    {
        [Header("Resources")]
        public PlayerMouseController m_PlayerMouseController;
        public PhotonPlayerECSO m_PlayerDisconnectingEvent;
        public MonoBehaviour[] localOnlyScripts;
        public GameObject[] localOnlyGameObjects;

        public static GameObject localPlayerInstance;

        private Rigidbody m_PlayerRigidbody;
        private PlayerUI m_PlayerUI;

        private void Awake()
        {
            m_PlayerRigidbody = GetComponent<Rigidbody>();
            m_PlayerUI = GetComponent<PlayerUI>();
            DontDestroyOnLoad(gameObject);

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
                PlayerNetworking.localPlayerInstance = gameObject;
                transform.name = "player_local";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                m_PlayerMouseController.LockMouseToCamera(false);
                PhotonNetwork.Disconnect();
            }
        }

        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {

            }
            else
            {

            }
        }
        */
    }
}
