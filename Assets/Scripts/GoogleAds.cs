using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class GoogleAds : MonoBehaviour
{
    public BannerView bannerView;
    public InterstitialAd interstitialAd;
    public bool isInterstitialLoaded = false;
    bool isAdLoaded = false, firstCall = false, isSubscribed = false, isChecking = false;
    
    public void Start()
    {            
        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
            .SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.True)
            .SetMaxAdContentRating(MaxAdContentRating.G)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => 
        {
            StartCoroutine(CheckInternetConnection());
        });        
    }
    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-7354271640413267/7967260710";//"ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #endif
        // Create a 320x50 banner at the top of the screen.
        if(SceneManager.GetActiveScene().name == "Matching" || SceneManager.GetActiveScene().name == "Words")
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        else
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // if(!isSubscribed && this.bannerView != null)
        // {
        //     this.bannerView.OnAdFailedToLoad += AdNotLoaded;
        //     this.bannerView.OnAdLoaded += AdLoaded;
        //     isSubscribed = true;
        // }
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        this.bannerView.LoadAd(request);

    }
    // void HandleOnAdLoaded()
    // {
    //     isInterstitialLoaded = true;
    // }
    // void AdNotLoaded(object sender, AdFailedToLoadEventArgs action)
    // {
    //     isAdLoaded = false;
    //     isChecking = false;
    //     firstCall = true;
    // }
    // void AdLoaded(object sender, EventArgs action)
    // {
    //     isAdLoaded = true;
    // }
    IEnumerator CheckInternetConnection()
    {
        float timeCheck = 2.0f;//will check google.com every two seconds
        float t1;
        float t2;
        while(!isChecking)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://google.com");
            t1 = Time.fixedTime;
            yield return www.SendWebRequest();
            if (!www.isNetworkError)
            {
                if(!(SceneManager.GetActiveScene().name == "Quiz") && !(SceneManager.GetActiveScene().name == "Multiplayer"))
                this.RequestBanner();
                if(!(SceneManager.GetActiveScene().name == "GridView") && !(SceneManager.GetActiveScene().name == "MainView"))
                this.RequestAndLoadInterstitialAd();
                isChecking = true;
                break;
            }
            t2 = Time.fixedTime - t1;
            if(t2 < timeCheck)
                yield return new WaitForSeconds(timeCheck - t2);
        }
    } 
    void OnDestroy()
    {
        if(this.bannerView != null)
        this.bannerView.Destroy();
    }

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-7354271640413267/3446871280";//"ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);

        // Load an interstitial ad
        interstitialAd.LoadAd(new AdRequest.Builder().Build());
    }

    public void ShowInterstitialAd()
    {
        if(interstitialAd != null)
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
    #endregion
}
