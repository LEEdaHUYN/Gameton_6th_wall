
    using System;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    //TODO, 
    public class UI_Fade : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _dayText;
        private Image _image;
        private Sequence sequence;
        private float _time = 1.0f;
        private void Awake()
        {
            _image = GetComponent<Image>();
            Managers.Game.SetFadeUI(this);
            sequence = DOTween.Sequence()
                    .SetAutoKill(false)
                    .OnRewind(() =>
                    {
                        this.GetComponent<RectTransform>().SetAsLastSibling();
                        _image.raycastTarget = true;
                    })
                    .Append(_image.DOFade(1.0f, _time))
                    .Join(_dayText.DOFade(1.0f, _time))
                    .Append(_image.DOFade(0.0f, _time))
                    .Join(_dayText.DOFade(0.0f, _time)).
                    SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        _image.raycastTarget = false;
                    })
                ;
        }

        public void FadeIn(int currentDay, float time = 1.0f)
        {
            _dayText.text = $"{currentDay} 일" ;
            _time = time;
            sequence.Restart();
        }

        public void FadeOut()
        {
            
        }
        
    }
