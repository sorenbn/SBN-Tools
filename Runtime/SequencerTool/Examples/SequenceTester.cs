using SBN.SequencerTool;
using SBN.SequencerTool.Interfaces;
using SBN.SequencerTool.Sequencers;
using UnityEngine;

public class SequenceTester : MonoBehaviour
{
    [SerializeField] private SequenceActionMono[] actions;

    private ISequencer sequencer;
    private ISequencer sequencerParallel;

    private void OnEnable()
    {
        if (sequencer == null)
            sequencer = new Sequencer();

        if (sequencerParallel == null)
            sequencerParallel = new SequencerParallel();

        sequencer.OnSequenceEnd += Sequencer_OnSequenceEnd;
        sequencer.OnSequenceReset += Sequencer_OnSequenceReset;

        sequencerParallel.OnSequenceEnd += SequencerParallel_OnSequenceEnd;
        sequencerParallel.OnSequenceReset += SequencerParallel_OnSequenceReset;
    }

    private void OnDisable()
    {
        sequencer.OnSequenceEnd -= Sequencer_OnSequenceEnd;
        sequencer.OnSequenceReset -= Sequencer_OnSequenceReset;

        sequencerParallel.OnSequenceEnd -= SequencerParallel_OnSequenceEnd;
        sequencerParallel.OnSequenceReset -= SequencerParallel_OnSequenceReset;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            sequencer.StartSequence(actions);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            sequencerParallel.StartSequence(actions);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            sequencer.ResetSequence();
            sequencerParallel.ResetSequence();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            sequencer.SkipSequence();
            sequencerParallel.SkipSequence();
        }
    }

    private void Sequencer_OnSequenceEnd()
    {
        Debug.Log($"Sequence end");
    }

    private void SequencerParallel_OnSequenceEnd()
    {
        Debug.Log($"Sequence parallel end");
    }

    private void Sequencer_OnSequenceReset()
    {
        Debug.Log($"Sequence reset");
    }

    private void SequencerParallel_OnSequenceReset()
    {
        Debug.Log($"Sequence parallel reset");
    }
}