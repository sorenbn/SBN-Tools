using SBN.SequencerTool.Core;
using UnityEngine;

public class SequenceActionLogger : SequenceAction
{
    [SerializeField] private string message;

    protected override void OnBeginAction()
    {
        Debug.Log(message);
        EndAction();
    }

    protected override void OnCancelAction()
    {

    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }
}