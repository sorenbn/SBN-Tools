using SBN.SequencerTool.Interfaces;
using System;
using System.Linq;

namespace SBN.SequencerTool.Sequencers
{
    public class Sequencer : ISequencer
    {
        public event Action OnSequenceBegin;
        public event Action OnSequenceEnd;

        private ISequenceAction[] actions;
        private ISequenceAction currentAction;

        private int currentActionIndex = 0;

        public void StartSequence(params ISequenceAction[] actions)
        {
            if (actions.Length == 0)
                return;

            this.actions = actions;

            currentActionIndex = 0;
            OnSequenceBegin?.Invoke();

            ExecuteAction(actions[currentActionIndex]);
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
            return actions.Any(x => x.Active);
        }

        private void ExecuteAction(ISequenceAction action)
        {
            currentAction = action;
            currentAction.OnActionEnd += CurrentAction_OnActionEnd;

            currentAction.BeginAction();
        }

        private void CurrentAction_OnActionEnd(ISequenceAction action)
        {
            action.OnActionEnd -= CurrentAction_OnActionEnd;

            if (currentActionIndex < actions.Length - 1)
            {
                currentActionIndex++;
                ExecuteAction(actions[currentActionIndex]);
            }
            else
            {
                OnSequenceEnd?.Invoke();
            }
        }
    }
}