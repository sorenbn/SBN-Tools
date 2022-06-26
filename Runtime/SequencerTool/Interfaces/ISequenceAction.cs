using System;

namespace SBN.SequencerTool.Interfaces
{
    public interface ISequenceAction
    {
        bool Active
        {
            get;
            set;
        }

        event Action<ISequenceAction> OnActionBegin;
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