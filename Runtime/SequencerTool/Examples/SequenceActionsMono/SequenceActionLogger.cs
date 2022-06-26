using SBN.SequencerTool;
using UnityEngine;

public class SequenceActionLogger : SequenceActionMono
{
    [SerializeField] private string message;

    protected override void OnBeginAction()
    {
        Debug.Log(message);
        EndAction();
    }

    protected override void OnResetAction()
    {

    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }
}