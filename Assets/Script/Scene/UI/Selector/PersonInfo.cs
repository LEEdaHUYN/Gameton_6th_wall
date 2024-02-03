using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;


    public class PersonInfo : MonoBehaviour
    {
            private Image _portraitSprite;

            private void Awake()
            {
                _portraitSprite = GetComponent<Image>();
            }

            public void SetPersonInfo(Sprite image)
            {
                _portraitSprite.sprite = image;
            }

    }
