using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBtnAnimation : MonoBehaviour
{
    [SerializeField]
    private Image[] PlayLabel;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayLabel.Length; i++)
        {
            PlayLabel[i].rectTransform.DOScaleX(0.1f, 0.3f);
        }
    }

    public void PlayLabelShow()
    {
        for (int i = 0; i < PlayLabel.Length; i++)
        {
            PlayLabel[i].rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBounce);
        }

    }

    public void PlayLabelHide()
    {
        for (int i = 0; i < PlayLabel.Length; i++)
        {
            PlayLabel[i].rectTransform.DOScaleX(0.1f, 0.2f).SetEase(Ease.OutBounce);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
