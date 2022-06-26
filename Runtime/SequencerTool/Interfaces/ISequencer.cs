using System;

namespace SBN.SequencerTool.Interfaces
{
    public interface ISequencer
    {
        event Action OnSequenceBegin;
        event Action OnSequenceEnd;

        void StartSequence(ISequenceAction[] actions);
        void SkipSequence();
        void CancelSequence();

        bool IsRunning();
    }
}