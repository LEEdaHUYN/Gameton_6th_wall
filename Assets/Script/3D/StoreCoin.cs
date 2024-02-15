using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCoin : MonoBehaviour
{
    [SerializeField]
    private Button[] coin;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void ShowStartSequence(int a)
    {
        var seq = DOTween.Sequence();
        seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2f, 0.1f).SetEase(Ease.InSine));
        seq.Play().OnComplete(() =>
        {
            yes(a);
        });
    }

    void yes(int a)
    {
        var seq = DOTween.Sequence();
        seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2.3f, 0.1f).SetEase(Ease.Linear));
        seq.Append(coin[a].GetComponent<RectTransform>().DOScale(2f, 0.1f).SetEase(Ease.Linear));
        //seq.Play().SetLoops(-1);
    }
}
