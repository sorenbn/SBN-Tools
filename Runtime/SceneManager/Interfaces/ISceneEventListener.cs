using UnityEngine.SceneManagement;

namespace SBN.SceneManager.Interfaces
{
    public interface ISceneEventListener
    {
        int ExecutionOrder
        {
            get;
        }

        void OnSceneInitialize(Scene scene);
        void OnSceneDispose(Scene scene);
        void OnSceneReady(Scene scene);
    }
}