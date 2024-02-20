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
    private Scrollbar scrollBar;                    // Scrollbar�� ��ġ�� �������� ���� ������ �˻�
    private float swipeTime = 0.005f;         // �������� Swipe �Ǵ� �ð�
    [SerializeField]
    private float swipeDistance = 0.1f;        // �������� Swipe�Ǳ� ���� �������� �ϴ� �ּ� �Ÿ�
    [SerializeField]
    private Image InfoSpon;

    private ItemDictionaryAnimation ItemScript;
    private float[] scrollPageValues;           // �� �������� ��ġ �� [0.0 - 1.0]
    private float valueDistance = 0;            // �� ������ ������ �Ÿ�
    private int currentPage = 0;            // ���� ������
    private int maxPage = 0;                // �ִ� ������
    private bool isSwipeMode = false;       // ���� Swipe�� �ǰ� �ִ��� üũ

    private void Awake()
    {
        // ��ũ�� �Ǵ� �������� �� value ���� �����ϴ� �迭 �޸� �Ҵ�
        scrollPageValues = new float[transform.childCount];
        ItemScript = InfoSpon.GetComponent<ItemDictionaryAnimation>();
        // ��ũ�� �Ǵ� ������ ������ �Ÿ�
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // �ִ� �������� ��
        maxPage = transform.childCount;
    }

    private void Start()
    {
        // ���� ������ �� 0�� �������� �� �� �ֵ��� ����
        SetScrollBarValue(0);

        //currentpage�� �ٲ�� �۵�
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
        // ���� �������� ������ ���̸� ����
        if (currentPage == maxPage - 1) return;
        // ���������� �̵��� ���� ���� �������� 1 ����
        currentPage++;
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    public void LeftBtn()
    {
        if (currentPage == 0) return;
        // �������� �̵��� ���� ���� �������� 1 ����
        currentPage--;
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// �������� �� �� ������ �ѱ�� Swipe ȿ�� ���
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
//                // ��ġ ���� ���� (Swipe ���� ����)
//                endTouchX = touch.position.x;

//                UpdateSwipe();
//            }
//        }
//#endif
//    }

}
