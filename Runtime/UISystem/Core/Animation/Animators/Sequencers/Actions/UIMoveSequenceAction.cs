using DG.Tweening;
using SBN.SequencerTool.Actions;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Animators.Sequencers.Actions
{
    public class UIMoveSequenceAction : SequenceActionMono
    {
        [Header("Components")]
        [SerializeField] private RectTransform target;

        [Header("Settings")]
        [SerializeField] private Vector2 targetValue;
        [SerializeField] private Vector2 initialValue;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease easing = Ease.OutSine;

        private Tween tween;

        protected override void OnBeginAction()
        {
            tween?.Kill();

            target.anchoredPosition = initialValue;
            tween = target.DOAnchorPos(endValue: targetValue, duration: duration)
                .SetEase(easing)
                .OnComplete(EndAction);
        }

        protected override void OnEndAction()
        {
        }

        protected override void OnResetAction()
        {
            tween?.Kill();
            target.anchoredPosition = initialValue;
        }

        protected override void OnSkipAction()
        {
            tween?.Kill();
            target.anchoredPosition = targetValue;
        }
    }
}
