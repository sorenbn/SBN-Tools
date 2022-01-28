using UnityEngine.SceneManagement;

namespace SBN.SceneHelper.Interfaces
{
    /// <summary>
    /// Interface to be invoked by the ISceneObservable.
    /// </summary>
    public interface ISceneObserver
    {
        /// <summary>
        /// Determines the order of execution. Lower numbers are executed first.
        /// </summary>
        int ExecutionOrder
        {
            get;
        }

        /// <summary>
        /// Gets called when a scene is loaded and initialization has begun.
        /// </summary>
        /// <param name="scene"></param>
        void OnSceneInitialize(Scene scene);

        /// <summary>
        /// Gets called when a scene has finished all the initialization of all scene observers.
        /// </summary>
        /// <param name="scene"></param>
        void OnSceneReady(Scene scene);
    }
}