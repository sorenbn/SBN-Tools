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

        [SerializeField]
        private float fakeLoadingDelay = 1.0f;

        public int ExecutionOrder => executionOrder;

        public void OnSceneInitialize(Scene scene)
        {
            Debug.Log($"Scene initialize: {scene.name} | {name}");
        }

        public IEnumerator OnSceneInitializeAsync(Scene scene)
        {
            Debug.Log($"Scene initialize async: {scene.name} | {name}");
            yield return new WaitForSeconds(fakeLoadingDelay); // simulate heavy loading
        }

        public void OnSceneReady(Scene scene)
        {
            Debug.Log($"Scene ready: {scene.name} | {name}");
        }

        public void OnSceneDispose(Scene scene)
        {
            Debug.Log($"Scene dispose: {scene.name} | {name}");
        }
    }
}