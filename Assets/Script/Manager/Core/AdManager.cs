
    using System;
    using Cysharp.Threading.Tasks;
    using GoogleMobileAds.Api;
    using UnityEngine;

    public class AdManager
    {
        #region  Banner
        #if UNITY_ANDROID
        private string _adUnitBannerId = Admob.bannerId;
        #elif UNITY_IPHONE
                private string _adUnitBannerId = "ca-app-pub-3940256099942544/2934735716";
        #else
                private string _adUnitBannerId = "unused";
        #endif
        BannerView _bannerView;
        public void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyBannerView();
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(_adUnitId, AdSize.Leaderboard, AdPosition.Top);
        }
        public void LoadAdBanner()
        {
            // create an instance of a banner view first.
            if(_bannerView == null)
            {
                CreateBannerView();
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }
        
        /// <summary>
        /// Destroys the banner view.
        /// </summary>
        public void DestroyBannerView()
        {
            if (_bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        #endregion
 


        #region RewardAd

           #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
          private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
          private string _adUnitId = "unused";
        #endif
        
        
        private RewardedAd _rewardedAd;
        public void RunRewardedAd(Action successCallback,string adUnitId,Action errorCallback = null) {

                _adUnitId = adUnitId;
                LoadRewardedAd(() =>
                {
                    //핸들러 등
                    WaitTask(() =>
                    {
                        ShowRewardedAd(successCallback);

                    }).Forget();

                },errorCallback);
            
    }
        
        /// <summary>
        /// 보상형 로드 하기
        /// </summary>
        private void LoadRewardedAd(Action successCallback = null,Action errorCallback = null)
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
            
            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        errorCallback?.Invoke();
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                    RegisterEventHandlers(_rewardedAd);
                    successCallback?.Invoke();
                });
        }

        /// <summary>
        /// https://parksh3641.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-Graphics-device-is-null-%EC%97%90%EB%9F%AC-%EB%8C%80%EC%B2%98%EB%B2%95-%EA%B0%84%EB%8B%A8-%EC%84%A4%EB%AA%85#google_vignette
        /// </summary>
        /// <param name="callback"></param>
        async UniTaskVoid WaitTask(Action callback )
        {
            await UniTask.WaitForSeconds(0.2f);
            callback?.Invoke();
        } 
        
        /// <summary>
        /// 보상형 광고 띄우기
        /// </summary>
        private void ShowRewardedAd(Action succes)
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    succes?.Invoke();
                    // TODO: Reward the user.
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }
        
        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
                LoadRewardedAd();
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
            };
        }

        #endregion
     
    }
