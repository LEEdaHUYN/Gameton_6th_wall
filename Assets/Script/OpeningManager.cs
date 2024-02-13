using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    [SerializeField]
    private Canvas Maincanvas;
    [SerializeField]
    private Canvas Startcanvas;
    [SerializeField]
    private Canvas Storecanvas;
    [SerializeField]
    private Canvas Roulettecanvas;

    [SerializeField]
    private Image CoinStore;
    [SerializeField]
    private Image SkillStore;

    private void Start()
    {
        Startcanvas.gameObject.SetActive(false);
        Storecanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(false);
    }


    public void ClickSkillbtn()
    {
        CoinStore.gameObject.SetActive(false);
        SkillStore.gameObject.SetActive(true);
    }
    public void ClickCoinbtn()
    {
        CoinStore.gameObject.SetActive(true);
        SkillStore.gameObject.SetActive(false);
    }
    public void MainStartBtn()
    {
        Maincanvas.gameObject.SetActive(false);  
        Startcanvas.gameObject.SetActive(true); 
    }
    public void MainStoreBtn()
    {
        Maincanvas.gameObject.SetActive(false);
        ClickCoinbtn();
        Storecanvas.gameObject.SetActive(true);
    }
    public void MainRouletteBtn()
    {
        Maincanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(true);
    }
    public void Exitbtn()
    {
        Maincanvas.gameObject.SetActive(true);
        Storecanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(false);
        Startcanvas.gameObject.SetActive(false);
    }
}
