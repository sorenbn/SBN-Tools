using SBN.SceneHelper.Interfaces;
using SBN.Utilities.ExtensionMethods;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace SBN.SceneHelper.Core
{
    /// <summary>
    /// A system to help prepare scenes with heavy loading
    /// or just a better flow of control when changing scenes 
    /// and which objects are loaded and prepared in which order.
    /// Use the standard SceneManager.LoadScene()
    /// 
    /// Only one of these must exist in each scene.
    /// </summary>
    public class ScenePrepper : MonoBehaviour, ISceneObservable
    {
        public event Action<Scene> OnSceneIntialize;
        public event Action<Scene> OnSceneReady;

        [SerializeField]
        [Tooltip("This determines if the Scene Loader should look for event listeners only in child objects or the whole scene hierarchy")]
        private bool onlyEvaluateChildren;

        [SerializeField]
        private bool includeInactiveObjects = true;

        private List<ISceneObserver> sceneObservers;
        private List<ISceneObserverAsync> sceneObserversAsync;
        private Coroutine initializeRoutine;

        private bool initializing;

        private void Awake()
        {
            if (onlyEvaluateChildren)
                GetObserversInChildren();
            else
                GetObserversInScene();
        }

        private void Start()
        {
            if (initializing)
            {
                StartCoroutine(c_WaitForSceneReady());
                return;
            }

            ReadyScene();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void GetObserversInChildren()
        {
            sceneObservers = GetComponentsInChildren<ISceneObserver>(includeInactiveObjects).OrderBy(x => x.ExecutionOrder).ToList();
            sceneObserversAsync = GetComponentsInChildren<ISceneObserverAsync>(includeInactiveObjects).OrderBy(x => x.ExecutionOrder).ToList();
        }

        private void GetObserversInScene()
        {
            sceneObservers = this.GetAllTypesOf<ISceneObserver>(includeInactiveObjects).OrderBy(x => x.ExecutionOrder).ToList();
            sceneObserversAsync = this.GetAllTypesOf<ISceneObserverAsync>(includeInactiveObjects).OrderBy(x => x.ExecutionOrder).ToList();
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (scene.name != gameObject.scene.name)
                return;

            if (initializeRoutine != null)
            {
                Debug.LogWarning($"WARNING: Scene is already being initialized");
                return;
            }

            initializeRoutine = StartCoroutine(c_InitializeScene(scene));
        }

        private void ReadyScene()
        {
            var activeScene = gameObject.scene;

            for (int i = 0; i < sceneObservers.Count; i++)
                sceneObservers[i].OnSceneReady(activeScene);

            OnSceneReady?.Invoke(activeScene);
        }

        private IEnumerator c_InitializeScene(Scene scene)
        {
            initializing = true;

            OnSceneIntialize?.Invoke(scene);

            for (int i = 0; i < sceneObservers.Count; i++)
                sceneObservers[i].OnSceneInitialize(scene);

            for (int i = 0; i < sceneObserversAsync.Count; i++)
                yield return sceneObserversAsync[i].OnSceneInitializeAsync(scene);

            initializing = false;
        }

        private IEnumerator c_WaitForSceneReady()
        {
            while (initializing)
                yield return null;

            ReadyScene();
        }

        public void Subscribe<T>(T observer) where T : ISceneObserver
        {
            sceneObservers.Add(observer);
            sceneObservers = sceneObservers.OrderBy(x => x.ExecutionOrder).ToList();
        }

        public void Unsubscribe<T>(T observer) where T : ISceneObserver
        {
            sceneObservers.Remove(observer);
        }
    }
}