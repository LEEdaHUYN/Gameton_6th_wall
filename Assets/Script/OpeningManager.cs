using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    //스크립트.. 정말 개발새발로 짠거 죄성함다... 최대한 빨리 끝내려다보니..
    //playing song : 과제곡 - 이무진

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
    [SerializeField]
    private Image Defaultcanvas;
    [SerializeField]
    private Image Toturial3dcanvas;
    [SerializeField]
    private Image Toturial2dcanvas;
    [SerializeField]
    private Image ItemDictionarycanvas;
    [SerializeField]
    private TextMeshPro Coin;
    [SerializeField]
    private TextMeshPro Dia;

    public LabelAnimatin Animationscript;
    public PlayBtnAnimation PlaybtnAnimation;
    public Camera Maincam;

    [SerializeField]
    private GameObject[] CameraPos = new GameObject[3];
    bool openstore = false;
    bool Playbtn = false;

    private Outline hitoutline;

    bool inmobile = true;
    public GameObject Popup;

    struct Basket
    {
        public string itemName;
        public int price;
        public string vc;
        public Action action;
    }

    List<Basket> basketList = new List<Basket>();



    private void Start()
    {
        Startcanvas.gameObject.SetActive(false);
        Store_coincanvas.gameObject.SetActive(false);
        Store_skillcanvas.gameObject.SetActive(false);
        Roulettecanvas.gameObject.SetActive(false);
        Defaultcanvas.gameObject.SetActive(false);
        Toturial3dcanvas.gameObject.SetActive(false);
        Toturial2dcanvas.gameObject.SetActive(false);
        //ItemDictionarycanvas.gameObject.SetActive(false);

    }
    private void FixedUpdate()
    {
        if (openstore == true)
        {
            StartCoroutine(Invokelabel());
            CameraPosition(0, 82f, -102f);
        }
        if (Playbtn == true)
        {
            CameraPosition(1, 87f, -84f);
            StartCoroutine(Invokeplaylabel());
        }
        else if(Playbtn == false && openstore == false)
        {
            CameraPosition(2, 1.5f, -102f);
            Animationscript.StoreLabelHide();
            PlaybtnAnimation.PlayLabelHide();
        }

    }
    void Update()
    {
        if(inmobile == true)
        {
            MobileTouch();
        }
    }

    IEnumerator Invokelabel()
    {
        yield return new WaitForSeconds(0.7f);
        Animationscript.StoreLabelShow();
    }

    IEnumerator Invokeplaylabel()
    {
        yield return new WaitForSeconds(1.3f);
        PlaybtnAnimation.PlayLabelShow();
    }

    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    private void MobileTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos;
            Ray ray;
            RaycastHit[] hits;

            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                // UI를 무시하고 처리
                if (!IsPointerOverUIObject())
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);

                    switch (hit.collider.name)
                    {
                        case ("medkit"):
                            ShowImage(Store_coincanvas);
                            HideIamge(Defaultcanvas);
                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("waterbottle"):
                            ShowImage(Store_skillcanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("default"):
                            ShowImage(Creditcanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("can"):
                            ShowImage(Roulettecanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("book"):
                            ShowImage(Startcanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("Flare"):
                            ShowImage(Toturial3dcanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        case ("GuitarA"):
                            ShowImage(Toturial2dcanvas);
                            HideIamge(Defaultcanvas);

                            hitoutline = hit.collider.GetComponent<Outline>();
                            hitoutline.OutlineColor = Color.green;
                            StartCoroutine(TurnoffOutline(hit.collider));
                            break;
                        default:

                            break;
                    }
                }
            }
            // else
            // {
            //     Debug.DrawLine(ray.origin, touchPos, Color.yellow, 1.5f);
            // }

        }

    }

    public void OnStartBtn()
    {
        Managers.Back.PurchaseItem("Key", 1, Define.Key, () =>
        {
            Debug.Log(Managers.Back.GetCurrencyData(Define.Key));
            foreach (var Item in basketList)
            {
                Managers.Back.PurchaseItem(Item.itemName, Item.price, Item.vc, () =>
                {
                    Item.action?.Invoke();
                },() =>
                { 
                    //TODO
                });
            }
            Managers.Scene.LoadScene(Define.Scene.ShipScene);
        }, () =>
        {
            Debug.Log(Managers.Back.GetCurrencyData(Define.Key));
            Popup.SetActive(true);
        });

        
    }

    IEnumerator TurnoffOutline(Collider hit)
    {
        yield return new WaitForSeconds(0.2f);
        hitoutline.OutlineColor = Color.white;
    }
        
    

    public void BacktoTitle()
    {
        Playbtn = false;
        openstore = false;
        HideIamge(Defaultcanvas);
        ShowImage(Maincanvas);

    }
    public void OpenItemDictionary()
    {
        HideIamge(Defaultcanvas);
        HideIamge(Maincanvas);
        HideIamge(Startcanvas);
        HideIamge(Store_coincanvas);
        HideIamge(Store_skillcanvas);
        ShowImage(ItemDictionarycanvas);

    }

    public void MainStartBtn()
    {
        Playbtn = true;
        HideIamge(Maincanvas);
        ShowImage(Defaultcanvas);
    }
    public void MainStoreBtn()
    {
        openstore = true;
        HideIamge(Maincanvas);
        ShowImage(Defaultcanvas);
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
        Toturial3dcanvas.GetComponent<Toturial>().Resetting();
        Toturial2dcanvas.GetComponent<Toturial>().Resetting();
        OnClearItem();
        ShowImage(Defaultcanvas);
        HideIamge(Store_skillcanvas);
        HideIamge(Store_coincanvas);
        HideIamge(Roulettecanvas);
        HideIamge(Startcanvas);
        HideIamge(Toturial3dcanvas);
        HideIamge(Toturial2dcanvas);
       
    }


    void HideIamge(Image canvas)
    {
        var seq = DOTween.Sequence();

        canvas.transform.localScale = Vector3.one * 0.2f;
        seq.Append(canvas.transform.DOScale(1.0f, 0.2f));
        seq.Append(canvas.transform.DOScale(0.2f, 0.1f));
        seq.Play().OnComplete(() =>
        {
            canvas.gameObject.SetActive(false);
        });
    }
    void ShowImage(Image canvas)
    {
        canvas.gameObject.transform.localScale = Vector3.one * 0.2f;
        canvas.gameObject.SetActive(true);

        var seq = DOTween.Sequence();
        seq.Append(canvas.transform.DOScale(1.1f, 0.3f));
        seq.Append(canvas.transform.DOScale(1f, 0.2f));

        seq.Play();
    }

    void CameraPosition(int a, float x, float y)
    {
        Maincam.transform.position = Vector3.Lerp(
        Maincam.transform.position, CameraPos[a].transform.position, 1.5f * Time.deltaTime);
        Quaternion quaternion = Quaternion.Euler(x, y, 0);
        Maincam.transform.rotation = Quaternion.Lerp(Maincam.transform.rotation, quaternion, Time.deltaTime * 1.5f);
    }

    public void OnClickCanFood()
    {
        AddItem("CanFood", 1500, Define.Coin,() =>
        {
            Managers.Game.AddItem("CanFood", 1, false);
        });
    }
    public void OnClickWater()
    {
        AddItem("Water", 1500, Define.Coin, () =>
        {
            Managers.Game.AddItem("Water", 1, false);
        });
    }

    private void AddItem(string id, int price, string vc, Action action)
    {
        Basket basket = new Basket();
        basket.itemName = id;
        basket.price = price;
        basket.vc = vc;
        basket.action = action;
        basketList.Add(basket);
    }

    public void OnClearItem()
    {
        foreach (var basket in basketList)
        {
            int currencyPrice = Managers.Back.GetCurrencyData(basket.vc) + basket.price;
            Managers.Back.SetClientCurrencyData(basket.vc, currencyPrice);
        }
        basketList.Clear();

    }
}
