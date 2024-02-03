using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookContents : MonoBehaviour
{
    [TextArea(10, 20)][SerializeField] private string content;
    [Space][SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space][SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;
    [SerializeField] private Image foodcliker;
    [SerializeField] private Image eventclicker;
    private void OnValidate()
    {
        UpdatePagination();

        if (leftSide.text == content) // 확인
            return;

        SetupContent();
    }

    private void Awake()
    {
        foodcliker.gameObject.SetActive(false);
        eventclicker.gameObject.SetActive(false);
        SetupContent();
        UpdatePagination();
    }

    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }

    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
    }

    public void PreviousPage()
    {
        if (leftSide.pageToDisplay < 1) // 0페이지는 1페이지로 표시
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1)// -2했을때 1보다크면 1이 아닌 홀수 페이지므로 -2하고 표시
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1; //아니면 페이지가1이므로 1로 표시

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //오른쪽은 그보다 +1된 페이지로 표시

        UpdatePagination(); //페이지표시업데이트
    }

    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount) // 이미 마지막페이지라면
        {
            leftSide.gameObject.SetActive(false);
            rightSide.gameObject.SetActive(false);
            foodcliker.gameObject.SetActive(true);
            eventclicker.gameObject.SetActive(true);
        }

        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1) //왼쪽이 마지막페이지-1보다 크거나 같으면
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;//왼쪽은 마지막페이지-1로 설정
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //오른쪽은 왼쪽보다+1된 값으로 설정
        }
        else //둘다 아니라면(끝부분이 아닌 중간 정도쯤이라면)
        {
            leftSide.pageToDisplay += 2;//걍 2더함
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;//오른쪽은 (왼쪽페이지+1)함
        }

        UpdatePagination(); //페이지표시업데이트
    }
}
