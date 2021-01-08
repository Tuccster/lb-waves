using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lemon;

public class TestingGetComponentInAllChildren : MonoBehaviour
{
    private void Awake()
    {
        Transform[] allChildren = UnityHelper.GetAllChildTransforms(transform, true);
        Debug.Log($"children_found -> {allChildren.Length}");
        for (int i = 0; i < allChildren.Length; i++)
            Debug.Log($"array_index -> {i} | child_name -> {allChildren[i].name} | child_id -> {allChildren[i].GetInstanceID()}");
    }
}
