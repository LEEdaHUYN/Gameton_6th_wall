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
        //[SerializeField]
        //private Image ItemDictionarycanvas;
        //[SerializeField]
        //private Button ItemDictionarybtn;
        //[SerializeField]
        //private Image Bg;
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
        //ItemDictionarycanvas.gameObject.SetActive(false);
        //Bg.gameObject.SetActive(false);
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

    //public void OpenDictionary()
    //{
    //    ShowImage(ItemDictionarycanvas);
    //    ShowImage(Bg);
    //    ItemDictionarybtn.interactable = false;
    //}
    //public void CloseDictionary()
    //{
    //    Bg.gameObject.SetActive(false);
    //    HideIamge(ItemDictionarycanvas);
    //    ItemDictionarybtn.interactable = true;
    //}

    //void HideIamge(Image canvas)
    //{
    //    var seq = DOTween.Sequence();

    //    canvas.transform.localScale = Vector3.one * 0.2f;
    //    seq.Append(canvas.transform.DOScale(1.0f, 0.2f));
    //    seq.Append(canvas.transform.DOScale(0.2f, 0.1f));
    //    seq.Play().OnComplete(() =>
    //    {
    //        canvas.gameObject.SetActive(false);
    //    });
    //}
    //void ShowImage(Image canvas)
    //{
    //    canvas.gameObject.transform.localScale = Vector3.one * 0.2f;
    //    canvas.gameObject.SetActive(true);

    //    var seq = DOTween.Sequence();
    //    seq.Append(canvas.transform.DOScale(1.1f, 0.3f));
    //    seq.Append(canvas.transform.DOScale(1f, 0.2f));

    //    seq.Play();
    //}

}
