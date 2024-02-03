using System;
using Script.Manager.Contents;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Script.Tool
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private Button _newTextButton;
        [SerializeField] private BoxInfo _boxInfo;
        [SerializeField] private GameObject _textBoxPrefab;
        [SerializeField] private Note _note;
        private void Start()
        {
            _newTextButton.onClick.AddListener(OnAddText);
        }

        private void OnAddText()
        {

        }
    }
}