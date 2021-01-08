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
        [Header("Resoureces")]
        public Transform m_CameraControls;

        [Header("Model Rigs")]
        public Transform m_FaceTrans;
        public Transform m_ModelContainer;
        public Transform m_FirstPersonArmsPivot;

        [Header("Models")]
        public Transform[] m_ModelFirstPersonRoots;
        public bool m_ShowFirstPersonModel;
        [Space(10)]
        public Transform[] m_ModelThirdPersonRoots;
        public bool m_ShowThirdPersonModel;


        private Vector3 m_Position;
        private Vector3 m_LookDir;
        private Camera m_PlayerCamera;

        private Quaternion m_ArmCameraOffset;

        private void Awake()
        {
            m_PlayerCamera = Camera.main; // <= May need to go in Start() instead
            ReloadModels();

            m_ArmCameraOffset = m_FirstPersonArmsPivot.rotation * Quaternion.Inverse(m_PlayerCamera.transform.rotation);
        }

        // Correctly set the visibilty of the first and third-person model renderers
        public void ReloadModels()
        {
            if (m_ShowFirstPersonModel)
                SetMeshRendererState(UnityHelper.GetComponentFromAllChildren<MeshRenderer>(m_ModelFirstPersonRoots, true), photonView.IsMine);

            if (!m_ShowThirdPersonModel)
                SetMeshRendererState(UnityHelper.GetComponentFromAllChildren<MeshRenderer>(m_ModelThirdPersonRoots, true), !photonView.IsMine);
        }

        private void SetMeshRendererState(MeshRenderer[] renderers, bool state)
        {
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].enabled = state;
        }

        private void Update()
        {
            // Update client's third person model
            if (photonView.IsMine)
            {
                m_ModelContainer.rotation = Quaternion.Euler(0, m_PlayerCamera.transform.eulerAngles.y, 0);
                m_FirstPersonArmsPivot.rotation = m_PlayerCamera.transform.rotation * m_ArmCameraOffset;
                m_Position = transform.position;
                m_LookDir = m_PlayerCamera.transform.forward;
            }
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            try
            {
                // Sync this player's third-person model for other players 
                if (stream.IsWriting)
                {
                    // Send order -> 0
                    stream.SendNext(m_ModelContainer.transform.eulerAngles.y);
                    // Send order -> 1
                    stream.SendNext(m_FirstPersonArmsPivot.transform.eulerAngles); // ideally we could just send a float
                    // Send order -> 2
                    stream.SendNext(m_LookDir);
                }
                else
                {
                    // Receive order -> 0
                    m_ModelContainer.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
                    // Receive order -> 1
                    m_FirstPersonArmsPivot.rotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
                    // Receive order -> 2
                    this.m_LookDir = (Vector3)stream.ReceiveNext();
                    m_FaceTrans.forward = m_LookDir;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 512, 32), $"photonView.IsMine -> {photonView.IsMine}");
            //GUI.Label(new Rect(10, 10, 512, 32), $"m_ReceivedBodyRotY -> {m_ReceivedBodyRotY}");
        }
    }
}
