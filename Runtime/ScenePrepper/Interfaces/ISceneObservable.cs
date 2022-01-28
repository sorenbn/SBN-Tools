using System;
using UnityEngine.SceneManagement;

namespace SBN.SceneHelper.Interfaces
{
    /// <summary>
    /// Interface for listening to scene specific events.
    /// </summary>
    public interface ISceneObservable
    {
        event Action<Scene> OnSceneIntialize;
        event Action<Scene> OnSceneReady;

        void Subscribe<T>(T observer) where T : ISceneObserver;
        void Unsubscribe<T>(T observer) where T : ISceneObserver;
    }
}