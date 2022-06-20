using System;
using UnityEngine;
using UnityEngine.UI;

namespace SBN.UITool.Core.Elements
{
    public class UIModal : UIElement
    {
        public event Action OnConfirmClicked;
        public event Action OnCancelClicked;

        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private void OnEnable()
        {
            if (confirmButton != null)
                confirmButton.onClick.AddListener(OnConfirmedClicked);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelledClicked);
        }

        private void OnDisable()
        {
            if (confirmButton != null)
                confirmButton.onClick.RemoveListener(OnConfirmedClicked);

            if (cancelButton != null)
                cancelButton.onClick.RemoveListener(OnCancelledClicked);
        }

        protected virtual void OnConfirmedClicked()
        {
            OnConfirmClicked?.Invoke();
            Hide();
        }

        protected virtual void OnCancelledClicked()
        {
            OnCancelClicked?.Invoke();
            Hide();
        }
    }
}