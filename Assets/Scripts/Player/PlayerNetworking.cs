using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Lemon.Attributes;

namespace Lemon
{
    /*
        This is a general networking class for the player. More specific networking
        implications such as the player's UI will be handled in a different class.
    */

    public class PlayerNetworking : MonoBehaviourPunCallbacks, IPunObservable
    {
        public MonoBehaviour[] localOnlyScripts;
        public GameObject[] localOnlyGameObjects;

        public static GameObject localPlayerInstance;

        [HideInInspector] public Rigidbody m_PlayerRigidbody;    // Aquired on Awake
        [HideInInspector] public HealthAttribute m_PlayerHealth; // Aquired on Awake

        // Used for code that needs to run before we actually disconnect
        public delegate void OnDisconnectingEventHandler(object sender, EventArgs args);
        public event OnDisconnectingEventHandler Disconnecting;

        private void Awake()
        {
            if (photonView.IsMine)
                PlayerNetworking.localPlayerInstance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);

            m_PlayerRigidbody = GetComponent<Rigidbody>();

            m_PlayerHealth = GetComponent<HealthAttribute>();
            m_PlayerHealth.HealthDepleted += OnHealthDepleted;

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
                OnDisconnecting();
        }

        public void OnDisconnecting()
        {
            Disconnecting?.Invoke(this, EventArgs.Empty);
            PhotonNetwork.Disconnect();
        }

        public void OnHealthDepleted(object sender, EventArgs e)
        {
            //PhotonNetwork.Disconnect(); // Welp, that doesn't work as expected
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //stream.SendNext(m_Health);
            }
            else
            {
                // Network player, receive data
                //this.m_Health = (float)stream.ReceiveNext();
            }
        }
    }
}
