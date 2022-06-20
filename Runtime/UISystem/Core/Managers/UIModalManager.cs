using SBN.UITool.Core.Elements;
using UnityEngine;

namespace SBN.UITool.Core.Managers
{
    [RequireComponent(typeof(UIManager))]
    public class UIModalManager : MonoBehaviour
    {
        [SerializeField] private UIModal modal;

        private UIManager uiManager;

        private void Awake()
        {
            if (modal == null)
            {
                Debug.LogError($"ERROR: No modal found! Did you forget to set it in the inspector?", gameObject);
                return;
            }

            SetupModel();
        }

        private void SetupModel()
        {
            uiManager = GetComponent<UIManager>();

            if (uiManager == null)
            {
                Debug.LogError($"ERROR: No UIManager found! This is required to be one the same object as this", gameObject);
                return;
            }

            modal.Setup(uiManager);
            modal.HideInstant();
        }

        public UIModal ShowModal()
        {
            modal.OnActiveStatusChanged += Modal_OnActiveStatusChanged;
            modal.Show();

            return modal;
        }

        private void Modal_OnActiveStatusChanged(bool active)
        {
            if (active)
            {
                uiManager.CurrentWindow?.SetInteractable(false);
            }
            else
            {
                modal.OnActiveStatusChanged -= Modal_OnActiveStatusChanged;
                uiManager.CurrentWindow?.SetInteractable(true);
            }
        }
    }
}