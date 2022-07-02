using System;

namespace SBN.SequencerTool.Interfaces
{
    /// <summary>
    /// The interface for a sequencer which should be used
    /// to go through all the ISequenceActions it is given.
    /// </summary>
    public interface ISequencer
    {
        /// <summary>
        /// Event to be notified when a sequence begins
        /// </summary>
        event Action OnSequenceBegin;

        /// <summary>
        /// Event to be notified when a sequence ends
        /// </summary>
        event Action OnSequenceEnd;

        /// <summary>
        /// Event to be notified when a sequence is reset
        /// </summary>
        event Action OnSequenceReset;

        /// <summary>
        /// Starts the sequence with the given sequence actions
        /// </summary>
        /// <param name="actions"></param>
        void StartSequence(params ISequenceAction[] actions);

        /// <summary>
        /// Skips the whole sequence. This means all sequence actions that are running
        /// or still needs to be run, will be skipped and have their state set to
        /// end state of whatever the action is doing/going to do.
        /// </summary>
        void SkipSequence();

        /// <summary>
        /// Resets the whole sequence. This means all sequence actions will have
        /// their state reset back to the original state before the sequennce began.
        /// </summary>
        void ResetSequence();

        /// <summary>
        /// Check to see if any actions are currently running.
        /// </summary>
        /// <returns></returns>
        bool IsRunning();
    }
}