using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.Utilities.ExtensionMethods
{
    public static class UnityExtensionMethods
    {
        /// <summary>
        /// A helper method to find the first type of T available in the scene.
        /// NOTE: Can be expensive for big scenes with lots of hierarchy structures.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static T GetScriptOfType<T>(this Object _) where T : class
        {
            var scripts = Object.FindObjectsOfType<MonoBehaviour>();

            foreach (var script in scripts)
            {
                if (script is T)
                    return script as T;
            }

            return default(T);
        }

        /// <summary>
        /// A helper method to find all types of type T (including interfaces).
        /// NOTE: Can be expensive for big scenes with lots of hierarchy structures.
        /// </summary>
        /// <typeparam name="T">The type of object it needs to find</typeparam>
        public static List<T> GetAllTypesOf<T>(this Object _, bool includeInactive = true)
        {
            var types = new List<T>();
            var root = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var child in root)
            {
                // Skip children that are already inactive when not including inactive objects.
                if (!includeInactive && !child.activeSelf)
                    continue;

                types.AddRange(child.GetComponentsInChildren<T>(includeInactive));
            }

            return types;
        }
    }
}
