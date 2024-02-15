using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewItem : MonoBehaviour
{
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
    private float startTouchX;              // ��ġ ���� ��ġ
    private float endTouchX;                    // ��ġ ���� ��ġ
    private bool isSwipeMode = false;       // ���� Swipe�� �ǰ� �ִ��� üũ
    private float circleContentScale = 1.6f;    // ���� �������� �� ũ��(����)

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
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollBar.value = scrollPageValues[index];
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

        ItemScript.ShowInformation(currentPage);
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
