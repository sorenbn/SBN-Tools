using SBN.UITool.Core;
using SBN.Utilities.Attributes;
using UnityEngine;

namespace SBN.UITool
{
    public class UIWindow : UIElement
    {
        [SerializeField] private UIWindowId id;

        public UIWindowId Id
        {
            get => id;
#if UNITY_EDITOR
            set => id = value;
#endif
        }
    } 
}