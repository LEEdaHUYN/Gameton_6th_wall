
    using System;
    using GoogleMobileAds.Api;
    using UnityEngine;

    public class AdManager
    {
        #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
          private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
          private string _adUnitId = "unused";
        #endif
        
        
        private RewardedAd _rewardedAd;

        /// <summary>
        /// 보상형 광고 띄우기
        /// </summary>
        public void LoadRewardedAd(Action successCallback ,string unitId = "ca-app-pub-3940256099942544/5224354917")
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
            RewardedAd.Load(unitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                    successCallback?.Invoke();
                });
        }
        
        
    }
