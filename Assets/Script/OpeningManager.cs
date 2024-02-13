using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    [SerializeField]
    private Image Maincanvas;
    [SerializeField]
    private Image Startcanvas;
    [SerializeField]
    private Image Store_coincanvas;
    [SerializeField]
    private Image Store_skillcanvas;
    [SerializeField]
    private Image Roulettecanvas;
    [SerializeField]
    private Image Creditcanvas;

    public Camera Maincam;
    public GameObject CameraPos;
    bool openstore = false;

    [SerializeField]
    private Canvas[] rebel;
    [SerializeField]
    private GameObject[] StoreObejct;


    Vector3 velo = Vector3.zero;

    private void Start()
    {
        Startcanvas.gameObject.SetActive(false);
        Store_coincanvas.gameObject.SetActive(false);
        Store_skillcanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (openstore == true)
        {
            Maincam.transform.position = Vector3.Lerp(
            Maincam.transform.position, CameraPos.transform.position, 1.5f * Time.deltaTime);
            Quaternion quaternion = Quaternion.Euler(82f, -102f, 0);
            Maincam.transform.rotation = Quaternion.Lerp(Maincam.transform.rotation, quaternion, Time.deltaTime * 1.5f);
        }
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // 싱글 터치.
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos;
            Ray ray;
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Vector3 touchPosToVector3 = new Vector3(touch.position.x, touch.position.y, 100);
                    touchPos = Camera.main.ScreenToWorldPoint(touchPosToVector3);
                    ray = Camera.main.ScreenPointToRay(touchPosToVector3);

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);
                        hit.transform.GetComponent<Outline>().OutlineColor = Color.green;

                        if (hit.collider.name == "medkit")
                        {
                            Store_coincanvas.gameObject.SetActive(true);

                            var seq = DOTween.Sequence();

                            seq.Append(Store_coincanvas.transform.DOScale(1.1f, 0.2f));
                            seq.Append(Store_coincanvas.transform.DOScale(1f, 0.1f));

                            seq.Play();
                        }
                        else if (hit.collider.name == "waterbottle")
                        {
                            Store_skillcanvas.gameObject.SetActive(true);

                            var seq = DOTween.Sequence();

                            seq.Append(Store_skillcanvas.transform.DOScale(1.1f, 0.2f));
                            seq.Append(Store_skillcanvas.transform.DOScale(1f, 0.1f));

                            seq.Play();
                        }
                        else if (hit.collider.name == "default")
                        {
                            Creditcanvas.gameObject.SetActive(true);

                            var seq = DOTween.Sequence();

                            seq.Append(Creditcanvas.transform.DOScale(1.1f, 0.2f));
                            seq.Append(Creditcanvas.transform.DOScale(1f, 0.1f));

                            seq.Play();
                        }
                        else if (hit.collider.name == "can")
                        {
                            Roulettecanvas.gameObject.SetActive(true);

                            var seq = DOTween.Sequence();

                            
                            seq.Append(Roulettecanvas.transform.DOScale(1.1f, 0.2f));
                            seq.Append(Roulettecanvas.transform.DOScale(1f, 0.1f));

                            seq.Play();
                        }
                    }
                    else
                    {
                        Debug.DrawLine(ray.origin, touchPos, Color.yellow, 1.5f);
                    }

                    break;
            }
        }
    }

  
    public void MainStartBtn()
    {
        Maincanvas.gameObject.SetActive(false);  
        Startcanvas.gameObject.SetActive(true); 
    }
    public void MainStoreBtn()
    {
        openstore = true;
        var seq = DOTween.Sequence();

        Maincanvas.transform.localScale = Vector3.one * 0.2f;
      
        seq.Append(Maincanvas.transform.DOScale(1.2f, 0.3f));
        seq.Append(Maincanvas.transform.DOScale(0.2f, 0.3f));
        seq.Play().OnComplete(() =>
        {
            Maincanvas.gameObject.SetActive(false);
        });
        //ClickCoinbtn();
        //Storecanvas.gameObject.SetActive(true);
    }

    public void MainRouletteBtn()
    {
        Maincanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(true);
    }
    public void Exitbtn()
    {
        Maincanvas.gameObject.SetActive(true);
        Store_skillcanvas.gameObject.SetActive(false);
        Store_coincanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(false);
        Startcanvas.gameObject.SetActive(false);
    }
}
