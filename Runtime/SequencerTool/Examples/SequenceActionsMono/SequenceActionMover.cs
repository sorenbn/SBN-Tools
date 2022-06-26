using SBN.SequencerTool;
using System.Collections;
using UnityEngine;

public class SequenceActionMover : SequenceActionMono
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 direction;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveDuration = 2.0f;

    protected override void OnBeginAction()
    {
        StartCoroutine(c_Move());
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnCancelAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSkipAction()
    {
        throw new System.NotImplementedException();
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