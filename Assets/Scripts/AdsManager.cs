using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsManager : Singleton<AdsManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //Ads
    public string gameIdAndroid = "4855128";
    public string gameIdIOS = "4855129";
    public string bannerPlacementIdA = "Banner_Android";
    public string interstitialPlacementIdA = "Interstitial_Android";
    public string rewardedPlacementIdA = "Rewarded_Android";
    public string bannerPlacementIdI = "Banner_iOS";
    public string interstitialPlacementIdI = "Interstitial_iOS";
    public string rewardedPlacementIdI = "Rewarded_iOS";
    
    public bool isTestMode = true;

    private IUnityAdsInitializationListener unityAdsInitializationListenerImplementation;

    private string gameId;
    private string bannerId;
    private string interstitialId;
    private string rewardId;

    // Start is called before the first frame update
    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? gameIdIOS : gameIdAndroid;
        bannerId = (Application.platform == RuntimePlatform.IPhonePlayer) ? bannerPlacementIdI : bannerPlacementIdA;
        interstitialId = (Application.platform == RuntimePlatform.IPhonePlayer) ? interstitialPlacementIdI : interstitialPlacementIdA;
        rewardId = (Application.platform == RuntimePlatform.IPhonePlayer) ? rewardedPlacementIdI : rewardedPlacementIdA;
        
        Advertisement.Initialize(gameId, isTestMode, this);
    }

    public void LoadBanner()
    {
        BannerLoadOptions bannerLoadOptions = new BannerLoadOptions()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.SetPosition(UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerId, bannerLoadOptions);
    }
    
    public void ShowBanner()
    {
        BannerOptions bannerOptions = new BannerOptions()
        {
            clickCallback = OnBannerClick,
            showCallback = OnBannerShow,
            hideCallback = OnBannerHide
        };
        Advertisement.Banner.Show(bannerId, bannerOptions);
    }
    
    public void OnInitializationComplete()
    {
        print("Ads initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print("Ads initialization failed: " + message);
    }

    public void OnBannerLoaded()
    {
        print("Banner loaded");
        ShowBanner();
    }
    
    public void OnBannerError(string message)
    {
        print("Banner error: " + message);
    }
    
    public void OnBannerClick()
    {
        print("Banner clicked");
    }
    
    public void OnBannerShow()
    {
        print("Banner shown");
    }
    
    public void OnBannerHide()
    {
        print("Banner hidden");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        throw new System.NotImplementedException();
    }
}
