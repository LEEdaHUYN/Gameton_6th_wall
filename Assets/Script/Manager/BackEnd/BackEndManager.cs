
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using PlayFab;
    using PlayFab.ClientModels;
    using UnityEditor.PackageManager;
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
        // public int SetClientCurrencyData(string id,int data)
        // {
        //     _userCurrecy[id] = data;
        //     return _userCurrecy[id];
        // }
        
        /// <summary>
        /// 서버에서 데이터를 동기화 해야할 때.
        /// </summary>
        public void SyncCurrencyDataFromServer(Action callback = null)
        {
            GetCurrencyDataBackEnd(_userCurrecy,callback).Forget();
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
        private async UniTaskVoid GetCurrencyDataBackEnd(Dictionary<string, int> CurrencyList,Action callback = null)
        {
            bool isResult = false;
            PlayFabClientAPI.GetUserInventory(new PlayFab.ClientModels.GetUserInventoryRequest(),
                (result) =>
                {
                    List<string> Keys = new List<string>(CurrencyList.Keys);
                    foreach (var item in Keys)
                    {
                        CurrencyList[item] = result.VirtualCurrency[item];

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
        public async UniTask<List<StoreItem>> GetStoreItems(string storeId)
        {
            List<StoreItem> storeItems = new List<StoreItem>();
            bool isComplated = false;
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest() {
                StoreId = storeId
            }, Success => {
                storeItems = Success.Store;
                isComplated = true;
            },error=>ErrorLog(error));
            await UniTask.WaitUntil(() => { return isComplated == true; });
            return storeItems;
        }
        

        /// <summary>
        /// 아이템 구매 이거 써야 API콜 호출을 백번해도 괜찮음.
        /// </summary>
        /// <param name="itemId">아이템 ID</param>
        /// <param name="price">가격은 Catalog에 있는 가격으로 해야함</param>
        /// <param name="vc">재화 String</param>
        /// <param name="SuccesCallback">성공시 실행시킬 액션</param>
        public void PurchaseItem(string itemId,int price,string vc,Action SuccesCallback, string StoreId = null)
        {
   
            _userCurrecy[vc] -= price;
            if (StoreId == null) StoreId = Define.PublicStore;
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                ItemId = itemId,
                Price = price,
                VirtualCurrency = vc,
                StoreId = StoreId
            }, success => {
                // TODO 상점 구현? 열쇠소모도 이걸로 해야 할 듯.
                // for (int i = 0; i < success.Items.Count; i++)
                // {
                //     int haveQuantity = FindItemQuantity(success.Items[i].ItemId);
                //     if(haveQuantity == 0)
                //     {
                //         userInventory.Add(success.Items[i]);
                //     }
                //     else
                //     {
                //         int idx = FindItemIdx(success.Items[i].ItemId);
                //         userInventory[idx].RemainingUses = success.Items[i].RemainingUses;
                //     }
                //
                // }
                SuccesCallback?.Invoke(); 
            }, error => ErrorLog(error));
        }
        #endregion
        
        private void ErrorLog(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

    }
