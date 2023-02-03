using SBN.UITool.Core.Managers;
using SBN.Utilities.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField, ReadOnly] private string ownerSceneName;

        private UIElement[] uiElements;

        public Scene OwnerScene
        {
            get;
            private set;
        }

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

        public void Setup(UIManager uiManager, Scene ownerScene)
        {
            base.Setup(uiManager);

            OwnerScene = ownerScene;
            ownerSceneName = OwnerScene.name;

            uiElements = GetComponentsInChildren<UIElement>();

            // Start at index 1 because 0 is this UIWindow object
            // since GetComponentsInChildren also gets on the object itself
            for (int i = 1; i < uiElements.Length; i++)
                uiElements[i].Setup(uiManager);
        }

        public void SetFocus(bool value)
        {
            SetInteractableState(value);
            SetRaycastBlocking(value);
        }

        protected virtual void OnBackButtonClick()
        {
            UIManager.GoBack();
        }
    }
}