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
            Iteminformation[i].gameObject.SetActive(true);
            Iteminformation[i].rectTransform.localScale= new Vector2(1f,1f);
        }
        Instantiate(Iteminformation[0], itemspon.rectTransform);
        ShowImage(Iteminformation[0]);
    }

    public void ShowInformation(int currentPage)
    {
        //HideIamge(Iteminformation[currentPage - 1]);
        //Destroy(Iteminformation[currentPage-1]);
        Iteminformation[currentPage-1].gameObject.SetActive(false);
        Instantiate(Iteminformation[currentPage],itemspon.rectTransform);
        Iteminformation[currentPage].gameObject.SetActive(true);
        //ShowImage(Iteminformation[currentPage]);
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
