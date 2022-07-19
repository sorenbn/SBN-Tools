using DG.Tweening;
using SBN.SequencerTool.Actions;
using UnityEngine;

namespace SBN.UITool.Core.Animation.Animators.Sequencers.Actions
{
    public class UIScaleSequenceAction : SequenceActionMono
    {
        [Header("Components")]
        [SerializeField] private Transform target;  

        [Header("Settings")]
        [SerializeField] private Vector3 targetValue;
        [SerializeField] private Vector3 initialValue;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease easing = Ease.OutSine;

        private Tween tween;

        protected override void OnBeginAction()
        {
            tween?.Kill();

            target.transform.localScale = initialValue;
            tween = target.transform.DOScale(endValue: targetValue, duration: duration)
                .SetEase(easing)
                .OnComplete(EndAction);
        }

        protected override void OnEndAction()
        {
        }

        protected override void OnResetAction()
        {
            tween?.Kill();
            target.transform.localScale = initialValue;
        }

        protected override void OnSkipAction()
        {
            tween?.Kill();
            target.transform.localScale = targetValue;
        }
    }
}
