using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Scene.Game
{
    public class GameBackGround : MonoBehaviour
    {
        [SerializeField] 
        private Image _image;
        private void Awake()
        {
            Managers.Game.SetBackGround(this);
        }

        public void SetBackGround(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}