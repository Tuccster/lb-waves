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

        public Text networkStatus;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnConnectedToMaster()
		{
            networkStatus.text = "Status: Connected to master";
            PhotonNetwork.JoinRandomRoom();
		}

        public override void OnJoinRandomFailed(short returnCode, string message)
        {   
            networkStatus.text = "Status: Unable to join room";
            PhotonNetwork.CreateRoom(null, new RoomOptions());
        }

        public override void OnJoinedRoom()
        {
            networkStatus.text = "Status: Joined room";

            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel(1);
        }
    }
}
