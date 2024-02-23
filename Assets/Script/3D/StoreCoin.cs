using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCoin : MonoBehaviour
{
    [SerializeField]
    private Button[] coin;


    /// <summary>
    /// 광고로 다이아 구매
    /// </summary>
    public void OnClickBuyAdDia()
    {
        
        Managers.Ad.RunRewardedAd(() =>
            {
                Managers.Back.AddCurrency(50,Define.Diamond, () =>
                {
                    // TODO 구매 성공 메세지 
                });
            }
            ,Admob.diaId, 
            () =>
            {
                // TODO 광고 이미 시청했을 때,
            });
    }
    
    
    public void ShowStartSequence(int a)
    {
        if (a >= 6f)
        {
            var seq = DOTween.Sequence();
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(0.425f, 0.1f).SetEase(Ease.InSine));
            seq.Play().OnComplete(() =>
            {
                yes(a);
            });
        }
        else
        {
            var seq = DOTween.Sequence();
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2f, 0.1f).SetEase(Ease.InSine));
            seq.Play().OnComplete(() =>
            {
                yes(a);
            });
        }
    }

    void yes(int a)
    {
        if (a >= 6f)
        {
            var seq = DOTween.Sequence();
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(0.5f, 0.1f).SetEase(Ease.Linear));
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(0.425f, 0.1f).SetEase(Ease.Linear));
            //seq.Play().SetLoops(-1);
        }
        else
        {
            var seq = DOTween.Sequence();
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2.3f, 0.1f).SetEase(Ease.Linear));
            seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2f, 0.1f).SetEase(Ease.Linear));
            //seq.Play().SetLoops(-1);
        }
    }
}
