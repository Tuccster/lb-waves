using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon.Attributes;

namespace Lemon
{
    public class RaycastShooter : MonoBehaviour
    {
        [Header("RaycastShooter Settings")]
        public float m_FireCooldown;
        public float m_MaxDistance;
        public float m_MaxDamage;
        public float m_DamageFalloffStartDistance;
        public float m_DamageFalloffPercentPerUnit;
        public DamageFalloff m_DamageFalloffType;

        public enum DamageFalloff { Linear, Exponential }

        [Header("RaycastShooter Resources")]
        public Transform m_ShootPoint;

        private WaitForSeconds m_WaitForSeconds;
        private IEnumerator m_ShootEnumerator;

        private void Awake()
        {
            m_WaitForSeconds = new WaitForSeconds(m_FireCooldown);
        }

        public virtual void Shoot(Vector3 direction)
        {
            if (m_ShootEnumerator == null)
            {
                m_ShootEnumerator = ShootEnumerator(direction);
                StartCoroutine(m_ShootEnumerator);
            }
        }

        private IEnumerator ShootEnumerator(Vector3 direction)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
            {
                HealthAttribute healthAtt = hit.transform.GetComponent<HealthAttribute>();
                if (healthAtt != null)
                    healthAtt.ApplyHealthDelta(((hit.distance / m_MaxDistance) * m_MaxDamage) * Mathf.Pow(m_DamageFalloffPercentPerUnit, (int)hit.distance));

                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }

            yield return m_WaitForSeconds;
            m_ShootEnumerator = null;
        }
    }
}
