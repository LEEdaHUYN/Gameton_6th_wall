using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickFood : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI overfoodtext;

    [SerializeField]
    private GameObject[] person;

    private Personfood[] personscript= new Personfood[4];

    public float currentfood=6f;

    Image foodBar;
    float max = 6.0f;
    bool foodbarclick = false;
    bool waterbarclick = false;
    void Awake()
    {
        foodBar = GetComponent<Image>();
        currentfood = max;
        foodBar.fillAmount = currentfood / max;
        for(int i = 0; i< person.Length; i++)
        {
            personscript[i] = person[i].gameObject.GetComponent<Personfood>();
        }
    }
    void Update()
    {
        foodBar.fillAmount = currentfood / max;
        if (currentfood > max)
        {
            overfoodtext.text = "+" + $"{(currentfood - max):N1}";
        }
        else
        {
            overfoodtext.text = "";
        }
    }

    public void Clickwaterimage()
    {
        if(waterbarclick == false)
        {
            waterbarclick = true;
            for (int i = 0; i < person.Length; i++)
            {
                personscript[i].watertoggle.isOn = true;
            }
        }
        else
        {
            waterbarclick = false;
            for (int i = 0; i < person.Length; i++)
            {
                personscript[i].watertoggle.isOn = false;
            }
        }
    }

    public void Clickfoodimage()
    {
        if (foodbarclick == false)
        {
            foodbarclick = true;
            for (int i = 0; i < person.Length; i++)
            {
                personscript[i].foodtoggle.isOn = true;
            }

        }
        else
        {
            foodbarclick = false;
            for (int i = 0; i < person.Length; i++)
            {
                personscript[i].foodtoggle.isOn = false;
            }
        }
    }

}

