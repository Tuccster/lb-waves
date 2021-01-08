using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon;

public class TestingGetComponentInAllChildren : MonoBehaviour
{
    private void Awake()
    {
        MeshRenderer[] allChildren = UnityHelper.GetComponentFromAllChildren<MeshRenderer>(transform, false);
        foreach(MeshRenderer mr in allChildren)
            mr.enabled = false;
    }
}
