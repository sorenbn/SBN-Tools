using SBN.Utilities.Attributes;
using System;
using UnityEngine;

namespace SBN.SequencerTool.Core
{
    public interface ISequencer
    {
        void StartSequence();
        void SkipSequence();
        void CancelSequence();

        bool IsRunning();
    }

    [Serializable]
    public class Sequencer : ISequencer
    {
        [SerializeField] private SequenceAction[] actions;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentActionIndex = 0;
        [SerializeField, ReadOnly] private SequenceAction currentAction;

        public void StartSequence()
        {
            currentActionIndex = 0;
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
            throw new NotImplementedException();
        }

        private void ExecuteAction(SequenceAction action)
        {
            currentAction = action;
            currentAction.OnActionEnd += CurrentAction_OnActionEnd;

            currentAction.BeginAction();
        }

        private void CurrentAction_OnActionEnd(SequenceAction action)
        {
            action.OnActionEnd -= CurrentAction_OnActionEnd;

            if (currentActionIndex < actions.Length - 1)
            {
                currentActionIndex++;
                ExecuteAction(actions[currentActionIndex]);
            }
            else
            {
                Debug.Log($"End of sequence");
            }
        }
    }

    [Serializable]
    public class SequencerParallel : ISequencer
    {
        [SerializeField] private SequenceAction[] actions;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentlyFinishedActions = 0;

        public void StartSequence()
        {
            currentlyFinishedActions = 0;

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].OnActionEnd += SequencerParallel_OnActionEnd;
                actions[i].BeginAction();
            }
        }

        private void SequencerParallel_OnActionEnd(SequenceAction action)
        {
            action.OnActionEnd -= SequencerParallel_OnActionEnd;
            currentlyFinishedActions++;

            if (currentlyFinishedActions >= actions.Length)
            {
                Debug.Log($"End of sequence");
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