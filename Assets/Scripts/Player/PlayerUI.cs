using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Attributes;
using Lemon.EventChannels;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class PlayerUI : MonoBehaviourPunCallbacks
    {
        [Header("Resources")]
        public GameObject m_NonlocalPlayerUI;
        public Text m_HealthDisplay;

        [HideInInspector] public NonLocalPlayerInfo m_NonlocalPlayerInfo;

        private PlayerNetworking m_PlayerNetworking;
        private HealthAttribute m_HealthAttribute;

        private void Awake()
        {
            m_PlayerNetworking = gameObject.GetComponent<PlayerNetworking>();
            m_HealthAttribute = gameObject.GetComponent<HealthAttribute>();

            m_HealthAttribute.HealthChanged += OnHealthChanged;
            m_HealthAttribute.ForceUpdate();

            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                m_NonlocalPlayerInfo = Instantiate(m_NonlocalPlayerUI).GetComponent<NonLocalPlayerInfo>();
                m_NonlocalPlayerInfo.SetTarget(PhotonNetwork.LocalPlayer, transform);
            }
        }

        public void Disconnecting()
        {
            m_NonlocalPlayerInfo?.RemoveSetTarget(); // Somehow null??????????? 
        }

        public void OnHealthChanged(object sender, HealthDeltaEventArgs e)
        {
            //m_HealthDisplay.fillAmount = e.Health / e.MaxHealth; // <= Used if health is displayed as a bar
            m_HealthDisplay.text = $"Health: {e.Health}";
        }
    }
}
