using SBN.Utilities.Attributes;
using UnityEngine;

namespace SBN.UITool.Core.Elements
{
    public class UIWindow : UIElement
    {
        [SerializeField, ReadOnly] private UIWindowId id;

        public UIWindowId Id
        {
            get => id;
#if UNITY_EDITOR
            set => id = value;
#endif
        }
    } 
}