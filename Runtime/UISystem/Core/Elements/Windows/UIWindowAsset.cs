using UnityEngine;

namespace SBN.UITool.Core.Elements.Windows
{
    [CreateAssetMenu(fileName = "UIWindowAsset_NAME", menuName = "SBN/UI Tool/UI Window Asset")]
    public class UIWindowAsset : ScriptableObject
    {
        [SerializeField] private UIWindow prefab;

        public UIWindow Prefab
        {
            get => prefab;
        }
    }
}