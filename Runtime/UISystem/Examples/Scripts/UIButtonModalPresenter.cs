using SBN.UITool.Core.Elements;
using UnityEngine;

public class UIButtonModalPresenter : UIElement
{
    private UIModal modal;

    public void ShowModal()
    {
        modal = UIManager.ModalManager?.ShowModal();

        if (modal != null)
        {
            modal.OnConfirmClicked += Modal_OnConfirmClicked;
            modal.OnCancelClicked += Modal_OnCancelClicked;
        }
    }

    private void Modal_OnCancelClicked()
    {
        Debug.Log($"Cancel");

        modal.OnConfirmClicked -= Modal_OnConfirmClicked;
        modal.OnCancelClicked -= Modal_OnCancelClicked;
    }

    private void Modal_OnConfirmClicked()
    {
        Debug.Log($"Confirm");

        modal.OnConfirmClicked -= Modal_OnConfirmClicked;
        modal.OnCancelClicked -= Modal_OnCancelClicked;
    }
}