using SBN.UITool.Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SBN.UITool.Core.Elements.Windows
{
    /// <summary>
    /// Base class for any specific UI window you want to able to show
    /// on screen. 
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class UIWindow : UIElement
    {
        [Tooltip("A default back button which will trigger to go back in window history. Can be null if no back button exists for this window.")]
        [SerializeField] private Button defaultBackButton;
        [SerializeField] private Settings settings;

        private UIElement[] uiElements;

        protected virtual void OnEnable()
        {
            if (defaultBackButton != null)
                defaultBackButton.onClick.AddListener(OnBackButtonClick);
        }

        protected virtual void OnDisable()
        {
            if (defaultBackButton != null)
                defaultBackButton.onClick.RemoveListener(OnBackButtonClick);
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

        protected virtual void OnBackButtonClick()
        {
            UIManager.GoBack();
        }

        public virtual Settings GetSettings()
        {
            return settings;
        }

        [System.Serializable]
        public struct Settings
        {
            [Tooltip("Should this window persist between different scenes in order to be availble any time during game lifecycle?")]
            public bool DontDestroyOnLoad;
        }
    }
}