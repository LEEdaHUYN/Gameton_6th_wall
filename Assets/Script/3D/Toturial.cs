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
    int currentPage;
    int Imagecount;

    private void Start()
    {
        Resetting();
        Imagecount = image.Length;
    }
    public void Resetting()
    {
        currentPage = 0;
        for (int i = 0; i < image.Length; i++)
        {
            image[i].gameObject.SetActive(false);
        }
        image[0].gameObject.SetActive(true);
        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
    }
    public void Nextbtn()
    {
        Debug.Log("현재 페이지" + currentPage);

        if (currentPage == image.Length - 2)
        {
            next.gameObject.SetActive(false); 
        }
        if (currentPage == image.Length)
        {
            return;
        }
            image[currentPage].gameObject.SetActive(false);
            currentPage++;
            prev.gameObject.SetActive(true);

            image[currentPage].gameObject.SetActive(true);

       


    }

    public void Prevbtn()
    {
        if (currentPage == 0)
        {
            return;
        }
        image[currentPage].gameObject.SetActive(false);
        if(currentPage == 1)
        {
                prev.gameObject.SetActive(false);
        }   
        currentPage--;
        next.gameObject.SetActive(true);
 
            
        image[currentPage].gameObject.SetActive(true);
    }

}
