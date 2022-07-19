using SBN.SequencerTool.Actions;
using UnityEngine;
using DG.Tweening;

namespace SBN.UITool.Core.Animation.Animators.Sequencers.Actions
{
    public class UIFadeSequenceAction : SequenceActionMono
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup target;

        [Header("Settings")]
        [SerializeField] private float targetValue = 1.0f;
        [SerializeField] private float initialValue = 0.0f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease easing = Ease.OutSine;

        private Tween tween;

        protected override void OnBeginAction()
        {
            tween?.Kill();

            target.alpha = initialValue;
            tween = target.DOFade(endValue: targetValue, duration: duration)
                .SetEase(easing)
                .OnComplete(EndAction);
        }

        protected override void OnEndAction()
        {
        }

        protected override void OnResetAction()
        {
            tween?.Kill();
            target.alpha = initialValue;
        }

        protected override void OnSkipAction()
        {
            tween?.Kill();
            target.alpha = targetValue;
        }
    }
}