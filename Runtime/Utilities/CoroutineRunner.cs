using UnityEngine;

namespace SBN.Utilities
{
    /// <summary>
    /// An gameobject used for running coroutines, for easy of access.
    /// Is supposed to be used in conjunction with a dependency framework such as "Extenject"
    /// 
    /// EXAMPLE:
    /// 
    /// Container.Bind<CoroutineRunner>()
    ///    .FromNewComponentOnNewGameObject()
    ///    .AsSingle();
    ///    
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
    }
}