using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreCoin : MonoBehaviour
{
    [SerializeField]
    private Button[] coin;
    [SerializeField]
    private Image[] Panel;
    //getdia, getpackage, fail

    [SerializeField] private IAPManager IAPManager;

    /// <summary>
    /// ����� ���̾� ����
    /// </summary>
    public void OnClickBuyAdDia()
    {

        Debug.Log(Managers.Back.GetCurrencyData("AD"));
        if (Managers.Back.GetCurrencyData("AD") == 0)
        {
            Managers.Ad.RunRewardedAd(() =>
                {
                    Managers.Back.AddCurrency(50,Define.Diamond, () =>
                    {
                        // TODO ���� ���� �޼��� 
                        Panel[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 50 + "��\n ���ſϷ�";
                        ShowImage(Panel[0]);
                    });
                }
                ,Admob.diaId, 
                () =>
                {
                    // TODO ���� �̹� ��û���� ��,
                    Panel[2].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "�̹�\n���ŵǾ����ϴ�";
                    ShowImage(Panel[2]);
                    ErrorMessagePopUp();
                });
        }
        else
        {
            Debug.Log(Managers.Back.GetItem("addia50"));
            if (Managers.Back.GetItem("addia50") != null)
            {
                ErrorMessagePopUp();
            }
            else
            {
                Managers.Back.PurchaseItem("addia50",0,Define.Coin, () =>
                {
                    Managers.Back.AddCurrency(50,Define.Diamond, () =>
                    {
                        Panel[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 50 + "��\n ȹ��Ϸ�";
                        ShowImage(Panel[0]);
                    });
                }, () =>
                {
                    
                });
            }
            
        }
    }

    private void ErrorMessagePopUp()
    {
        ShowImage(Panel[2]); 
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
    void ShowImage(Image canvas)
    {
        canvas.gameObject.transform.localScale = Vector3.one * 0.2f;
        canvas.gameObject.SetActive(true);

        var seq = DOTween.Sequence();
        seq.Append(canvas.transform.DOScale(1.1f, 0.3f));
        seq.Append(canvas.transform.DOScale(1f, 0.2f));

        seq.Play();
    }
}
