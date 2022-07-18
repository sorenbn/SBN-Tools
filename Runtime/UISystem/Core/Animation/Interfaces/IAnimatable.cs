using System;

namespace SBN.UITool.Core.Animation.Interfaces
{
    // TODO: Figure out show/hide animation. Currently only plays animation on hide
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