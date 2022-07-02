using SBN.SequencerTool.Interfaces;
using System;
using UnityEngine;

namespace SBN.SequencerTool
{
    /// <summary>
    /// A base class which can be derived from to create your own sequence actions.
    /// Handles all the basics of invoking events and handling active state.
    /// This is the mono version, meaning you can inject custom data from the inspector instead of constructor.
    /// </summary>
    public abstract class SequenceActionMono : MonoBehaviour, ISequenceAction
    {
        public event Action<ISequenceAction> OnActionBegin;
        public event Action<ISequenceAction> OnActionEnd;

        public bool Active
        {
            get; set;
        }

        public void BeginAction()
        {
            Active = true;
            OnBeginAction();

            OnActionBegin?.Invoke(this);
        }

        public void EndAction()
        {
            Active = false;
            OnEndAction();

            OnActionEnd?.Invoke(this);
        }

        public void SkipAction()
        {
            Active = false;
            OnSkipAction();
        }

        public void ResetAction()
        {
            Active = false;
            OnResetAction();
        }

        /// <summary>
        /// Invoked whenever an action is beginning
        /// </summary>
        protected abstract void OnBeginAction();

        /// <summary>
        /// Invoked whenever an action is ending
        /// </summary>
        protected abstract void OnEndAction();

        /// <summary>
        /// Invoked whenever an action or sequence is skipped.
        /// Should be used to set the "end state" of the action instantly.
        /// </summary>
        protected abstract void OnSkipAction();

        /// <summary>
        /// Invoked whenever an action or sequence is reset.
        /// Should be used to reset the action back to its original state.
        /// </summary>
        protected abstract void OnResetAction();
    }
}