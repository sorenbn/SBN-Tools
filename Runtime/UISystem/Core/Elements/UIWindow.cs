using SBN.UITool.Core.Managers;
using SBN.Utilities.Attributes;
using UnityEngine;

namespace SBN.UITool.Core.Elements
{
    /// <summary>
    /// Base class for any specific UI window you want to able to show
    /// on screen. 
    /// 
    /// All UI windows must be added to the UI Window Container scriptable object
    /// and every time a new UI Window is added, the "Generate Ids" button on 
    /// the scriptable object must be clicked to update the enum list of ids.
    /// </summary>
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

        public override void Setup(UIManager uiManager)
        {
            base.Setup(uiManager);

            uiElements = GetComponentsInChildren<UIElement>();

            // Start at index 1 because 0 is this UIWindow object
            // since GetComponentsInChildren also gets on the object itself
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
            [Tooltip("Should this window persist between different scenes in order to be availble any time during game lifecycle?")]
            public bool DontDetroyOnLoad;
        }
    }
}