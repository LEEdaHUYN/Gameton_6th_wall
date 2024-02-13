using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingPage : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private Button _NextButton;

    private List<EndingStruct> _endings = new List<EndingStruct>();
    private void Awake()
    {
        Managers.Game.EndingPage = this;
        _NextButton = Utils.GetOrAddComponent<Button>(this.gameObject);
        this.gameObject.SetActive(false);
    }
    private int currentIdx = 0;
    public void ShowEnding()
    {
        _endings = Managers.Game.GetEndingList;
        this.GetComponent<RectTransform>().SetAsLastSibling();
        NextPage(currentIdx);

    }
    private void NextPage(int idx)
    {
        if (currentIdx > _endings.Count - 1 || _endings.IsNullOrEmpty())
        {
            Managers.Game.GameOver();
            return;
        }
        
        _text.text = _endings[idx].text;
        _image.sprite = _endings[idx].sprite ? _endings[idx].sprite :    _image.sprite;
        currentIdx++;
        _NextButton.onClick.RemoveAllListeners();
        _NextButton.onClick.AddListener(()=>NextPage(currentIdx));
    }
    
}
