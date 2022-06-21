using SBN.SequencerTool.Core;
using UnityEngine;

public class SequenceTester : MonoBehaviour 
{
    [SerializeField] private Sequencer sequencer;
    [Space(10.0f)]
    [SerializeField] private SequencerParallel sequencerParallel;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            sequencer.StartSequence();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            sequencerParallel.StartSequence();
        }
    }
}