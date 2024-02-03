using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Scene.Game
{
    public class BookOpenUI : MonoBehaviour
    {
        private Button btn;

        private void Awake()
        {
            btn = Utils.GetOrAddComponent<Button>(this.gameObject);
            btn.onClick.AddListener(() =>
            {
                Managers.Game.OnUiCanvas();
            });
        }
    }
}