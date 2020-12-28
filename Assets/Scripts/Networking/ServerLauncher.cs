using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class ServerLauncher : MonoBehaviourPunCallbacks
    {
        // This should be moved to a more general game information class
        private static string gameVersion = "20w50b";

        public Button connectButton;
        public Text connectButtonText;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Connect()
        {
            Debug.Log("Attempting to connect...");
            connectButton.interactable = false;
            connectButtonText.text = "CONNECTING...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
		{
            PhotonNetwork.JoinRandomRoom();
		}

        public override void OnJoinRandomFailed(short returnCode, string message)
        {   
            PhotonNetwork.CreateRoom(null, new RoomOptions());
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel(1);
        }
    }
}
