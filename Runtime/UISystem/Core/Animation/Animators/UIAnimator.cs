using SBN.UITool.Core.Animation.Interfaces;
using System;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Animators
{
    /// <summary>
    /// A component to run animations via the unity animation system.
    /// This will automatically be picked up by the UIElement on the same gameobject
    /// and played when required.
    /// 
    /// In order for the UIAniamtor to know when it is finished with its animation, 
    /// EndAnimation() must be invoked via animation events or 
    /// simply add the utility script "UIAnimatorEndStateBehaviour" to the animationclip
    /// in the animator.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class UIAnimator : MonoBehaviour, IAnimatable
    {
        public event Action OnAnimationDone;

        [SerializeField] private AnimationClip animationClip;  

        private int hashedClip;
        private Animator animator;

        public bool IsAnimating => animator.enabled;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            if (animationClip == null)
            {
                Debug.LogError($"ERROR: No animation clip has been assigned to UIAnimator", gameObject);
                return;
            }

            hashedClip = Animator.StringToHash(animationClip.name);
            animator.enabled = false;
        }

        public void BeginAnimation()
        {
            if (animationClip == null)
                return;

            animator.enabled = true;
            animator.Play(hashedClip, -1, 0.0f);
        }

        public void EndAnimation()
        {
            OnAnimationDone?.Invoke();
            animator.enabled = false;
        }

        public void ResetAnimation()
        {
            animator.enabled = false;
            animator.enabled = true;
        }

        private void OnDisable()
        {
            ResetAnimation();
        }
    }
}
