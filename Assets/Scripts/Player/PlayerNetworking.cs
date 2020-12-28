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
        [Header("Resources")]
        public GameObject m_PlayerUI;

        [Space(10)]
        public MonoBehaviour[] localOnlyScripts;
        public GameObject[] localOnlyGameObjects;

        public static GameObject localPlayerInstance;

        [HideInInspector] public NonLocalPlayerInfo m_PlayerUIInstance; // Aquired when another player spawns
        [HideInInspector] public Rigidbody m_PlayerRigidbody;    // Aquired on Awake
        [HideInInspector] public HealthAttribute m_PlayerHealth; // Aquired on Awake

        private void Awake()
        {
            if (photonView.IsMine)
                PlayerNetworking.localPlayerInstance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);

            m_PlayerRigidbody = GetComponent<Rigidbody>();

            m_PlayerHealth = GetComponent<HealthAttribute>();
            m_PlayerHealth.HealthDepleted += OnHealthDepleted;
        }

        private void Start()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                transform.name = "player_nonlocal";

                for (int i = 0; i < localOnlyScripts.Length; i++)
                    localOnlyScripts[i].enabled = false;

                for (int i = 0; i < localOnlyGameObjects.Length; i++)
                    localOnlyGameObjects[i].SetActive(false);

                // Probably should be moved into PlayerUI
                m_PlayerUIInstance = Instantiate(m_PlayerUI).GetComponent<NonLocalPlayerInfo>();
                m_PlayerUIInstance.SetTarget(photonView.Owner.NickName, transform);
            }
            else
            {
                transform.name = "player_local";
            }
        }

        public void OnHealthDepleted(object sender, EventArgs e)
        {
            //PhotonNetwork.Disconnect(); // Welp, that doesn't work as expected
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (m_PlayerUIInstance != null) Destroy(m_PlayerUIInstance);
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
