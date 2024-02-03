using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personfood : MonoBehaviour
{
    [SerializeField]
    Button foodbar;
    [SerializeField]
    Button waterbar;

    float amounteat = 0.2f;
    public Toggle watertoggle;
    public Toggle foodtoggle;

    bool personbtnon = false;
    ClickFood foodscript;
    ClickFood waterscript;
    void Start()
    {
        foodscript = foodbar.GetComponent<ClickFood>();
        waterscript = waterbar.GetComponent<ClickFood>();
    }

    public void Personbtn()
    {
        if (personbtnon == false)//��� �� �ȴ�������
        {
            watertoggle.isOn = true;//���̶� ���ֱ�
            foodtoggle.isOn = true;
            personbtnon = true;//�������
        }
        else//�̹� ������ ������
        {
            watertoggle.isOn = false;//���̶� ���ֱ� ���
            foodtoggle.isOn = false;
            personbtnon = false;//����޻�
        }
    }

    public void Watertoggleclick()
    {
        if (watertoggle.isOn == false)//������
        {
            waterscript.currentfood += amounteat;//���ϱ�
        }
        else//������
        {
            waterscript.currentfood -= amounteat;//����
            
        }
    }
    public void Foodtoggleclick()
    {
        if (foodtoggle.isOn == false)//������
        {
            foodscript.currentfood += amounteat;//���ϱ�
        }
        else//������
        {
            foodscript.currentfood -= amounteat;//����

        }
    }
}
