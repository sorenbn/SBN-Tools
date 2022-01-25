using SBN.SceneManager.Interfaces;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.SceneManager.Core
{
    public class SceneManager : MonoBehaviour
    {
        private List<ISceneEventListener> sceneEventListeners;
        private List<ISceneEventListenerAsync> sceneEventListenersAsync;

        private void Awake()
        {
            sceneEventListeners = GetTypesOf<ISceneEventListener>().OrderBy(x => x.ExecutionOrder).ToList();
            sceneEventListenersAsync = GetTypesOf<ISceneEventListenerAsync>().OrderBy(x => x.ExecutionOrder).ToList();
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            StartCoroutine(InitializeScene(scene));
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            StartCoroutine(DisposeScene(scene));
        }

        private IEnumerator InitializeScene(Scene scene)
        {
            for (int i = 0; i < sceneEventListeners.Count; i++)
                sceneEventListeners[i].OnSceneInitialize(scene);

            for (int i = 0; i < sceneEventListenersAsync.Count; i++)
                yield return sceneEventListenersAsync[i].OnSceneInitializeAsync(scene);

            for (int i = 0; i < sceneEventListeners.Count; i++)
                sceneEventListeners[i].OnSceneReady(scene);

            for (int i = 0; i < sceneEventListenersAsync.Count; i++)
                yield return sceneEventListenersAsync[i].OnSceneReadyAsync(scene);
        }

        private IEnumerator DisposeScene(Scene scene)
        {
            for (int i = 0; i < sceneEventListeners.Count; i++)
                sceneEventListeners[i].OnSceneDispose(scene);

            for (int i = 0; i < sceneEventListenersAsync.Count; i++)
                yield return sceneEventListenersAsync[i].OnSceneDisposeAsync(scene);
        }

        private List<T> GetTypesOf<T>()
        {
            var types = new List<T>();
            var root = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var child in root)
                types.AddRange(child.GetComponentsInChildren<T>(true));

            return types;
        }
    }
}