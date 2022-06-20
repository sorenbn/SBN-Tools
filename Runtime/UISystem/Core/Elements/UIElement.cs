using SBN.UITool.Core.Managers;
using System;
using UnityEngine;

namespace SBN.UITool.Core.Elements
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIElement : MonoBehaviour
    {
        public event Action<bool> OnActiveStatusChanged;

        public bool Active
        {
            get; private set;
        }

        protected CanvasGroup CanvasGroup;
        protected UIManager UIManager;

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Setup(UIManager uiManager)
        {
            UIManager = uiManager;
        }

        public virtual void Show()
        {
            // TODO: tmp, until animations are in place.

            gameObject.SetActive(true);
            Active = true;

            OnActiveStatusChanged?.Invoke(Active);
        }

        public virtual void Hide()
        {
            // TODO: tmp, until animations are in place.

            Active = false;
            gameObject.SetActive(false);

            OnActiveStatusChanged?.Invoke(Active);
        }

        public virtual void ShowInstant()
        {
            gameObject.SetActive(true);
            Active = true;

            OnActiveStatusChanged?.Invoke(Active);
        }

        public virtual void HideInstant()
        {
            Active = false;
            gameObject.SetActive(false);

            OnActiveStatusChanged?.Invoke(Active);
        }

        public virtual void SetInteractable(bool value)
        {
            CanvasGroup.interactable = value;
        }
    }
}