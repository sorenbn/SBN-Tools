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

        private void Awake()
        {
            if (onlyEvaluateChildren)
                GetObserversInChildren();
            else
                GetObserversInScene();
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
            if (initializeRoutine != null)
            {
                Debug.LogWarning($"WARNING: Scene is already being initialized");
                return;
            }

            initializeRoutine = StartCoroutine(InitializeScene(scene));
        }

        private IEnumerator InitializeScene(Scene scene)
        {
            OnSceneIntialize?.Invoke(scene);

            for (int i = 0; i < sceneObservers.Count; i++)
                sceneObservers[i].OnSceneInitialize(scene);

            for (int i = 0; i < sceneObserversAsync.Count; i++)
                yield return sceneObserversAsync[i].OnSceneInitializeAsync(scene);

            for (int i = 0; i < sceneObservers.Count; i++)
                sceneObservers[i].OnSceneReady(scene);

            OnSceneReady?.Invoke(scene);
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