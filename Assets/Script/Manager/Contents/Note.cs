using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Manager.Contents
{
    public class Note : MonoBehaviour
    {
        private const float MaxHeightSize = 0;
        
        [SerializeField] private GameObject _content;
        private RectTransform _contentRect;
        
        // 각 노트의 컨텐츠는 우선 GameObject로 관리하고자 함. && 일자가 지나면 삭제 및 새로추가 고려.
        private Dictionary<int, List<GameObject>> _pageContents = new Dictionary<int, List<GameObject>>();
        //TODO NOTE에는 페이지가 존재한다. 
        // 1. NOTE의 현재 세로 사이즈를 알아낸다.
        // 2. 만들어지는 컨텐츠가 영억을 초과하면 다음 페이지에 저장한다.
        // 3. 페이지 변화시 해당 페이지의 컨텐츠를 보여준다.
        // 4. Content는 (Text나 기타 등등에 Component를 하나 붙이자 만들어줄 때 사이즈를 해당 컴포넌트에서 알아서 계산할 수 있도록.)
        
        //TODO 끝난걸 알려줘야 첫장 빼고 false 해줄 수 있음.
        private const float ContentSize = -600f;
        private void Awake()
        {
            _contentRect = _content.GetComponent<RectTransform>();
            // this.UpdateAsObservable()
            //     .Select(_ => _contentRect.sizeDelta)
            //     .Where(x=> x.y > MaxHeightSize)
            //     .Subscribe(x =>
            //     {
            //         // 다시 짜야함
            //         Debug.Log($"값변화함 {x}");
            //         var findLast = _pageContents[_currentPage].Last();
            //         _pageContents[_currentPage].RemoveAt(_pageContents[_currentPage].Count-1);
            //         // // 또 다음 페이지 검증해야해서 싹다 false
            //         foreach (var gameObject in _pageContents[_currentPage])
            //         {
            //             gameObject.SetActive(false);
            //         }
            //         _currentPage++;
            //         AddPage(findLast);
            //     });
        }

        private void Update()
        {
            int a = 3;
        }

        private int _currentPage = 1;
        // public void AddCurrentPageText(string text)
        // {
        //     Managers.Resource.Load<GameObject>("TextBox", (success) =>
        //     {
        //         GameObject textBoxGo = Object.Instantiate(success,_content.transform);
        //         TextBox textBox = textBoxGo.GetComponent<TextBox>();
        //         textBox.SetText(text);
        //         AddPage(textBoxGo);
        //     });
        // }

        // private void AddPage(GameObject pageItem)
        // {
        //     if (!_pageContents.ContainsKey(_currentPage))
        //     {
        //         _pageContents.Add(_currentPage,new List<GameObject>());
        //            
        //     }
        //     Debug.Log("추가댐");
        //     _pageContents[_currentPage].Add(pageItem);
        // }

    }
}