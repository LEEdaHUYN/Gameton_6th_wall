using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Script.Manager.Contents
{
    public class TextBox : MonoBehaviour
    {
        private Vector2 _boxSize;
        private RectTransform _rectTransform;
        [SerializeField] private TMP_Text  _viewText;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _boxSize = _rectTransform.sizeDelta;
            this.UpdateAsObservable()
                .Select(_ => GetPageCount() != 0)
                .DistinctUntilChanged()
                .Where(x => x)
                .Subscribe(_ => SetHeightBox());
        }

        private Action _sizeCheckAction;
        public void SetText(string text)
        {
            _viewText.text = text;
        }
        private void SetHeightBox()
        {
            _rectTransform.sizeDelta = new Vector2(_boxSize.x, GetHeightSize());
        }
        public String GetText() => _viewText.text;

        public int GetPageCount()
        {
            return _viewText.textInfo.pageCount;
        }

        public float GetHeightSize()
        {
            return _boxSize.y + ((GetPageCount()-1) * 50);
        }



    }
}