using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toturial : MonoBehaviour
{
    [SerializeField]
    private Image[] image;

    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Button Exit;

    int currentPage;

    private void Start()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].gameObject.SetActive(false);
        }
        image[0].gameObject.SetActive(true);
        image[3].gameObject.SetActive(true);
        currentPage = 0;
        prev.gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);
    }

    public void Nextbtn()
    {
        image[currentPage].gameObject.SetActive(false);
        //Exit.gameObject.SetActive(false);
        if (currentPage < 2)
        {
            if (currentPage == 1)
            {
                next.gameObject.SetActive(false);
                Exit.gameObject.SetActive(true);
            }
            currentPage++;
            prev.gameObject.SetActive(true);
        }
        else
            currentPage = 2;
 
        image[currentPage].gameObject.SetActive(true);
    }

    public void Prevbtn()
    {
        image[currentPage].gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);
        if (currentPage > 0)
        {
            if(currentPage == 1)
            {
                  prev.gameObject.SetActive(false);
            }   
            currentPage--;
            next.gameObject.SetActive(true);
        }
        else
            currentPage = 0;
            
        image[currentPage].gameObject.SetActive(true);
    }

    public void Exitbtn()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].gameObject.SetActive(false);
        }
        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        Exit.gameObject.SetActive(false);


    }
}
