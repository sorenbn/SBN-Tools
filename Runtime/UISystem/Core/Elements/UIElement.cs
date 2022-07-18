using SBN.UITool.Core.Animation.Interfaces;
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

        private IAnimatable uiAnimation;

        private void Awake()
        {
            uiAnimation = GetComponent<IAnimatable>();
        }

        public virtual void Setup(UIManager uiManager)
        {
            UIManager = uiManager;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);

            if (uiAnimation != null)
            {
                SetInteractableState(false);

                uiAnimation.OnAnimationDone += UiAnimation_OnShowAnimationDone;
                uiAnimation.BeginAnimation();
            }
            else
            {
                SetActiveState(true);
                SetInteractableState(true);
            }
        }

        public virtual void Hide()
        {
            // TODO: tmp, until animations are in place.

            SetInteractableState(false);
            SetActiveState(false);
            gameObject.SetActive(false);
        }

        public virtual void ShowInstant()
        {
            gameObject.SetActive(true);
            SetActiveState(true);
            SetInteractableState(true);
        }

        public virtual void HideInstant()
        {
            SetInteractableState(false);
            SetActiveState(false);
            gameObject.SetActive(false);
        }

        public virtual void SetInteractableState(bool value)
        {
            if (CanvasGroup == null)
                CanvasGroup = GetComponent<CanvasGroup>();

            CanvasGroup.interactable = value;
        }

        private void SetActiveState(bool value)
        {
            Active = value;
            OnActiveStatusChanged?.Invoke(Active);
        }

        private void UiAnimation_OnShowAnimationDone()
        {
            uiAnimation.OnAnimationDone -= UiAnimation_OnShowAnimationDone;
            SetActiveState(true);
            SetInteractableState(true);
        }
    }
}