using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    /*
        This class is used only to update the weapon models for first person, and possibly third person views.
    */

    [RequireComponent(typeof(Animator))]
    public class PlayerWeaponModelController : MonoBehaviour
    {
        [Header("Resources")]
        public ParticleSystem m_MuzzleFlash;
        public ViewModelController[] m_ViewModels;

        private RaycastGun m_ActiveGun;
        private int m_ActiveViewModelIndex = 0;
        public enum VisualType { Shoot, Reload };

        private void Awake()
        {
            //SetViewModel(null);
        }

        public void SetViewModel(RaycastGun gun)
        {
            m_ActiveGun = gun;
            for (int i = 0; i < m_ViewModels.Length; i++)
            {
                if (m_ViewModels == null) continue;
                if (gun?.id == m_ViewModels[i].m_GunReference.id)
                {
                    m_ViewModels[i].m_ViewModel?.SetActive(true); // Enable model if ids match
                    m_ActiveViewModelIndex = i;
                }
                else
                    m_ViewModels[i].m_ViewModel?.SetActive(false);
            }
        }

        public void ExecuteVisuals(VisualType vType)
        {
            switch (vType)
            {
                case VisualType.Shoot:
                    Transform shootPoint = m_ViewModels[m_ActiveViewModelIndex].m_ShootPoint;
                    Destroy(Instantiate(m_MuzzleFlash, shootPoint.position, shootPoint.rotation, shootPoint), 1);
                    m_ViewModels[m_ActiveViewModelIndex].m_Animator?.Play("shoot", 0, 0);
                    break;

                case VisualType.Reload:
                    m_ViewModels[m_ActiveViewModelIndex].m_Animator?.Play("reload", 0, 0);
                    break;
            }
        }
    }
}

