using SBN.SequencerTool.Core;
using System;
using System.Collections;
using UnityEngine;

public class SequenceActionWaiter : SequenceAction
{
    [SerializeField] private float waitTime = 1.0f;

    protected override void OnBeginAction()
    {
        Debug.Log($"Waiting {waitTime} second(s)..");
        StartCoroutine(c_Wait(waitTime, () => EndAction()));
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

    private IEnumerator c_Wait(float timeInSeconds, Action callback)
    {
        yield return new WaitForSeconds(timeInSeconds);
        callback?.Invoke();
    }
}