using SBN.UITool.Core.Animation.Animators;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Utilities
{
    /// <summary>
    /// A utility script to invoke EndAnimation on the UIAnimator component.
    /// In order for this to work, an exit transition must be present on the 
    /// animationclip this script is attached to.
    /// </summary>
    public class UIAnimatorEndStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var animatorComponent = animator.GetComponent<UIAnimator>();

            if (animatorComponent == null)
            {
                Debug.LogError($"ERROR: No UI Animator component found on object: {animator.gameObject.name}", animator.gameObject);
                return;
            }

            animatorComponent.EndAnimation();
        }
    }
}