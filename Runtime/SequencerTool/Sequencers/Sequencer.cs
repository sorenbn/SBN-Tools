using SBN.SequencerTool.Interfaces;
using System;

namespace SBN.SequencerTool.Sequencers
{
    /// <summary>
    /// A standard sequencer.
    /// Runs every sequence action sequentiallly in the order that they came in through the StartSequence() method.
    /// </summary>
    public class Sequencer : ISequencer
    {
        public event Action OnSequenceBegin;
        public event Action OnSequenceEnd;
        public event Action OnSequenceReset;

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
            if (!IsRunning())
                return;

            if (currentAction != null)
                currentAction.OnActionEnd -= CurrentAction_OnActionEnd;

            for (int i = currentActionIndex; i < actions.Length; i++)
                actions[i].SkipAction();

            currentAction = null;
            currentActionIndex = 0;

            OnSequenceEnd?.Invoke();
        }

        public void ResetSequence()
        {
            if (!IsRunning())
                return;

            if (currentAction != null)
                currentAction.OnActionEnd -= CurrentAction_OnActionEnd;

            for (int i = 0; i < actions.Length; i++)
                actions[i].ResetAction();

            currentAction = null;
            currentActionIndex = 0;

            OnSequenceReset?.Invoke();
        }

        public bool IsRunning()
        {
            if (actions == null || actions.Length == 0)
                return false;

            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Active)
                    return true;
            }

            return false;
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