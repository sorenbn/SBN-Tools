using System;

namespace SBN.UITool.Core.Animation.Interfaces
{
    public interface IAnimatable
    {
        Action<IAnimatable> OnAnimationDone
        {
            get;
            set;
        }

        bool IsAnimating
        {
            get;
        }

        void BeginAnimation();
        void StopAnimation();
    } 
}