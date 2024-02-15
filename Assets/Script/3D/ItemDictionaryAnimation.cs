using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDictionaryAnimation : MonoBehaviour
{
    public Image[] Iteminformation;
    public Image itemspon;
    void Start()
    {
        for (int i = 1; i < Iteminformation.Length; i++)
        {
            Iteminformation[i].gameObject.SetActive(false);
            Iteminformation[i].rectTransform.localScale= Vector2.zero;
        }
    }

    public void ShowInformation(int currentPage)
    {
        HideIamge(Iteminformation[currentPage - 1]);
        //Destroy(Iteminformation[currentPage-1]);
        Instantiate(Iteminformation[currentPage],itemspon.rectTransform , itemspon.rectTransform);
        ShowImage(Iteminformation[currentPage]);
    }

    void HideIamge(Image canvas)
    {
        var seq = DOTween.Sequence();

        canvas.transform.localScale = Vector3.one * 0.2f;
        seq.Append(canvas.transform.DOScale(1.1f, 0.2f));
        seq.Append(canvas.transform.DOScale(0.2f, 0.2f));
        seq.Play().OnComplete(() =>
        {
            canvas.gameObject.SetActive(false);
        });
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
