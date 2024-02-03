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
        if (personbtnon == false)//»ç¶÷ ¾ó±¼ ¾È´­·¶À¸¸é
        {
            watertoggle.isOn = true;//¹äÀÌ¶û ¹°ÁÖ±â
            foodtoggle.isOn = true;
            personbtnon = true;//´­·¶¾î¿ä
        }
        else//ÀÌ¹Ì ´­·¯Á® ÀÖÀ¸¸é
        {
            watertoggle.isOn = false;//¹äÀÌ¶û ¹°ÁÖ±â Ãë¼Ò
            foodtoggle.isOn = false;
            personbtnon = false;//Ãë¼ÒÇÞ»ï
        }
    }

    public void Watertoggleclick()
    {
        if (watertoggle.isOn == false)//ÄÑÁö¸é
        {
            waterscript.currentfood += amounteat;//´õÇÏ±â
        }
        else//²¨Áö¸é
        {
            waterscript.currentfood -= amounteat;//»©±â
            
        }
    }
    public void Foodtoggleclick()
    {
        if (foodtoggle.isOn == false)//ÄÑÁö¸é
        {
            foodscript.currentfood += amounteat;//´õÇÏ±â
        }
        else//²¨Áö¸é
        {
            foodscript.currentfood -= amounteat;//»©±â

        }
    }
}
