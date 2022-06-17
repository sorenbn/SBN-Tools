using SBN.Utilities.Attributes;
using UnityEngine;

namespace SBN.UITool.Core.Elements
{
    public class UIWindow : UIElement
    {
        [SerializeField, ReadOnly] private UIWindowId id;
        [SerializeField] private Settings settings;

        private UIElement[] uiElements;

        public UIWindowId Id
        {
            get => id;
#if UNITY_EDITOR
            set => id = value;
#endif
        }

        public override void Setup(UIWindowManager uiManager)
        {
            base.Setup(uiManager);

            uiElements = GetComponentsInChildren<UIElement>();

            // Start at index 1 because 0 is this UIWindow object.
            for (int i = 1; i < uiElements.Length; i++)
                uiElements[i].Setup(uiManager);
        }

        public virtual Settings GetSettings()
        {
            return settings;
        }

        [System.Serializable]
        public struct Settings
        {
            public bool DontDetroyOnLoad;
        }
    }
}