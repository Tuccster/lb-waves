using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Lemon
{
    public class PlayerNetworking : MonoBehaviourPunCallbacks, IPunObservable
    {
        private float health = 100.0f;

        [Header("Resources")]
        public TextMeshPro username;
        //public PhotonTransformViewClassic photonTransformView;
        public Rigidbody playerRigidbody;
        public MonoBehaviour[] localOnlyScripts;
        public GameObject[] localOnlyGameObjects;

        public static GameObject localPlayerInstance;

        private void Awake()
        {
            if (photonView.IsMine)
                PlayerNetworking.localPlayerInstance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            username.text = photonView.Owner.NickName;
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                for (int i = 0; i < localOnlyScripts.Length; i++)
                    localOnlyScripts[i].enabled = false;

                for (int i = 0; i < localOnlyGameObjects.Length; i++)
                    localOnlyGameObjects[i].SetActive(false);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(health);
            }
            else
            {
                // Network player, receive data
                this.health = (float)stream.ReceiveNext();
            }
        }

        private void Update()
        {
            //photonTransformView.SetSynchronizedValues(playerRigidbody.velocity, playerRigidbody.angularVelocity.magnitude);
        }

        void OnGUI()
        {
            if (photonView.IsMine)
                GUI.Label(new Rect(10, 32, 512, 32), $"photonTransformView.SetSynchronizedValues({playerRigidbody.velocity}, {playerRigidbody.angularVelocity.magnitude})");
        }
    }
}
