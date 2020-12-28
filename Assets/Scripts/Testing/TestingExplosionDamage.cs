using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon.Attributes;

public class TestingExplosionDamage : MonoBehaviour
{
    public float m_Fuse;
    public float m_Radius;
    public float m_MaxDamage;

    private void Awake()
    {
        StartCoroutine(DetinationTimer());
    }

    private IEnumerator DetinationTimer()
    {
        yield return new WaitForSeconds(m_Fuse);
        Explode(m_Radius, m_MaxDamage);
    }

    // This method can be used independently in other scripts
    public void Explode(float radius, float maxDamage)
    {
        foreach(Collider collider in Physics.OverlapSphere(transform.position, radius))
        {
            HealthAttribute healthAtt = collider.transform.GetComponent<HealthAttribute>();
            if (healthAtt == null) continue;
            healthAtt.ApplyHealthDelta(-maxDamage * ( 1 - (Vector3.Distance(healthAtt.transform.position, transform.position) / radius)));
        }
        Destroy(gameObject);
    }
}
