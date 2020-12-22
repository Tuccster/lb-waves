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
        public Text masterClient;
        public GameObject prefabPlayer;

        private void Awake()
        {
            masterClient.text = "master_client : " + PhotonNetwork.IsMasterClient + "\napp_id : " + Application.identifier;

            if (PlayerNetworking.localPlayerInstance == null)
                PhotonNetwork.Instantiate(prefabPlayer.name, new Vector3(0f, 50f, 0f), Quaternion.identity, 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                PhotonNetwork.Disconnect();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : dev");
            PhotonNetwork.LoadLevel("dev");
        }
    }
}
