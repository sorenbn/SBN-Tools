using SBN.SceneHelper.Interfaces;
using SBN.Utilities.ExtensionMethods;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.SceneHelper.Examples
{
    public class TestObserverSpawn : MonoBehaviour, ISceneObserver, ISceneObserverAsync
    {
        public int ExecutionOrder => -10;

        private void OnEnable()
        {
            var sceneLoader = this.GetScriptOfType<ISceneObservable>();
            sceneLoader?.Subscribe(this);
        }

        private void OnDisable()
        {
            var sceneLoader = this.GetScriptOfType<ISceneObservable>();
            sceneLoader?.Unsubscribe(this);
        }

        public void OnSceneInitialize(Scene scene)
        {
            Debug.Log($"Scene initialize spawner");
        }

        public IEnumerator OnSceneInitializeAsync(Scene scene)
        {
            Debug.Log($"Scene initialize spawner async");
            yield return null;
        }

        public void OnSceneDispose(Scene scene)
        {
            Debug.Log($"Scene dispose spawner");
        }

        public void OnSceneReady(Scene scene)
        {
            Debug.Log($"Scene ready spawner");
        }
    }
}