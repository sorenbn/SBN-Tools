using SBN.SequencerTool.Actions;
using System.Collections;
using UnityEngine;

public class SequenceActionMover : SequenceActionMono
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 direction;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDuration = 2.0f;

    private Vector3 initialTargetPosition;

    private Coroutine coroutine;

    protected override void OnBeginAction()
    {
        initialTargetPosition = target.position;
        coroutine = StartCoroutine(c_Move());
    }

    protected override void OnEndAction()
    {
        coroutine = null;
    }

    protected override void OnResetAction()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        target.position = initialTargetPosition;
    }

    protected override void OnSkipAction()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        var endPosition = initialTargetPosition + (direction * moveSpeed * moveDuration);
        target.position = endPosition;

        EndAction();
    }

    private IEnumerator c_Move()
    {
        float timer = 0.0f;

        while (timer < moveDuration)
        {
            target.position += direction * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;

            yield return null;
        }

        EndAction();
    }
}