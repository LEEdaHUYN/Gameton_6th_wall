using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCoin : MonoBehaviour
{
    [SerializeField]
    private Button[] coin;

    [SerializeField] private IAPManager IAPManager;

    /// <summary>
    /// ����� ���̾� ����
    /// </summary>
    public void OnClickBuyAdDia()
    {

        if (Managers.Back.GetCurrencyData("AD") == 0)
        {
            Managers.Ad.RunRewardedAd(() =>
                {
                    Managers.Back.AddCurrency(50,Define.Diamond, () =>
                    {
                        // TODO ���� ���� �޼��� 
                    });
                }
                ,Admob.diaId, 
                () =>
                {
                    // TODO ���� �̹� ��û���� ��,
                    ErrorMessagePopUp();
                });
        }
        else
        {
            if (Managers.Back.GetItem("addia50") != null)
            {
                ErrorMessagePopUp();
            }
            Managers.Back.PurchaseItem("addia50",0,Define.Coin, () =>
            {
                Managers.Back.AddCurrency(50,Define.Diamond, () =>
                {
                    // TODO ���� ���� �޼��� 
                });
            }, () =>
            {
                    
            });
        }
    }

    private void ErrorMessagePopUp()
    {
        //TODO
    }

    public void OnClickByDiaInApp(string productCode)
    {
        IAPManager.Purchase(productCode);
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
