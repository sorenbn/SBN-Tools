using UnityEngine;

namespace SBN.UITool.Core.Elements.Windows
{
    [CreateAssetMenu(fileName = "UIWindowAsset_NAME", menuName = "SBN/UI Tool/UI Window Asset")]
    public class UIWindowAsset : ScriptableObject
    {
        [SerializeField] private UIWindow prefab;
        [SerializeField] private UIWindowSettings settings;

        public UIWindow Prefab
        {
            get => prefab;
        }
        public UIWindowSettings Settings
        {
            get => settings;
        }

        [System.Serializable]
        public struct UIWindowSettings
        {
            [Tooltip("Should this window persist between different scenes in order to be availble any time during game lifecycle?")]
            public bool DontDestroyOnLoad;
        }
    }
}