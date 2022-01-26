namespace SBN.SceneHelper.Interfaces
{
    public interface ISceneObservable
    {
        void Subscribe<T>(T observer) where T : ISceneObserver;
        void Unsubscribe<T>(T observer) where T : ISceneObserver;
    }
}