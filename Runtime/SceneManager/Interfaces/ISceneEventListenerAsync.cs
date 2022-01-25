using System.Collections;
using UnityEngine.SceneManagement;

namespace SBN.SceneManager.Interfaces
{
    public interface ISceneEventListenerAsync
    {
        int ExecutionOrder
        {
            get;
        }

        IEnumerator OnSceneInitializeAsync(Scene scene);
        IEnumerator OnSceneDisposeAsync(Scene scene);
        IEnumerator OnSceneReadyAsync(Scene scene);
    }
}