
    using System;
    using Cysharp.Threading.Tasks;

    public class BackEndManager
    {   private PlayFabAuthService authSerivce;
        public BackEndManager()
        {
            authSerivce = new PlayFabAuthService();
        }

        public void OnLogin(Action callback)
        {
            LoginAsync(callback).Forget();
        }

        private async UniTaskVoid LoginAsync(Action callback)
        {
            authSerivce.Authenticate(Authtypes.Silent);
            await UniTask.WaitUntil(() => authSerivce.SessionTicket != null);
            callback?.Invoke();
        }
    }
