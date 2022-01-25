using SBN.SceneManager.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoader : MonoBehaviour, ISceneEventListener, ISceneEventListenerAsync
{
    public int ExecutionOrder => 1;

    public void OnSceneInitialize(Scene scene)
    {
        Debug.Log($"Scene initialize");
    }

    public IEnumerator OnSceneInitializeAsync(Scene scene)
    {
        Debug.Log($"Scene initialize async");
        yield return new WaitForSeconds(2.0f); // simulate heavy loading
    }

    public void OnSceneReady(Scene scene)
    {
        Debug.Log($"Scene ready");
    }

    public IEnumerator OnSceneReadyAsync(Scene scene)
    {
        Debug.Log($"Scene ready async");
        yield return null;
    }

    public void OnSceneDispose(Scene scene)
    {
        Debug.Log($"Scene dispose");
    }

    public IEnumerator OnSceneDisposeAsync(Scene scene)
    {
        Debug.Log($"Scene dispose async");
        yield return null;
    }
}
