using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBN.Utilities.ExtensionMethods
{
    public static class UnityExtensionMethods
    {
        /// <summary>
        /// A helper method to find the first type of T available in the scene.
        /// NOTE: Can be expensive for big scenes with lots of hierarchy structures.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetScriptOfType<T>(this Component component) where T : class
        {
            var scripts = Object.FindObjectsOfType<MonoBehaviour>().Where(x => x.gameObject.scene.buildIndex == component.gameObject.scene.buildIndex);

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
        public static List<T> GetAllTypesOf<T>(this Component component, bool includeInactive = true)
        {
            var types = new List<T>();
            var root = component.gameObject.scene.GetRootGameObjects();

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
