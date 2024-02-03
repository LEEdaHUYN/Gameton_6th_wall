using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StartButton : MonoBehaviour
{
    RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.localScale = Vector3.zero;
        ShowStartSequence();

    }

   private void ShowStartSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOScale(1f, 0.5f).SetEase(Ease.InSine));
        seq.Play().OnComplete(() =>
        {
            BoundSequence();
        });
    }
    private void BoundSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.Linear));
        seq.Append(_rectTransform.DOScale(1f, 0.5f).SetEase(Ease.Linear));
        seq.Play().SetLoops(-1);
    }
}
