using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    /*
        This class is used only to update the weapon models for first person, and possibly third person views.
    */

    public class PlayerWeaponModelController : MonoBehaviour
    {
        [Header("Resources")]
        public ParticleSystem m_MuzzleFlash;
        public ViewModel[] m_ViewModels;
        
        private RaycastGun m_ActiveGun;
        private int m_ActiveViewModelIndex = 0;

        private void Awake()
        {
            SetViewModel(null);
        }

        public void SetViewModel(RaycastGun gun)
        {
            m_ActiveGun = gun;
            for (int i = 0; i < m_ViewModels.Length; i++)
            {
                if (gun?.id == m_ViewModels[i].id)
                {
                    m_ViewModels[i].model?.SetActive(true); // Enable model if ids match
                    m_ActiveViewModelIndex = i;
                }
                else
                    m_ViewModels[i].model?.SetActive(false);
            }
        }

        public void ExectuteShootVisuals()
        {
            Transform shootPoint =  m_ViewModels[m_ActiveViewModelIndex].shootPoint;
            Destroy(Instantiate(m_MuzzleFlash, shootPoint.position, shootPoint.rotation, shootPoint), 1);

            m_ViewModels[m_ActiveViewModelIndex].animator?.Play("shoot");
        }

        public void ExecuteReloadVisuals()
        {

        }

        // Make ViewModel a MonoBehaviour so that each viewmodel can deal with its own information, rather than dealing with a messy
        // array on this script. 

        [System.Serializable]
        public class ViewModel
        {
            public string id;
            public GameObject model;
            public Transform shootPoint;
            public Animator animator;
        }
    }
}

