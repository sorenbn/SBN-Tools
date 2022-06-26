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

        void BeginAction();
        void CancelAction();
        void EndAction();
        void SkipAction();
    }
}