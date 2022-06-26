using SBN.SequencerTool.Interfaces;
using System;
using UnityEngine;

namespace SBN.SequencerTool.Sequencers
{
    public class SequencerParallel : ISequencer
    {
        public event Action OnSequenceBegin;
        public event Action OnSequenceEnd;

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

        private void SequencerParallel_OnActionEnd(ISequenceAction action)
        {
            action.OnActionEnd -= SequencerParallel_OnActionEnd;
            currentlyFinishedActions++;

            if (currentlyFinishedActions >= actions.Length)
            {
                OnSequenceEnd?.Invoke();
            }
        }

        public void SkipSequence()
        {
            throw new NotImplementedException();
        }

        public void CancelSequence()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning()
        {
            throw new NotImplementedException();
        }
    }
}