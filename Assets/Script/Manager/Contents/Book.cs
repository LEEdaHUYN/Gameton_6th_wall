
using System;
using System.Collections.Generic;
using System.Linq;
using Script.TriggerSystem;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Book : MonoBehaviour
{
    [TextArea(10, 20)][SerializeField] private string content;
    [Space][SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space][SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;
    
    private Dictionary<string, Selector> _selectors = new Dictionary<string, Selector>();
    private int _maxPageCount;

    private YesOrNoSelector _yesOrNoSelector;
    private ItemChoiceSelector _itemChoiceSelector;
    private void Awake()
    {
        Managers.Resource.Load<GameObject>("FoodBox", (success) =>
        {
            var selectorGameObject = Object.Instantiate(success, this.transform.parent).GetComponent<FoodSelector>();
            _selectors.Add("FoodBox",selectorGameObject);
        });
        Managers.Resource.Load<GameObject>("YesOrNoBox", (success) =>
        {
            var selectorGameObject = Object.Instantiate(success, this.transform.parent).GetComponent<YesOrNoSelector>();
            _yesOrNoSelector = selectorGameObject;
            _yesOrNoSelector.gameObject.SetActive(false);
        });
        Managers.Resource.Load<GameObject>("ItemChoiceBox", (success) =>
        {
            var selectorGameObject = Object.Instantiate(success, this.transform.parent).GetComponent<ItemChoiceSelector>();
            _itemChoiceSelector = selectorGameObject;
            _itemChoiceSelector.gameObject.SetActive(false);
        });
       
    }

    public void AddYesOrNoBox(string text,Flag yesFlag, Flag noFlag)
    { 
        _yesOrNoSelector.gameObject.SetActive(true);
      _selectors.Add("YesOrNoBox",_yesOrNoSelector);
      _yesOrNoSelector.Init(text,yesFlag,noFlag,NextPage);
    }
    
    public void AddItemChoiceBox(string text, List<ItemFlag> itemFlagList)
    {
        //TODO
        _itemChoiceSelector.gameObject.SetActive(true);
        _selectors.Add("ItemChoiceBox",_itemChoiceSelector);
        _itemChoiceSelector.Init(text, itemFlagList,NextPage);
    }
    public void AddText(string text)
    {
        content += text;
    }

    private void ClearText()
    {
        content = "";
        leftSide.pageToDisplay = -1;
        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        UpdatePagination();
    }
    
    public void EndText() 
    {
        leftSide.text = content;
        rightSide.text = content;

        this.UpdateAsObservable()
            .Select(_ => leftSide.textInfo.pageCount)
            .DistinctUntilChanged()
            .Where(x => x != 0)
            .Subscribe(x =>
            {
                UpdatePagination();
                int pageCount = _selectors.Count;
                foreach (var selector in _selectors.Select((value, index) => new { Value = value, Index = index }))
                {
                    int reversedIndex = pageCount - 1 - selector.Index;
                    var currentSelector = selector.Value.Value;

                    currentSelector.ShowCurrentDay();
                   
                    int result = _maxPageCount - 2 * reversedIndex - (_maxPageCount >= 2 && _maxPageCount % 2 == 0 ? 1 : 0);
                    ShowSelector(currentSelector.gameObject, result); // 역순으로 계산
                }
                
            });
       Managers.Game.CloseUiCanvas();
    }


    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
        _maxPageCount = rightSide.textInfo.pageCount + _selectors.Count * 2;
    }

    public void PreviousPage()
    {
        if (leftSide.pageToDisplay < 1) //0페이지는 1페이지로 표시
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1) //-2했을때 1보다크면 1이 아닌 홀수 페이지므로 -2하고 표시
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1; //아니면 페이지가1이므로 1로 표시

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //오른쪽은 그보다 +1된 페이지로 표시

        UpdatePagination(); //페이지표시업데이트
    }

    private void SelectorDisable()
    {
        if (_selectors.ContainsKey("YesOrNoBox"))
        {
            Selector yesOrNoBox = _selectors["YesOrNoBox"];
            yesOrNoBox.gameObject.SetActive(false);
            _selectors.Remove("YesOrNoBox");
        }

        if (_selectors.ContainsKey("ItemChoiceBox"))
        {
            Selector itemChoiceBox = _selectors["ItemChoiceBox"];
            itemChoiceBox.gameObject.SetActive(false);
            _selectors.Remove("ItemChoiceBox");
        }
    }
    public void NextPage()
    {
      
        if (rightSide.pageToDisplay >= _maxPageCount) //rightSide.textInfo.pageCount 이미 마지막페이지라면 넘어가지 않음
        {
            ClearText();
            foreach (var selector in _selectors)
            {
                selector.Value.NextDay();
            }

            SelectorDisable();

            EndShowSelector();
            Managers.Game.NextDay();
        }

        if (leftSide.pageToDisplay >= _maxPageCount - 1) //왼쪽이 마지막페이지-1보다 크거나 같으면
        {
            leftSide.pageToDisplay = _maxPageCount - 1; //왼쪽은 마지막페이지-1로 설정
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //오른쪽은 왼쪽보다+1된 값으로 설정
        }
        else //둘다 아니라면(끝부분이 아닌 중간 정도쯤이라면)
        {
            leftSide.pageToDisplay += 2; //걍 2더함
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1; //오른쪽은 (왼쪽페이지+1)함
        }

        UpdatePagination(); //페이지표시업데이트
    }

    private List<IDisposable> _disposables = new List<IDisposable>();
    private void ShowSelector(GameObject go, int currentPage)
    {
       
        IDisposable disposable = this.UpdateAsObservable()
            .Select(_ => leftSide.pageToDisplay)
            .Subscribe(x =>
            {
                bool active = x == currentPage;
                go.SetActive(active);
            });
        _disposables.Add(disposable);
    }

    private void EndShowSelector()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
    }


}
