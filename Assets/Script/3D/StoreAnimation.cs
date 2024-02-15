using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreAnimation : MonoBehaviour
{

    [SerializeField]
    private Image[] CharacterIamge;

    [SerializeField]
    private Image[] CharLabel;
    int currentnumber = 0;
    void Start()
    {
        for (int i = 1; i < CharacterIamge.Length; i++)
        {
            CharacterIamge[i].gameObject.SetActive(false);
            CharacterIamge[i].transform.localScale = Vector3.one * 0.1f;
        }
        for (int i = 1; i < CharacterIamge.Length; i++)
        {
            CharLabel[i].gameObject.SetActive(false);
            CharLabel[i].transform.DOScaleY(0.01f, 0.01f);
            CharLabel[i].rectTransform.DOMoveY(-48f, 0.1f);
        }
        CharLabel[0].transform.DOScaleY(0.5f, 0.1f).SetEase(Ease.OutBounce);
        CharLabel[0].rectTransform.DOAnchorPosY(-200f, 0.2f);
        CharacterIamge[0].gameObject.SetActive(true);
    }

    public void RightBtn()
    {
        var seq = DOTween.Sequence();
        CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().DOAnchorPosX(350f, 0.5f);
        seq.Append(CharacterIamge[currentnumber].gameObject.transform.DOScale(0.2f, 0.3f));

        CharLabel[currentnumber].transform.DOScaleY(0.01f, 0.1f).SetEase(Ease.OutBounce);
        seq.Join(CharLabel[currentnumber].rectTransform.DOAnchorPosY(-50f,0.1f).SetEase(Ease.OutBounce));

        seq.Play().OnComplete(() =>
        {
            CharacterIamge[currentnumber].gameObject.SetActive(false);
            CharLabel[currentnumber].gameObject.SetActive(false);

            if (currentnumber == 2)
                currentnumber = 0;
            else
                currentnumber++;

            CharacterIamge[currentnumber].gameObject.SetActive(true);
            CharLabel[currentnumber].gameObject.SetActive(true);

            CharLabel[currentnumber].transform.DOScaleY(0.5f, 0.1f).SetEase(Ease.OutBounce);
            seq.Join(CharLabel[currentnumber].rectTransform.DOAnchorPosY(-200f, 0.2f).SetEase(Ease.OutBounce));

            CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(950f, 5f);
            CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().DOAnchorPosX(650f, 0.5f);
            seq.Append(CharacterIamge[currentnumber].gameObject.transform.DOScale(0.5f, 0.1f));

        });


    }

    public void LeftBtn()
    {
        var seq = DOTween.Sequence();
        CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().DOAnchorPosX(950f, 0.5f);
        seq.Append(CharacterIamge[currentnumber].gameObject.transform.DOScale(0.2f, 0.3f));

        CharLabel[currentnumber].transform.DOScaleY(0.01f, 0.1f).SetEase(Ease.OutBounce);
        seq.Join(CharLabel[currentnumber].rectTransform.DOAnchorPosY(-50f, 0.1f).SetEase(Ease.OutBounce));

        seq.Play().OnComplete(() =>
        {
            CharacterIamge[currentnumber].gameObject.SetActive(false);
            CharLabel[currentnumber].gameObject.SetActive(false);

            if (currentnumber == 0)
                currentnumber = 2;
            else
                currentnumber--;

            CharacterIamge[currentnumber].gameObject.SetActive(true);
            CharLabel[currentnumber].gameObject.SetActive(true);

            CharLabel[currentnumber].transform.DOScaleY(0.5f, 0.1f).SetEase(Ease.OutBounce);
            seq.Join(CharLabel[currentnumber].rectTransform.DOAnchorPosY(-200f, 0.2f).SetEase(Ease.OutBounce));

            CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(350f, 5f);
            CharacterIamge[currentnumber].gameObject.GetComponent<RectTransform>().DOAnchorPosX(650f, 0.5f);
            seq.Append(CharacterIamge[currentnumber].gameObject.transform.DOScale(0.5f, 0.1f));

        });
    }
}

