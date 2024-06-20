// using System.Collections;
// using UnityEngine;
// using UnityEngine.Advertisements;
// using UnityEngine.UI;
// public class UnityAds : MonoBehaviour, IUnityAdsListener
// {
//     private string gameId = "4129094";
//     private string bannerId = "inter5031";
//     private string intertitialId = "MyAnotherAd";
//     public bool isTestMode = true;
//     public Button showAd;

//     void Start()
//     {
//         Advertisement.Initialize(gameId, isTestMode);
//         Advertisement.AddListener(this);
//         StartCoroutine(ShowBannerWhenInitialized());
//     }
    
//     IEnumerator ShowBannerWhenInitialized () {
//         while (!Advertisement.isInitialized) {
//             yield return new WaitForSeconds(0.5f);
//         }
//         Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
//         Advertisement.Banner.Show(bannerId);
        
//         Debug.Log("Ad Showing");
//     }
//     public void showInterstitial()
//     {
//         StartCoroutine(ShowInterWhenInitialized());
//     }
//     IEnumerator ShowInterWhenInitialized() {
//         while (!Advertisement.isInitialized) 
//         {
//             yield return new WaitForSeconds(0.5f);
//         }
//         Advertisement.Show(intertitialId);
//         Debug.Log("Ad Showing");
//     }
//     public void hideBennerAds()
//     {
//         Advertisement.Banner.Hide();
//     }
//     public void OnUnityAdsDidError(string message){}
//     public void OnUnityAdsDidFinish(string placementId, ShowResult showResult){}
//     public void OnUnityAdsDidStart(string placementId){}
//     public void OnUnityAdsReady(string placementId)
//     {
//         if(placementId == bannerId)
//         {
//             // Advertisement.Banner.SetPosition(BannerPosition.CENTER);
//             // Advertisement.Banner.Show(bannerId);
            
//         }
//     }

// }
