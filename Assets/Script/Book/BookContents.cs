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

        if (leftSide.text == content) // Ȯ��
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
        if (leftSide.pageToDisplay < 1) // 0�������� 1�������� ǥ��
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1)// -2������ 1����ũ�� 1�� �ƴ� Ȧ�� �������Ƿ� -2�ϰ� ǥ��
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1; //�ƴϸ� ��������1�̹Ƿ� 1�� ǥ��

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //�������� �׺��� +1�� �������� ǥ��

        UpdatePagination(); //������ǥ�þ�����Ʈ
    }

    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount) // �̹� ���������������
        {
            leftSide.gameObject.SetActive(false);
            rightSide.gameObject.SetActive(false);
            foodcliker.gameObject.SetActive(true);
            eventclicker.gameObject.SetActive(true);
        }

        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1) //������ ������������-1���� ũ�ų� ������
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;//������ ������������-1�� ����
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //�������� ���ʺ���+1�� ������ ����
        }
        else //�Ѵ� �ƴ϶��(���κ��� �ƴ� �߰� �������̶��)
        {
            leftSide.pageToDisplay += 2;//�� 2����
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;//�������� (����������+1)��
        }

        UpdatePagination(); //������ǥ�þ�����Ʈ
    }
}
