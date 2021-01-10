using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon.Attributes;

namespace Lemon
{
    /*
        This class in used for the actual shooting mechanics as well as calling methods to update the
        PlayerWeaponModelController's models.
    */

    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("Settings")]
        public float m_RayStartDist;

        [Header("Resources")]
        public PlayerWeaponModelController m_FirstPersonModelController;
        public RaycastGun m_gun1;
        public RaycastGun m_gun2;

        private WaitForSeconds m_WaitForSeconds;
        private IEnumerator m_ShootEnumerator;
        private Camera m_PlayerCamera;

        protected RaycastGun m_ActiveGun;

        private void Awake()
        {
            m_PlayerCamera = Camera.main; // May have to aquire in Start()
            SetActiveGun(m_gun1);
        }

        private void Update()
        {
            // TEMP GUN SWITCHING
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetActiveGun(m_gun1);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SetActiveGun(m_gun2);

            // TEMP SHOOTING
            bool shootThisFrame = m_gun1.automatic ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            if (shootThisFrame)
            {
                Vector3 startingPos = m_PlayerCamera.transform.position + (m_PlayerCamera.transform.forward * m_RayStartDist);
                Shoot(startingPos, m_PlayerCamera.transform.forward, m_ActiveGun);                
            }
        }

        public void SetActiveGun(RaycastGun newGun)
        {
            m_ActiveGun = newGun;
            m_FirstPersonModelController.SetViewModel(newGun);
            m_WaitForSeconds = new WaitForSeconds(newGun.fireCooldown);
        }

        public void Shoot(Vector3 startPos, Vector3 direction, RaycastGun gun)
        {
            if (m_ShootEnumerator == null)
            {
                m_ShootEnumerator = ShootEnumerator(startPos, direction, gun);
                StartCoroutine(m_ShootEnumerator);
            }
        }

        private IEnumerator ShootEnumerator(Vector3 startPos, Vector3 direction, RaycastGun gun)
        {
            // Original formula: -\left(a-\left(-a\frac{x}{m}\right)\cdot\left(1-\frac{m}{m\left(1-l\right)^{x}}\right)\right)
            // Simplified      : a \left(f \sqrt{m} x (1-l)^x-\frac{x}{m}-1\right)
            RaycastHit hit;
            if (Physics.Raycast(startPos, direction, out hit, gun.maxDistance))
            {
                HealthAttribute healthAtt = hit.transform.GetComponent<HealthAttribute>();
                if (healthAtt != null)
                {
                    float baseDamage = -gun.maxDamage * (hit.distance / gun.maxDistance);
                    float falloff = 1 - (gun.maxDistance / (gun.maxDistance * Mathf.Pow(1 - (gun.falloffPercentPerUnit * 0.01f), (int)hit.distance)));
                    float final = -(gun.maxDamage - (baseDamage * falloff));
                    Debug.Log(final.ToString("0.0000"));
                    healthAtt.ApplyHealthDelta(final);
                }

                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(direction * gun.force);
                }
            }
            Debug.DrawRay(startPos, direction * hit.distance, Color.yellow, 10);

            yield return m_WaitForSeconds;
            m_ShootEnumerator = null;
        }
    }
}
