using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Xml.Serialization;

public class ScrollviewItem : MonoBehaviour
{
    public Sprite[] _Itemiamge;

    public Image line;
    public Image yellow;

    public Image Itmeimage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI undertitle;
    public TextMeshProUGUI topright;
    public TextMeshProUGUI bottonright;
    public TextMeshProUGUI bottonleft;



    public string[] itmename;
    public string[] _undertitle;
    public string[] _topright;
    public string[] _bottonright;
    public string[] _bottonleft;


    [SerializeField]
    private Scrollbar scrollBar;                    // Scrollbar의 위치를 바탕으로 현재 페이지 검사
    private float swipeTime = 0.005f;         // 페이지가 Swipe 되는 시간
    [SerializeField]
    private float swipeDistance = 0.1f;        // 페이지가 Swipe되기 위해 움직여야 하는 최소 거리
    [SerializeField]
    private Image InfoSpon;

    private ItemDictionaryAnimation ItemScript;
    private float[] scrollPageValues;           // 각 페이지의 위치 값 [0.0 - 1.0]
    private float valueDistance = 0;            // 각 페이지 사이의 거리
    private int currentPage = 0;            // 현재 페이지
    private int maxPage = 0;                // 최대 페이지
    private bool isSwipeMode = false;       // 현재 Swipe가 되고 있는지 체크

    private void Awake()
    {
        // 스크롤 되는 페이지의 각 value 값을 저장하는 배열 메모리 할당
        scrollPageValues = new float[transform.childCount];
        ItemScript = InfoSpon.GetComponent<ItemDictionaryAnimation>();
        // 스크롤 되는 페이지 사이의 거리
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // 최대 페이지의 수
        maxPage = transform.childCount;
    }

    private void Start()
    {
        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
        SetScrollBarValue(0);

        //currentpage가 바뀌면 작동
        this.ObserveEveryValueChanged(_ => currentPage)
            .Subscribe(_ =>
            {
                Itmeimage.sprite = _Itemiamge[currentPage];
                title.text = itmename[currentPage];
                undertitle.text = _undertitle[currentPage];
                topright.text = _topright[currentPage];
                bottonright.text = _bottonright[currentPage];
                bottonleft.text = _bottonleft[currentPage];
                TMPDOText(title, 0.5f); TMPDOText(undertitle, 0.8f); TMPDOText(topright, 0.8f);
                TMPDOText(bottonright, 0.8f); TMPDOText(bottonleft, 0.8f);
                ShowImage(yellow);

                line.transform.localScale = Vector3.one * 0.2f;
                var seq = DOTween.Sequence();
                seq.Append(line.transform.DORotate(new Vector3(0, 0, 100f), 1f));  
                seq.Join(line.transform.DOScale(1.2f, 0.5f));
                seq.Join(line.transform.DOScale(1f, 0.5f));
                seq.Append(line.transform.DORotate(new Vector3(0, 0, 0f), 0.7f));
             

            });
    }

    public static void TMPDOText(TextMeshProUGUI text,float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }


    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
    }

    void ShowImage(Image canvas)
    {
        canvas.gameObject.transform.localScale = Vector3.one * 0.2f;
        //canvas.gameObject.SetActive(true);

        var seq = DOTween.Sequence();
        seq.Append(canvas.transform.DOScale(1.3f, 0.5f));
        seq.Append(canvas.transform.DOScale(1f, 0.3f));

        seq.Play();
    }

    private void Update()
    {
        
    }
    
    public void rightBtn()
    {
        // 현재 페이지가 오른쪽 끝이면 종료
        if (currentPage == maxPage - 1) return;
        // 오른쪽으로 이동을 위해 현재 페이지를 1 증가
        currentPage++;
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    public void LeftBtn()
    {
        if (currentPage == 0) return;
        // 왼쪽으로 이동을 위해 현재 페이지를 1 감소
        currentPage--;
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// 페이지를 한 장 옆으로 넘기는 Swipe 효과 재생
    private IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollBar.value;
        float current = 0;
        float percent = 0;

        isSwipeMode = true;

        //ItemScript.ShowInformation(currentPage);
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        isSwipeMode = false;
    }

//    private void UpdateInput()
//    {

//#if UNITY_ANDROID


//        if (Input.touchCount == 1)
//        {
//            Touch touch = Input.GetTouch(0);

//            if (touch.phase == TouchPhase.Began)
//            {

//                startTouchX = touch.position.x;
//            }

//            else if (touch.phase == TouchPhase.Ended)
//            {
//                // 터치 종료 지점 (Swipe 방향 구분)
//                endTouchX = touch.position.x;

//                UpdateSwipe();
//            }
//        }
//#endif
//    }

}
