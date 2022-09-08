using SBN.UITool.Core.Elements;
using System;
using UnityEngine;

namespace SBN.UITool.Core.Managers
{
    [RequireComponent(typeof(UIManager))]
    public class UIModalManager : MonoBehaviour
    {
        [SerializeField] private UIModal modalPrefab;

        private UIModal modalInstance;
        private UIManager uiManager;

        private Action<bool> modalCallback;

        private void Start()
        {
            SetupModel();
        }

        private void SetupModel()
        {
            if (modalPrefab == null)
            {
                Debug.LogWarning($"WARNING: No modal prefab found! Did you forget to assign it in the inspector of: {gameObject.name}?", gameObject);
                return;
            }

            uiManager = GetComponent<UIManager>();

            if (uiManager == null)
            {
                Debug.LogError($"ERROR: No UIManager found! It is required to be one the same object as the UI Modal Manager", gameObject);
                return;
            }

            modalInstance = Instantiate(modalPrefab, transform, false);
            modalInstance.Setup(uiManager);
            modalInstance.HideInstant();
        }

        public void ShowModal(Action<bool> callback = null)
        {
            modalCallback = callback;

            uiManager.CurrentWindowInstance?.SetInteractableState(false);
            modalInstance.OnInteracted += Modal_OnInteracted;

            modalInstance.Show();
        }

        private void Modal_OnInteracted(bool confirm)
        {
            modalInstance.OnInteracted -= Modal_OnInteracted;
            uiManager.CurrentWindowInstance?.SetInteractableState(true);

            modalCallback?.Invoke(confirm);
        }
    }
}