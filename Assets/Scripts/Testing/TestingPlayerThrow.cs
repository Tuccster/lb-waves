using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Lemon
{
    public class TestingPlayerThrow : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Settings")]
        public float m_ThrowPower;
        public float m_ThrowCooldown;

        [Header("Resources")]
        public GameObject m_GrenadePrefab;

        private Camera m_MainCamera;
        private Coroutine m_ThrowCoroutine;

        private void Start()
        {
            m_MainCamera = Camera.main;
        }

        private void Update()
        {
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (m_ThrowCoroutine == null)
                    m_ThrowCoroutine = StartCoroutine(ThrowCoroutine());
            }
        }

        private IEnumerator ThrowCoroutine()
        {
            GameObject curGrenade = PhotonNetwork.Instantiate(m_GrenadePrefab.name, m_MainCamera.transform.position, Quaternion.identity);
            curGrenade.GetComponent<Rigidbody>().AddForce(m_MainCamera.transform.forward * m_ThrowPower);
            yield return new WaitForSeconds(m_ThrowCooldown);
            m_ThrowCoroutine = null;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }
    }   
}
