using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lemon.Attributes;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class PlayerUI : MonoBehaviourPunCallbacks
    {
        [Header("Resources")]
        public GameObject m_NonlocalPlayerUI;
        public PlayerNetworking m_PlayerNetworking; // Should be dependency injected
        public HealthAttribute m_HealthAttribute; // Should be dependency injected
        public Text m_HealthDisplay;

        [HideInInspector] public NonLocalPlayerInfo m_NonlocalPlayerInfo;

        private void Awake()
        {
            m_HealthAttribute.HealthChanged += OnHealthChanged;
            m_HealthAttribute.ForceUpdate();

            m_PlayerNetworking.Disconnecting += DestoryNonLocalPlayerInfo;

            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                m_NonlocalPlayerInfo = Instantiate(m_NonlocalPlayerUI).GetComponent<NonLocalPlayerInfo>();
                m_NonlocalPlayerInfo.SetTarget(photonView.Owner.NickName, transform);
            }
        }

        public void OnHealthChanged(object sender, HealthDeltaEventArgs e)
        {
            //m_HealthDisplay.fillAmount = e.Health / e.MaxHealth; // <= Used if health is displayed as a bar
            m_HealthDisplay.text = $"Health: {e.Health}";
        }

        [PunRPC]
        public void DestoryNonLocalPlayerInfo(object sender, EventArgs e)
        {
            photonView.RPC("DestoryNonLocalPlayerInfo", RpcTarget.All);
            m_NonlocalPlayerInfo?.RemoveTarget();
        }
    }
}
