using SBN.ThirdParty.DevLocker.Utils;
using UnityEngine;

namespace SBN.UITool.Core.Elements.Windows
{
    [CreateAssetMenu(fileName = "UIWindowAsset_NAME", menuName = "SBN/UI Tool/UI Window Asset")]
    public class UIWindowAsset : ScriptableObject
    {
        [SerializeField] private SceneReference scene;
        [SerializeField] private WindowSettings settings;

        public SceneReference Scene
        {
            get => scene;
        }
        public WindowSettings Settings
        {
            get => settings;
        }

        [System.Serializable]
        public struct WindowSettings
        {
            [Tooltip("Should this window persist between different scenes in order to be availble any time during game lifecycle?")]
            public bool DontDestroyOnLoad;
        }
    }
}