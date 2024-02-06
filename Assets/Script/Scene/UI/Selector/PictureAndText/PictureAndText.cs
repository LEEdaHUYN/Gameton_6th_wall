
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PictureAndText : Selector
    {
        
        [SerializeField]
        private TMP_Text _text;

        [SerializeField] private Image _image;

        public void Init(string text, Sprite sprite = null)
        {
            _text.text = text;
            _image.sprite = sprite != null ? sprite : _image.sprite;
        }
        public override void ShowCurrentDay()
        {
       
        }

        public override void NextDay()
        {

        }
    }
