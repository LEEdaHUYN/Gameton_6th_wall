
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using PlayFab;
    using PlayFab.ClientModels;
    using UnityEngine;

    public class BackEndManager
    {   private PlayFabAuthService authSerivce;
        public BackEndManager()
        {
            authSerivce = new PlayFabAuthService();
            InitCurrencyData();
        }

        #region  Login

        public void OnLogin(Action callback)
        {
            LoginAsync(callback).Forget();
        }

        private async UniTaskVoid LoginAsync(Action callback)
        {
            authSerivce.Authenticate(Authtypes.Silent);
            await UniTask.WaitUntil(() => authSerivce.SessionTicket != null);
            //맨 마지막에 
            SyncCurrencyDataFromServer(() =>
            {
                callback?.Invoke();
            });
     
        }

        #endregion
        private Dictionary<string, int> _userCurrecy = new Dictionary<string, int>();
        #region Currency

        public void InitCurrencyData()
        {
            _userCurrecy.Add(Define.Coin,0);
            _userCurrecy.Add(Define.Diamond,0);
            _userCurrecy.Add(Define.Key,0);
        }
        
        /// <summary>
        /// 현재 재화 가져오기
        /// </summary>
        /// <param name="id">Key값</param>
        /// <returns></returns>
        public int GetCurrencyData(string id)
        {
            return _userCurrecy[id];
        }
        public int SetClientCurrencyData(string id,int data)
        {
            _userCurrecy[id] = data;
             return _userCurrecy[id];
        }
        
        /// <summary>
        /// 서버에서 데이터를 동기화 해야할 때.
        /// </summary>
        public void SyncCurrencyDataFromServer(Action callback = null)
        {
            GetCurrencyDataBackEnd(callback).Forget();
        }

        public void AddCurrency(string id,int amount,Action callback = null)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
                {
                    Amount = amount,
                    VirtualCurrency = id
                }, 
                success =>
                {
                    _userCurrecy[id] += amount;
                    callback?.Invoke();
                }, error =>
            {
                ErrorLog(error);
            });
            //_userCurrecy[id] += amount;
            
        }
        private async UniTaskVoid GetCurrencyDataBackEnd(Action callback = null)
        {
            bool isResult = false;
            PlayFabClientAPI.GetUserInventory(new PlayFab.ClientModels.GetUserInventoryRequest(),
                (result) =>
                {
                    List<string> Keys = new List<string>(_userCurrecy.Keys);
                    foreach (var item in Keys)
                    {
                        _userCurrecy[item] = result.VirtualCurrency[item];

                    }
                
                    isResult = true;
                },
                (error) => { ErrorLog(error); });
            await UniTask.WaitUntil(() => { return isResult == true; });
            callback?.Invoke();
        }
        #endregion

        #region Shop
        /// <summary>
        /// 상점의 아이템 불러오기
        /// </summary>
        /// <param name="storeId">상점 ID </param>
        /// <returns></returns>
        public async UniTask<GetStoreItemsResult> GetStoreItems(string storeId)
        {
            GetStoreItemsResult storeItems = new GetStoreItemsResult();
            bool isComplated = false;
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest() {
                StoreId = storeId
            }, Success =>
            {
                storeItems = Success;
                isComplated = true;
            },error=>ErrorLog(error));
            await UniTask.WaitUntil(() => { return isComplated == true; });
            return storeItems;
        }
        
        private List<ItemInstance> _userInventory = new List<ItemInstance>();
        

        public ItemInstance GetItem(string itemId)
        {
            return _userInventory.Find(item => item.ItemId == itemId);
        }
        /// <summary>
        /// 현재 인벤토리 가져오기.
        /// </summary>
        /// <param name="callback"></param>
        public void GetUserInventory(Action callback = null)
        {

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest { },
                result =>
                {
                    _userInventory = result.Inventory;
                    //foreach(var item in userInventory)
                    //{
                    //    Debug.Log(item.ItemClass + "," + item.RemainingUses);
                    //}
                    callback?.Invoke();


                }, error => ErrorLog(error));
        }

        


        

        /// <summary>
        /// 아이템 구매 이거 써야 API콜 호출을 백번해도 괜찮음.
        /// </summary>
        /// <param name="itemId">아이템 ID</param>
        /// <param name="price">가격은 Catalog에 있는 가격으로 해야함</param>
        /// <param name="vc">재화 String</param>
        /// <param name="successCallback">성공시 실행시킬 액션</param>
        /// <param name="failedCallBack">실패시 실행시킬 액션</param>
        public void PurchaseItem(string itemId,int price,string vc,Action successCallback,Action failedCallBack ,string StoreId = null)
        {
   

            if (StoreId == null) StoreId = Define.PublicStore;
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                ItemId = itemId,
                Price = price,
                VirtualCurrency = vc,
                StoreId = StoreId
            }, success =>
            {
                Action callbackAction = () =>
                {
                    _userCurrecy[vc] -= price;
                    SyncCurrencyDataFromServer(() =>
                    {
                        successCallback?.Invoke();
                    });
                };
                
                if (success.Items[0].ItemClass == "skill")
                {
                    var itemId = success.Items[0].ItemInstanceId;
                    UpdateItem(itemId,"false",callbackAction);
                    return;
                } 
                callbackAction?.Invoke();
                
            }, error =>
            {
                failedCallBack?.Invoke();
                ErrorLog(error);
            });
        }

        /// <summary>
        /// https://community.playfab.com/questions/4299/cloud-script-and-updating-user-data.html
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="equip"></param>
        /// <param name="callback"></param>
        public void UpdateItem(string itemId, string equip,Action callback)
        {

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdateSkill", // Arbitrary function name (must exist in your uploaded cloud.js file)
                FunctionParameter = new Dictionary<string,object> { {"ItemInstanceId", itemId }, { "equip", equip }},
                GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
            }, success =>
            {
                Debug.Log(success);
                callback?.Invoke();
            }, error => { ErrorLog(error); });
        }
        #endregion
        
        private void ErrorLog(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

    }
