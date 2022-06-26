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
        sequencerParallel.OnSequenceEnd += SequencerParallel_OnSequenceEnd;
    }

    private void OnDisable()
    {
        sequencer.OnSequenceEnd -= Sequencer_OnSequenceEnd;
        sequencerParallel.OnSequenceEnd -= SequencerParallel_OnSequenceEnd;
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
    }

    private void Sequencer_OnSequenceEnd()
    {
        Debug.Log($"Sequence end");
    }

    private void SequencerParallel_OnSequenceEnd()
    {
        Debug.Log($"Sequence parallel end");
    }
}