using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelConfigurator : MonoBehaviour
{
    [Header("Settings")]
    public AutoConfigCallType m_AutoConfig;
    public enum AutoConfigCallType { Awake, Start, None };

    [Header("Resources")]
    public ModelObjects[] m_ModelObjects;

    // Configure on Awake
    private void Awake() { if (m_AutoConfig == AutoConfigCallType.Awake) ConfigureModel(); }

    // Configure on Start
    private void Start() { if (m_AutoConfig == AutoConfigCallType.Start) ConfigureModel(); }

    public void ConfigureModel()
    {
        for (int i = 0; i < m_ModelObjects.Length; i++)
        {
            
        }
    }

    [System.Serializable]
    public class ModelObjects
    {
        public GameObject m_GameObject;
        public bool m_DisableCollider;
        public bool m_DisableRenderer;
        public bool m_DisableShaddowCasting;
    }
}
