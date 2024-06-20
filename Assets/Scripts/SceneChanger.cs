using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;
using UnityEngine.Networking;
using UnityEngine.Android;

public class SceneChanger : MonoBehaviour
{
    public static string previousScene = null;
    private string sceneName;
    public static bool isitfromGridView = false;
    public static string currentCategory = null;
    public static bool startCamera = false;
    public static int mainViewTargetPanelIndex = 0;
    public GameObject Loading,ScrollView,GridContents,Canvas, scrollGrid,CreditButton,CreditText,PrivacyButton,Title,Kidsdays,Share,Rate;
    public Slider ProgressBar;
    public Image profilePic;
    public bool isAdsDisabled = false;
    public RConfigs rConfigs;
    bool prv =false;
    AsyncOperation mainViewLoadingOp = null, mainAsyncOperation = null;
    string currentSceneName = null;
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if(isAdsDisabled != RConfigs.A)
        {
            isAdsDisabled = RConfigs.A;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Insert Code Here (I.E. Load Scene, Etc)
            // OR Application.Quit();
            if ((currentSceneName).Equals("MainView"))
            {
                SceneManager.LoadScene("GridView");
            }
            else if((currentSceneName).Equals("Multiplayer"))
            {
                SceneManager.LoadScene("MainView");
            }
            else if((currentSceneName).Equals("Words") )
            {
                WordSearchBot WordSearchBot =  GameObject.Find("AlphaGrid").GetComponent<WordSearchBot>(); 
                WordSearchBot.scoreWindow.SetActive(true);
                WordSearchBot.PlayAgainButton.GetComponentInChildren<TextMeshProUGUI>().text = "Keep Playing";
                WordSearchBot.PlayAgainButton.GetComponent<Button>().onClick.AddListener(() =>{
                    WordSearchBot.scoreWindow.SetActive(false);
                });
                GameObject starContainer = GameObject.Find("Stars");
                for(int i = 0; i < WordSearchBot.scoreCount; i++)
                {
                    starContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                }
            }
            else if((currentSceneName).Equals("Matching"))
            {
                StartCoroutine(WaitforLandscapeMode());
            }
            else if((currentSceneName).Equals("Quiz"))
            {
                SceneManager.LoadScene("MainView");
            }
            else if((currentSceneName).Equals("PictureTaking"))
            {
                SceneManager.LoadScene("Profile");
            }
            else if((currentSceneName).Equals("GridView"))
            {
                //its only works on Android devices not unity editor
                Application.Quit();
            }
        }
    }
    public void OnPrivacyButtonClick()
    {
        Application.OpenURL("https://sites.google.com/view/finsedsoft/");
    }   
    public void OnRateButtonClick()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.finsedsoft.kidsmultiplayersimulator");
    } 
    public void showAndHideCredits()
    {
        if(ScrollView.activeSelf)
        {
            Rate.SetActive(true);
            Share.SetActive(false);
            Kidsdays.SetActive(true);
            Title.SetActive(false);
            ScrollView.SetActive(false);
            CreditText.SetActive(true);
            PrivacyButton.SetActive(true);
            CreditButton.transform.GetChild(0).gameObject.SetActive(false);
            CreditButton.transform.GetChild(1).gameObject.SetActive(true);
            profilePic.gameObject.SetActive(false);
        }
        else
        {
            Rate.SetActive(false);
            Share.SetActive(true);
            Kidsdays.SetActive(false);
            Title.SetActive(true);
            ScrollView.SetActive(true);
            CreditText.SetActive(false);
            PrivacyButton.SetActive(false);
            CreditButton.transform.GetChild(0).gameObject.SetActive(true);
            CreditButton.transform.GetChild(1).gameObject.SetActive(false);
            profilePic.gameObject.SetActive(true);
        }
    }
    public void LoadGridViewBackbutton()
    {
        if(scrollGrid.activeInHierarchy)
        {
            scrollGrid.LeanScale(new Vector3 (0,0,1),0.5f).setOnComplete(() => 
            {
                scrollGrid.SetActive(false);
            });
        }
        else
        {
            SceneManager.LoadScene("GridView");
        }
    }
    

    public void LoadMainView()
    {
        if (!DragAndDropItem.longPress)
        {
            //save the gridview clicked item name in currentCategory string veriable
            currentCategory = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).name;
            // if(!RandomContentGetter.Category_Icons_Names.Contains(currentCategory))
            // {
            //     mainViewTargetPanelIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
            // }
            //then load the MainView based on the currentCategory
            

            Transform selectedObj = EventSystem.current.currentSelectedGameObject.transform;
            selectedObj.SetParent(Canvas.transform);
            ProgressBar.gameObject.SetActive(true);
            //ScrollView.SetActive(false);
            StartCoroutine("LoadMain");
            
            //mainViewLoadingOp.completed += OnLoadCompleted;
        }

    }
    IEnumerator LoadMain()
    {
        mainViewLoadingOp = SceneManager.LoadSceneAsync("MainView");
        
        while(!mainViewLoadingOp.isDone)
        {
            ProgressBar.value = mainViewLoadingOp.progress;
            yield return null;
        }

    }
    IEnumerator WaitforLandscapeMode()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainView");
    }    
    IEnumerator WaitforPoitraitMode(string sceneName)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        yield return new WaitForSeconds(.3f);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadScene(string sceneToLoad)
    {
        if(isitfromGridView)
        {
            SceneManager.LoadScene("GridView");
            isitfromGridView = false;
        }
        else if(sceneToLoad == "MainView")
        {
            if(Screen.orientation == ScreenOrientation.Portrait)
            StartCoroutine(WaitforLandscapeMode());
            else
            SceneManager.LoadScene(sceneToLoad);
        }
        else if(sceneToLoad == "Matching" || sceneToLoad == "Words")
        {
            if(Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                WordSearch.isFirstPlay = true;
                WordBot.currLines = 0;
                Matching.isFirstPlay = true;
                StartCoroutine(WaitforPoitraitMode(sceneToLoad));
            }
            else 
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else if(sceneToLoad == "Multiplayer")
        {
            StartCoroutine(StartMultiPlayerScene());
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    IEnumerator StartMultiPlayerScene()
    {
        GameObject noInternetPopUp = GameObject.Find("NoNetWorkPopUp"),
                    closeButton = GameObject.Find("CloseButton"),
                    netLoading = GameObject.Find("NetLoading");
        netLoading.transform.localScale =  Vector3.one;       
        LeanTween.rotateAround(netLoading, Vector3.back, 360, 2.5f).setLoopClamp();
        UnityWebRequest www = UnityWebRequest.Get("https://google.com");
        yield return www.SendWebRequest();
        if (!www.isNetworkError)
        {
            SceneManager.LoadScene("Multiplayer");
        }
        else
        {
            noInternetPopUp.LeanScale(Vector3.one,1.3f).setEaseOutElastic();
            netLoading.transform.localScale =  Vector3.zero;
            closeButton.GetComponent<Button>().onClick.AddListener(() => 
            {
                noInternetPopUp.LeanScale(Vector3.zero,0.8f).setEaseInSine();
            });
        }
    }
    public void LoadprofileOrOpenCamera()
    {
        // if(profilePic.sprite == Resources.Load<Sprite>("Images/blank-profile-picture-973460_1280"))
        // {
        //     startCamera = true;
        //     if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        //     {
        //         Permission.RequestUserPermission(Permission.Camera);
        //     }
        //     else
        //     {
        //         SceneManager.LoadScene("PictureTaking");
        //     }
        // }
        // else
        SceneManager.LoadScene("Profile");
        isitfromGridView = true;
    }
    // void OnApplicationFocus(bool focus)
    // {
    //     if (Permission.HasUserAuthorizedPermission(Permission.Camera) && startCamera)
    //     {
    //         startCamera = false;
    //         SceneManager.LoadScene("PictureTaking");
    //     }
    // }
    public void ShowAds(string sceneToLoad)
    {
        GoogleAds googleAds;
        if(currentSceneName == "Multiplayer")
            googleAds = GameObject.Find("GoogleAds").GetComponent<GoogleAds>();
        else
            googleAds = GameObject.Find("Canvas").GetComponent<GoogleAds>();
        sceneName = sceneToLoad;         

        if(googleAds.interstitialAd != null && !isAdsDisabled)
        { 
            googleAds.interstitialAd.OnAdClosed += Load;
            if(googleAds.bannerView != null) googleAds.bannerView.Destroy();
            if(googleAds.interstitialAd.IsLoaded())
                googleAds.interstitialAd.Show(); 
            else
                LoadScene(sceneName);      
        }
        else
        {
            LoadScene(sceneName);
        }
    }
    void Load(object sender, EventArgs e)
    {
        if(currentSceneName == "Multiplayer")
            SceneManager.LoadScene(sceneName);
        else
        {
            UnityMainThread.wkr.AddJob(() => {
            LoadScene(sceneName);            
            });
        }
    }
}
