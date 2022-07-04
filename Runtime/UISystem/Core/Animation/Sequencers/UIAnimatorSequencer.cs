using SBN.SequencerTool.Actions;
using SBN.SequencerTool.Sequencers;
using SBN.UITool.Core.Animation.Interfaces;
using System;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Sequencers
{
    public class UIAnimatorSequencer : MonoBehaviour, IAnimatable
    {
        public event Action OnAnimationDone;

        [SerializeField] private SequenceActionMono[] actions;

        private Sequencer sequencer = new Sequencer();

        public bool IsAnimating => sequencer.IsRunning();

        public void BeginAnimation()
        {
            if (actions == null || actions.Length == 0)
                EndAnimation();

            sequencer.OnSequenceEnd += Sequencer_OnSequenceEnd;
            sequencer.StartSequence(actions);
        }

        private void Sequencer_OnSequenceEnd()
        {
            sequencer.OnSequenceEnd -= Sequencer_OnSequenceEnd;
            EndAnimation();
        }

        public void EndAnimation()
        {
            sequencer.SkipSequence();
            OnAnimationDone?.Invoke();
        }

        public void ResetAnimation()
        {
            sequencer.ResetSequence();
        }
    }

    // TODO: Make a UIAnimatorSequencerParallel.cs which uses a parellel sequencer
    // TODO: Make a UIAnimator.cs which uses an animator controller
}