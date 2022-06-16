using UnityEngine;

namespace SBN.UITool.Core.Elements
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIElement : MonoBehaviour
    {
        public bool Active
        {
            get; private set;
        }

        protected CanvasGroup canvasGroup;
        protected UIWindowManager uiManager;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Setup(UIWindowManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public virtual void Show()
        {
            // TODO: tmp, until animations are in place.

            gameObject.SetActive(true);
            Active = true;
        }

        public virtual void Hide()
        {
            // TODO: tmp, until animations are in place.

            Active = false;
            gameObject.SetActive(false);
        }

        public virtual void ShowInstant()
        {
            gameObject.SetActive(true);
            Active = true;
        }

        public virtual void HideInstant()
        {
            Active = false;
            gameObject.SetActive(false);
        }
    }
}