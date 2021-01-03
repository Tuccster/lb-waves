using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class ServerSceneManager : MonoBehaviourPunCallbacks
    {   
        [Header("Resources")]
        public GameObject prefabPlayer;

        private void Awake()
        {
            if (PlayerNetworking.localPlayerInstance == null)
                PhotonNetwork.Instantiate(prefabPlayer.name, new Vector3(0f, 50f, 0f), Quaternion.identity, 0);
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel("menu");
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.LoadLevel("dev");
        }
    }
}
