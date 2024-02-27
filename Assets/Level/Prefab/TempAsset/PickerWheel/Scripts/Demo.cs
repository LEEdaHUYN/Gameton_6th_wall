using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Demo : MonoBehaviour
{
    //2 coin, 1 dia, 0 key
    [SerializeField] private Button UispinButton;
   // [SerializeField] private TextMeshProUGUI UiSpinButtonText;

    [SerializeField] private PickerWheel pickerwheel;
    [SerializeField] private Image[] panel;
    // Start is called before the first frame update
    void Start()
    {
        UispinButton.onClick.AddListener(() =>
        {
            //UispinButton.interactable = false;
          //  UiSpinButtonText.text = "spining";

            pickerwheel.OnSpinStart(() =>
            {
               // Debug.Log("spin started...");
            });


            pickerwheel.OnSpinEnd(wheelPiace =>
            {
                Debug.Log("spin end  :  Label  : " + wheelPiace.Label + ", Amount: " + wheelPiace.Amount);
                UispinButton.interactable = true;
              //  UiSpinButtonText.text = "spin";
                

                switch (wheelPiace.Label)
                {
                  
                    case "골드":
                        Managers.Back.SetClientCurrencyData(Define.Coin, Managers.Back.GetCurrencyData(Define.Coin) + wheelPiace.Amount);
                        panel[2].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + wheelPiace.Amount.ToString();
                        ShowImage(panel[2]);
                       // Debug.Log(Managers.Back.GetCurrencyData(Define.Coin));
                        break;
                    case "다이아":
                        Managers.Back.SetClientCurrencyData(Define.Diamond, Managers.Back.GetCurrencyData(Define.Diamond) + wheelPiace.Amount);
                        panel[1].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + wheelPiace.Amount.ToString();
                        ShowImage(panel[1]);
                       // Debug.Log(Managers.Back.GetCurrencyData(Define.Diamond));
                        break;
                    case "열쇠":
                        Managers.Back.SetClientCurrencyData(Define.Key, Managers.Back.GetCurrencyData(Define.Key) + wheelPiace.Amount);
                        panel[0].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + wheelPiace.Amount.ToString();
                        ShowImage(panel[0]);
                      //  Debug.Log(Managers.Back.GetCurrencyData(Define.Key));
                        break;

                }

               
            });

            if (Managers.Back.GetCurrencyData("AD") == 0)
            {
                Managers.Ad.RunRewardedAd(() =>
                {
                    pickerwheel.Spin();
                    UispinButton.interactable = false;
                    //UispinButton.interactable = false;
                },Admob.rulletId, () =>
                {
                    //요기 1일 1회 초과시 에러나는데 PopUp 띄어줘야함
                    ErrorMessagePopUp();
                });
            }
            else
            {
                if (Managers.Back.GetItem("adrullet") != null)
                {
                    ErrorMessagePopUp();
                }
                else
                {
                    Managers.Back.PurchaseItem("adrullet",0,Define.Coin, () =>
                    {
                        pickerwheel.Spin();
                        
                    }, () =>
                    {
                    
                    });
                }
             
            }
         

        });
    }

    private void ErrorMessagePopUp()
    {
        ShowImage(panel[3]);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
