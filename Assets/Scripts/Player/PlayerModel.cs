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
        public Transform m_ModelsContainer;
        public Transform m_FirstPersonModelPivot;
        public Transform m_ThirdPersonArmPivot;

        [Header("Models")]
        public Transform[] m_ModelFirstPersonRoots;
        public bool m_ShowFirstPersonModel;
        [Space(10)]
        public Transform[] m_ModelThirdPersonRoots;
        public bool m_ShowThirdPersonModel;


        private Vector3 m_Position;
        private Vector3 m_LookDir;
        private Camera m_PlayerCamera;

        private Quaternion m_FirstPersonModelCameraOffset;
        private Quaternion m_ThirdPersonArmCameraOffset;

        private void Awake()
        {
            m_PlayerCamera = Camera.main; // <= May need to go in Start() instead
            ReloadModels();

            m_FirstPersonModelCameraOffset = m_FirstPersonModelPivot.rotation * Quaternion.Inverse(m_PlayerCamera.transform.rotation);
            m_ThirdPersonArmCameraOffset = m_ThirdPersonArmPivot.rotation * Quaternion.Inverse(m_PlayerCamera.transform.rotation);
        }

        // Correctly set the visibilty of the first and third-person model renderers
        public void ReloadModels()
        {
            if (m_ShowFirstPersonModel)
                SetAllChildrenMeshRenderers(UnityHelper.GetComponentFromAllChildren<MeshRenderer>(m_ModelFirstPersonRoots, false), photonView.IsMine);

            if (!m_ShowThirdPersonModel)
                SetAllChildrenMeshRenderers(UnityHelper.GetComponentFromAllChildren<MeshRenderer>(m_ModelThirdPersonRoots, false), !photonView.IsMine);
        }

        private void SetAllChildrenMeshRenderers(MeshRenderer[] renderers, bool state)
        {
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].enabled = state;
        }

        private void Update()
        {
            // Update client's third person model
            if (photonView.IsMine)
            {
                m_ModelsContainer.rotation = Quaternion.Euler(0, m_PlayerCamera.transform.eulerAngles.y, 0);
                m_FirstPersonModelPivot.rotation = m_PlayerCamera.transform.rotation * m_FirstPersonModelCameraOffset;
                m_ThirdPersonArmPivot.rotation = m_PlayerCamera.transform.rotation * m_ThirdPersonArmCameraOffset;
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
                    stream.SendNext(m_ModelsContainer.transform.eulerAngles.y);
                    // Send order -> 1
                    stream.SendNext(m_FirstPersonModelPivot.transform.eulerAngles); // UNOPTIMIZED! SEND FLOAT!
                    // Send order -> 2
                    stream.SendNext(m_PlayerCamera.transform.rotation.eulerAngles); // UNOPTIMIZED! SEND FLOAT!
                    // Send order -> 3
                    stream.SendNext(m_ThirdPersonArmPivot.rotation.eulerAngles);  // UNOPTIMIZED! SEND FLOAT!
                }
                else
                {
                    // Receive order -> 0
                    m_ModelsContainer.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
                    // Receive order -> 1
                    m_FirstPersonModelPivot.rotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
                    // Receive order -> 2
                    m_FaceTrans.rotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
                    // Receive order -> 3
                    m_ThirdPersonArmPivot.rotation = Quaternion.Euler((Vector3)stream.ReceiveNext());
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void OnGUI()
        {
            if (!photonView.IsMine) return;

            //GUI.Label(new Rect(10, 10, 512, 32), $"m_ReceivedBodyRotY -> {m_ReceivedBodyRotY}");
        }
    }
}
