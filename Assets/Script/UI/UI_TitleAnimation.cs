using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class UI_TitleAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    //public Ease ease; // 인스펙터에서 설정
    //transform.DOMoveY(targetX, 3).SetEase(ease);
    //Jira Push Test
    void Start()
    {
        var rectTransform = this.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(0, 800);
        rectTransform.DOAnchorPosY(102f, 1f).SetEase(Ease.OutBounce);
    }

}
