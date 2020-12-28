using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Lemon
{
    /*
        This class is used to handle anything relating the player's model, both first
        and third person. This class is alse networked so that it can properly sync
        the models' visuals between players.
    */

    public class PlayerModel : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Model Rigs")]
        public Transform m_FaceTrans;
        public Transform m_BodyTrans;

        [Header("Model Renderers")]
        public MeshRenderer[] m_ModelFirstPerson;
        public MeshRenderer[] m_ModelThirdPerson;

        private Vector3 m_Position;
        private Vector3 m_LookDir;
        private Camera m_PlayerCamera;

        private void Awake()
        {
            m_PlayerCamera = Camera.main; // <= May need to go in Start() instead

            // Correctly set the visibilty of the first and third-person model renderers
            for (int i = 0; i < m_ModelFirstPerson.Length; i++)
                if (m_ModelFirstPerson[i] != null)
                    m_ModelFirstPerson[i].enabled = photonView.IsMine;
            for (int i = 0; i < m_ModelThirdPerson.Length; i++)
                if (m_ModelThirdPerson[i] != null)
                    m_ModelThirdPerson[i].enabled = !photonView.IsMine;
        }

        private void Update()
        {
            m_Position = transform.position;
            m_LookDir = m_PlayerCamera.transform.forward;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            try
            {
                // Sync this player's third-person model for other players 
                if (stream.IsWriting)
                {
                    // Send order -> 0
                    stream.SendNext(m_PlayerCamera.transform.rotation.y);
                    // Send order -> 1
                    stream.SendNext(m_LookDir);
                }
                else
                {
                    // Receive order -> 0
                    m_BodyTrans.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
                    // Receive order -> 1
                    this.m_LookDir = (Vector3)stream.ReceiveNext();
                    m_FaceTrans.forward = m_LookDir;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
