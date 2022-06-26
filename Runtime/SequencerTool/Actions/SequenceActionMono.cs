using SBN.SequencerTool.Interfaces;
using System;
using UnityEngine;

namespace SBN.SequencerTool
{
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

        protected abstract void OnBeginAction();
        protected abstract void OnEndAction();
        protected abstract void OnSkipAction();
        protected abstract void OnResetAction();
    }
}