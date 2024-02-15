using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelAnimatin : MonoBehaviour
{
    [SerializeField]
    private Image[] StoreLabel;

    void Start()
    {
        for (int i = 0; i < StoreLabel.Length; i++)
        {
            StoreLabel[i].rectTransform.DOScaleY(0.1f, 0.3f);
        }
    }
    public void StoreLabelShow()
    {
       for (int i = 0; i < StoreLabel.Length; i++)
        {
            StoreLabel[i].rectTransform.DOScaleY(1f, 0.3f).SetEase(Ease.OutBounce);
        }

    }

    public void StoreLabelHide()
    {
        for (int i = 0; i < StoreLabel.Length; i++)
        {
            StoreLabel[i].rectTransform.DOScaleY(0.1f, 0.2f).SetEase(Ease.OutBounce);
        }

    }

}
