
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PictureAndText : Selector
    {
        
        [SerializeField]
        private TMP_Text _text;

        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defalutSprite;

        public void Init(string text, string image = null)
        {
            _text.text = text;
            if (image == null)
            {
                _image.sprite = _defalutSprite;
            }
            else
            {
                Managers.Resource.Load<Sprite>(image, (success) =>
                {
                    _image.sprite = success;
                });
            }
        }
        public override void ShowCurrentDay()
        {
       
        }

        public override void NextDay()
        {

        }
    }
