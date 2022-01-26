using SBN.SceneHelper.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.SceneHelper.Examples
{
    public class TestObserver : MonoBehaviour, ISceneObserver, ISceneObserverAsync
    {
        [SerializeField]
        private int executionOrder = 1;

        public int ExecutionOrder => executionOrder;

        public void OnSceneInitialize(Scene scene)
        {
            Debug.Log($"Scene initialize: {name}");
        }

        public IEnumerator OnSceneInitializeAsync(Scene scene)
        {
            Debug.Log($"Scene initialize async: {name}");
            yield return new WaitForSeconds(4.0f); // simulate heavy loading
        }

        public void OnSceneReady(Scene scene)
        {
            Debug.Log($"Scene ready: {name}");
        }

        public IEnumerator OnSceneReadyAsync(Scene scene)
        {
            Debug.Log($"Scene ready async: {name}");
            yield return null;
        }

        public void OnSceneDispose(Scene scene)
        {
            Debug.Log($"Scene dispose: {name}");
        }

        public IEnumerator OnSceneDisposeAsync(Scene scene)
        {
            Debug.Log($"Scene dispose async: {name}");
            yield return null;
        }
    }

}