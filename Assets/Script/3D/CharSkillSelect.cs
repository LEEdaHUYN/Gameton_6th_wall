using JetBrains.Annotations;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class CharSkillSelect : MonoBehaviour
{

    private List<Button> _buttons = new List<Button>();
    public Image Popup;
    int Skillindex = 0; 

  

    /// <summary>
    /// public Color[] CharindexColor;
    /// </summary>
   /// int count = 0;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClickon(int a)
    {
        Popup.gameObject.SetActive(true);
        Skillindex = a;
    }

    public void Yesbtnclick()
    {
        ColorBlock colorBlock = _buttons[Skillindex].colors;
        colorBlock.normalColor = Color.white;
        _buttons[Skillindex].colors = colorBlock;
    }



    //���� �����Ѽ������ ���� �ٲ��
    //�丮�� -> ���� -> �ǻ�
    //�����ϴ� �����޾ƾ���

}
