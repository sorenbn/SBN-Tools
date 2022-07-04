using SBN.SequencerTool.Actions;
using UnityEngine;
using DG.Tweening;

namespace SBN.UITool.Core.Animation.Sequencers.Actions
{
    public class UICanvasGroupSequenceAction : SequenceActionMono
    {
        [SerializeField] private CanvasGroup target;

        protected override void OnBeginAction()
        {
            target.DOKill();

            target.alpha = 0.0f;
            target.transform.localScale = Vector3.one * 0.9f;

            target.transform.DOScale(endValue: Vector3.one, duration: 0.3f)
                .SetEase(Ease.OutBack);

            target.DOFade(endValue: 1.0f, duration: 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(EndAction);
        }

        protected override void OnEndAction()
        {
        }

        protected override void OnResetAction()
        {
        }

        protected override void OnSkipAction()
        {
        }
    }
}