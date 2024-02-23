
using System;
using UnityEngine.UI;
using UnityEngine;
    using UnityEngine.Purchasing;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;
using TMPro;

public class IAPManager : MonoBehaviour, IStoreListener
    {
        [Header("Product ID")]
        public readonly string productId_dia70 = "dia70";
        public readonly string productId_dia350 = "dia350";
        public readonly string productId_dia660 = "dia660";
        public readonly string productId_dia1350 = "dia1350";
        public readonly string productId_dia2050 = "dia2050";
        public readonly string removeAd = "removead";
        public readonly string removeAd500Dia = "removeaddia500";

        public Image[] PanelImage; //1dia 2package

        private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
        private void Start()
        {
            InitUnityIAP(); //Start 문에서 초기화 필수
        }

        /* Unity IAP를 초기화하는 함수 */
        private void InitUnityIAP()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            /* 구글 플레이 상품들 추가 */
            builder.AddProduct(productId_dia70, ProductType.Consumable, new IDs() { { productId_dia70, GooglePlay.Name } });
            builder.AddProduct(productId_dia350, ProductType.Consumable, new IDs() { { productId_dia350, GooglePlay.Name } });
            builder.AddProduct(productId_dia660, ProductType.Consumable, new IDs() { { productId_dia660, GooglePlay.Name } });
            builder.AddProduct(productId_dia1350, ProductType.Consumable, new IDs() { { productId_dia1350, GooglePlay.Name } });
            builder.AddProduct(productId_dia2050, ProductType.Consumable, new IDs() { { productId_dia2050, GooglePlay.Name } });
            builder.AddProduct(removeAd, ProductType.NonConsumable, new IDs() { { removeAd, GooglePlay.Name } });
            builder.AddProduct(removeAd500Dia, ProductType.NonConsumable, new IDs() { { removeAd500Dia, GooglePlay.Name } });

            UnityPurchasing.Initialize(this, builder);
        }
        
        
        public void Purchase(string productId)
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase) //상품이 존재하면서 구매 가능하면
            {
                storeController.InitiatePurchase(product); //구매가 가능하면 진행
            }
            else 
            {
                Debug.Log("상품이 없거나 현재 구매가 불가능합니다");
            }
        }
        
        #region Interface
        /* 초기화 성공 시 실행되는 함수 */
        public void OnInitialized(IStoreController controller, IExtensionProvider extension)
        {
            //Debug.Log("초기화에 성공했습니다");

            storeController = controller;
        }

        /* 초기화 실패 시 실행되는 함수 */
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            throw new System.NotImplementedException();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            throw new System.NotImplementedException();
        }

        /* 구매에 실패했을 때 실행되는 함수 */
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            //구매 실패
        }

        /* 구매를 처리하는 함수 */
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            //구매 성공
            if (args.purchasedProduct.definition.id == productId_dia70)
            {
            /* test_id 구매 처리 */
            PanelImage[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 70 + "개\n 구매완료";
            PanelImage[0].gameObject.SetActive(true);
            Managers.Back.AddCurrency(70,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == productId_dia350)
            {
            /* test_id 구매 처리 */
            PanelImage[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 350 + "개\n 구매완료";
            PanelImage[0].gameObject.SetActive(true);

            Managers.Back.AddCurrency(350,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == productId_dia660)
            {
            /* test_id 구매 처리 */
            PanelImage[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 660 + "개\n 구매완료";
            PanelImage[0].gameObject.SetActive(true);

            Managers.Back.AddCurrency(660,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == productId_dia1350)
            {
            /* test_id 구매 처리 */
            PanelImage[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 1350 + "개\n 구매완료";
            PanelImage[0].gameObject.SetActive(true);

            Managers.Back.AddCurrency(1350,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == productId_dia2050)
            {
            /* test_id 구매 처리 */
            PanelImage[0].gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 2050 + "개\n 구매완료";
            PanelImage[0].gameObject.SetActive(true);

            Managers.Back.AddCurrency(2050,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == removeAd500Dia)
            {
            /* test_id 구매 처리 */
            PanelImage[1].gameObject.SetActive(true);

            Managers.Back.AddCurrency(1, "AD");
                Managers.Back.AddCurrency(500,Define.Diamond);
            }
            else if (args.purchasedProduct.definition.id == productId_dia2050)
            {
            /* test_id 구매 처리 */
            PanelImage[2].gameObject.SetActive(true);
            Managers.Back.AddCurrency(1,"AD");
            }

            return PurchaseProcessingResult.Complete;
        }
        #endregion
        
    }
