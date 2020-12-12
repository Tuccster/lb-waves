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
        public Text masterClient;

        private void Start()
        {
            masterClient.text = "master_client : " + PhotonNetwork.IsMasterClient + "\napp_id : " + Application.identifier;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                PhotonNetwork.LeaveRoom();
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
