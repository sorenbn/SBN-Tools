using System;

namespace SBN.SequencerTool.Interfaces
{
    /// <summary>
    /// An interface defining the requirements for a sequence action.
    /// All sequence actions must implement this interface in order to
    /// work with the ISequencer.
    /// </summary>
    public interface ISequenceAction
    {
        /// <summary>
        /// Determines if this action is actively running.
        /// </summary>
        bool Active
        {
            get;
            set;
        }

        /// <summary>
        /// Invoked whenever an action is beginning
        /// to be notified
        /// </summary>
        event Action<ISequenceAction> OnActionBegin;

        /// <summary>
        /// Invoked whenever an action has ended
        /// to be notified
        /// </summary>
        event Action<ISequenceAction> OnActionEnd;

        /// <summary>
        /// Invoked whenever an action is beginning
        /// </summary>
        void BeginAction();

        /// <summary>
        /// Invoked whenever an action is ending
        /// </summary>
        void EndAction();

        /// <summary>
        /// Invoked whenever an action or sequence is reset.
        /// Should be used to reset the state back to its initial state.
        /// </summary>
        void ResetAction();

        /// <summary>
        /// Invoked whenever an action or sequence is skipped.
        /// Should be used to set the "end state" of the action instantly.
        /// </summary>
        void SkipAction();
    }
}