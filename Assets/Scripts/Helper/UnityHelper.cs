using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon
{
    public static class UnityHelper
    {
        /// <summary>
        /// Returns an array of transforms containing the transform of every single child under the root.
        /// </summary>
        public static Transform[] GetAllChildTransforms(Transform root, bool checkRoot)
        {
            return GetComponentFromAllChildren<Transform>(root, checkRoot);
        }

        /// <summary>
        /// Returns an array of a specified component type containing every single instances of that component that exist under the root.
        /// </summary>
        public static T[] GetComponentFromAllChildren<T>(Transform root, bool checkRoot) where T : Component
        {
            List<T> foundComponents = new List<T>();
            Transform[] children = root.GetComponentsInChildren<Transform>();
            for (int i = Convert.ToInt32(!checkRoot); i < children.Length; i++)
            {
                T component = children[i].GetComponent<T>();
                if (component != null)
                    foundComponents.Add(component);
            }
            return foundComponents.ToArray();

            /*
            List<T> foundComponents = new List<T>();
            Transform[] children = root.GetComponentsInChildren<Transform>();
            if (typeof(T).Equals(typeof(Transform)))
                return children.Skip(Convert.ToInt32(!checkRoot)).Cast<Transform>().ToArray() as T[];
            for (int i = Convert.ToInt32(!checkRoot); i < children.Length; i++)
                if (children[i].GetComponent<T>() != null)
                    foundComponents.Add(children[i].GetComponent<T>());
            return foundComponents.ToArray();
            */
        }

        /// <summary>
        /// Returns an array of a specified component type containing every single instances of that component that exist under all given roots.
        /// (Duplicates will be present if a root is given that is a child of another root given)
        /// </summary>
        public static T[] GetComponentFromAllChildren<T>(Transform[] roots, bool checkRoots) where T : Component
        {
            List<T> cl = new List<T>();
            for (int i = 0; i < roots.Length; i++)
                cl.AddRange(GetComponentFromAllChildren<T>(roots[i], checkRoots));
            return cl.ToArray();
        }

        /// <summary>
        /// Returns whether or not a given GameObject has the given component
        /// </summary>
        public static bool HasComponent<T>(Transform transform) where T : Component
        {
            return transform.GetComponent<T>() != null;
        }
    }
}
