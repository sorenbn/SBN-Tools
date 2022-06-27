using SBN.SequencerTool.Interfaces;
using System;
using System.Linq;

namespace SBN.SequencerTool.Sequencers
{
    public class SequencerParallel : ISequencer
    {
        public event Action OnSequenceBegin;
        public event Action OnSequenceEnd;
        public event Action OnSequenceReset;

        private ISequenceAction[] actions;

        private int currentlyFinishedActions = 0;

        public void StartSequence(params ISequenceAction[] actions)
        {
            if (actions.Length == 0)
                return;

            this.actions = actions;

            currentlyFinishedActions = 0;
            OnSequenceBegin?.Invoke();

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].OnActionEnd += SequencerParallel_OnActionEnd;
                actions[i].BeginAction();
            }
        }

        public void SkipSequence()
        {
            if (actions == null || actions.Length == 0)
                return;

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].OnActionEnd -= SequencerParallel_OnActionEnd;
                actions[i].SkipAction();
            }

            currentlyFinishedActions = 0;
            OnSequenceEnd?.Invoke();
        }

        public void ResetSequence()
        {
            if (actions == null || actions.Length == 0)
                return;

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].OnActionEnd -= SequencerParallel_OnActionEnd;
                actions[i].ResetAction();
            }

            currentlyFinishedActions = 0;
            OnSequenceReset?.Invoke();
        }

        public bool IsRunning()
        {
            return actions != null && actions.Any(x => x.Active);
        }

        private void SequencerParallel_OnActionEnd(ISequenceAction action)
        {
            action.OnActionEnd -= SequencerParallel_OnActionEnd;
            currentlyFinishedActions++;

            if (currentlyFinishedActions >= actions.Length)
            {
                OnSequenceEnd?.Invoke();
            }
        }
    }
}