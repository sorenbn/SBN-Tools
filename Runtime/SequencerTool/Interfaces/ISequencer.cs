using System;

namespace SBN.SequencerTool.Interfaces
{
    public interface ISequencer
    {
        event Action OnSequenceBegin;
        event Action OnSequenceEnd;
        event Action OnSequenceReset;

        void StartSequence(ISequenceAction[] actions);
        void SkipSequence();
        void ResetSequence();

        bool IsRunning();
    }
}