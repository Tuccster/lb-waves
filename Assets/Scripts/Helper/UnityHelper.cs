using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public static class UnityHelper
    {
        public static Transform[] GetAllChildTransforms(Transform root)
        {
            return GetComponentFromAllChildren<Transform>(root);
        }

        public static T[] GetComponentFromAllChildren<T>(Transform root) where T : Component
        {
            List<T> cl = new List<T>();
            foreach (Transform t in root.GetComponentsInChildren<Transform>())
            {
                T c = t.GetComponent<T>();
                if (c != null) cl.Add(c);
            }
            return cl.ToArray();
        }

        public static T[] GetComponentFromAllChildren<T>(Transform[] roots) where T : Component
        {
            List<T> cl = new List<T>();
            for (int i = 0; i < roots.Length; i++)
                foreach (Transform t in roots[i].GetComponentsInChildren<Transform>())
                {
                    T c = t.GetComponent<T>();
                    if (c != null) cl.Add(c);
                }
            return cl.ToArray();
        }
    }
}
