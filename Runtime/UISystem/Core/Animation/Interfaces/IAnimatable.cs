using System;

namespace SBN.UITool.Core.Animation.Interfaces
{
    public interface IAnimatable
    {
        public event Action OnAnimationDone;

        bool IsAnimating
        {
            get;
        }

        void BeginAnimation();
        void EndAnimation();
        void ResetAnimation();
    } 
}