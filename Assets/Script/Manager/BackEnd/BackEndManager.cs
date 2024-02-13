
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using PlayFab;
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
            GetCurrencyDataBackEnd(_userCurrecy,callback).Forget();
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
        
        private void ErrorLog(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

    }
