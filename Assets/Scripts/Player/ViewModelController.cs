using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    /*
        This class is just a data container for various elements of the first-person viewmodel
    */

    public class ViewModelController : MonoBehaviour
    {
        [Header("Resources")]
        public RaycastGun m_GunReference;
        public Transform m_ShootPoint;
        public AudioSource m_AudioSource;

         // Automatically assigned on Awake()
        [HideInInspector] public GameObject m_ViewModel;
        [HideInInspector] public Animator m_Animator;

        private void Awake()
        {
            m_ViewModel = gameObject;
            m_Animator = GetComponent<Animator>();
        }
    }
}

