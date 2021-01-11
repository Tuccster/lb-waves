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
        public enum VisualType { Pullout, Shoot, Reload };

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
            ExecuteVisuals(VisualType.Pullout);
        }

        public void ExecuteVisuals(VisualType vType)
        {
            ViewModelController curViewModel = m_ViewModels[m_ActiveViewModelIndex];

            switch (vType)
            {
                case VisualType.Pullout:
                    //curViewModel.m_Animator?.Play("pullout", 0, 0);
                    curViewModel.m_AudioSource.PlayOneShot(m_ActiveGun.clipReload);
                    break;

                case VisualType.Shoot:
                    Transform shootPoint = curViewModel.m_ShootPoint;
                    Destroy(Instantiate(m_MuzzleFlash, shootPoint.position, shootPoint.rotation, shootPoint), 0.5f);
                    curViewModel.m_Animator?.Play("shoot", 0, 0);
                    curViewModel.m_AudioSource?.PlayOneShot(m_ActiveGun.clipShoot);
                    break;

                case VisualType.Reload:
                    curViewModel.m_Animator?.Play("reload", 0, 0);
                    curViewModel.m_AudioSource?.PlayOneShot(m_ActiveGun.clipReload);
                    break;
            }
        }
    }
}

