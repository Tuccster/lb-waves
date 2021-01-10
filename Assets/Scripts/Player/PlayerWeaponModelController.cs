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
        public ViewModel[] m_ViewModels;

        private void Awake()
        {
            SetViewModel(SetActive.None);
        }

        public void SetViewModel(RaycastGun gun)
        {
            if (gun == null) return;
            for (int i = 0; i < m_ViewModels.Length; i++)
                m_ViewModels[i].model.SetActive(gun.id == m_ViewModels[i].id); // Enable model if ids match
        }

        public enum SetActive { All, None }
        public void SetViewModel(SetActive set)
        {
            for (int i = 0; i < m_ViewModels.Length; i++)
                m_ViewModels[i].model.SetActive(Convert.ToBoolean(set));
        }

        [System.Serializable]
        public class ViewModel
        {
            public string id;
            public GameObject model;
        }
    }
}

