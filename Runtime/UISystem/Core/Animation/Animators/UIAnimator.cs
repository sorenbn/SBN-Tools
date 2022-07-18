using SBN.UITool.Core.Animation.Interfaces;
using System;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Animators
{
    [RequireComponent(typeof(Animator))]
    public class UIAnimator : MonoBehaviour, IAnimatable
    {
        private Animator animator;

        public bool IsAnimating => throw new NotImplementedException();

        public event Action OnAnimationDone;

        public void BeginAnimation()
        {
            throw new NotImplementedException();
        }

        public void EndAnimation()
        {
            throw new NotImplementedException();
        }

        public void ResetAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
