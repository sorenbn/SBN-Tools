using System.Collections;
using UnityEngine.SceneManagement;

namespace SBN.SceneHelper.Interfaces
{
    public interface ISceneObserverAsync
    {
        /// <summary>
        /// Determines the order of execution. Lower numbers are executed first.
        /// </summary>
        int ExecutionOrder
        {
            get;
        }

        /// <summary>
        /// Gets called when a scene is loaded and initialization has begun
        /// </summary>
        /// <param name="scene"></param>
        IEnumerator OnSceneInitializeAsync(Scene scene);
    }
}