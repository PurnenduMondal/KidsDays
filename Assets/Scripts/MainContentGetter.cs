using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using PolyAndCode.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using TMPro;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Firebase;
using UnityEngine.Networking;

public class MainContentGetter : MonoBehaviour, IRecyclableScrollRectDataSource
{
    AsyncOperationHandle<IList<Object>> spriteOperationHandle;
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    public static string[] filesTofatch = null;
    public SimpleScrollSnap sssComponent;
    public GameObject itemPrefab,Loading;
    List<Sprite> allImages = new List<Sprite>();
    public static List<Sprite> spriteList = new List<Sprite>();
    public AudioClip[] letsPlayAudio;
    public TextMeshProUGUI spellText;
    public AudioSource spellAudioSource;
    public static List<AudioClip> audioClipList = new List<AudioClip>();
    public GameObject Quiz,Matching,Words,Multiplayer,ScrollGrid,gridItem;
    bool isContentLoaded = false,isGamesShown = false, isAudioPlayed = false,isSpellOn = false;
    string currentPanelName = null, previousPanelName = null;
    int prvindex = -1;
    bool isFirst = true;
    bool prv = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        isGamesShown = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return StartCoroutine("LoadAssets");
        sssComponent.snappingSpeed = 10;
        sssComponent.GoToPanel(SceneChanger.mainViewTargetPanelIndex);
        SceneChanger.mainViewTargetPanelIndex = 0;
        isContentLoaded = true;
        StartCoroutine(FatchRemoteConfigs());        
    }

    public int GetItemCount()
    {
        return 100000000;
    }
    public void SetCell(ICell cell, int index)
    {
        var item = cell as MainViewCell;
        item.ConfigureCell(spriteList[index%spriteList.Count], index%spriteList.Count);
        prvindex = index;
    }
    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(1.5f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = letsPlayAudio[0];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);            
        Quiz.LeanScale(new Vector3(1,1,1),0.5f);
        Matching.LeanScale(new Vector3(1,1,1),0.5f);
        Multiplayer.LeanScale(new Vector3(1,1,1),0.5f);
        Words.LeanScale(new Vector3(1,1,1),0.5f);
        isGamesShown = true;
        audioSource.clip = letsPlayAudio[1];
        audioSource.Play();
    }
    public void HandleOnPanelChanging()
    {

        if(isGamesShown)
        {
            Quiz.LeanScale(new Vector3(0,0,1),0.5f);
            Matching.LeanScale(new Vector3(0,0,1),0.5f);
            Multiplayer.LeanScale(new Vector3(0,0,1),0.5f);
            Words.LeanScale(new Vector3(0,0,1),0.5f);
            isGamesShown = false;
        }
    }
    public void HandleOnPanelChanged()
    {
        if(SceneChanger.currentCategory == "Numbers")
        {
            float s = Random.Range(0,2);
            float t = 1-s;
            float r = Random.Range(0.0f,1.0f);
            List<float> l = GenerateRandomList(new List<float>{s,t,r});
            transform.GetChild(sssComponent.DetermineNearestPanel()).GetComponentInChildren<Image>().color = new Color(l[0],l[1],l[2],1);
        }
        if(transform.GetChild(sssComponent.DetermineNearestPanel()).GetComponent<MainViewCell>().itemIndex == spriteList.Count -1)
        {
            StartCoroutine(PlayAudio());
        }
    }
    List<float> GenerateRandomList(List<float> n)
    {
        List <float> uniqueNumbers = new List<float> ();
        List <float> NumbersList = new List<float> ();
        for(int i = 0; i < n.Count; i++)
        {
            uniqueNumbers.Add(n[i]);
        }
        for(int i = 0; i< n.Count; i++)
        {
            float ranNum = uniqueNumbers[Random.Range(0,uniqueNumbers.Count)];
            NumbersList.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
        return NumbersList; 
    }
    public void OnSpellButtonClick()
    {
        if(isSpellOn)
        {
           //Play Pronounciation;
            for (int j = 0; j < audioClipList.Count; j++)
            {
                if(audioClipList[j].name == "_"+currentPanelName) spellAudioSource.clip = audioClipList[j];
            }
            isSpellOn = false; 
        } 
        else 
        {
            isSpellOn = true;
            //Play Spelling;
            for (int j = 0; j < audioClipList.Count; j++)
            {
                if(audioClipList[j].name == currentPanelName) spellAudioSource.clip = audioClipList[j];
            }
            if(!spellAudioSource.isPlaying)
            {
                spellAudioSource.Play();
            }
            isSpellOn = false;
        }
    }
    // Update is called once per frame
    void Update()
    {  
        if(prv != RConfigs.M)
        {
            Multiplayer.SetActive(!(RConfigs.M));
            prv = RConfigs.M;
        }
        if(sssComponent.Panels != null)
        {
            currentPanelName = sssComponent.Panels[sssComponent.CurrentPanel].name;

            if (!currentPanelName.Equals(previousPanelName))
            {
                spellText.text = currentPanelName;
                
                for (int j = 0; j < audioClipList.Count; j++)
                {
                    //Spelling Audio
                    if(isSpellOn && audioClipList[j].name == currentPanelName) spellAudioSource.clip = audioClipList[j];
                    //Pronunciation Audio
                    if(!isSpellOn && audioClipList[j].name == "_"+currentPanelName) spellAudioSource.clip = audioClipList[j];
                }
                if(sssComponent.thresholdSnappingSpeed == -1)
                {
                    spellAudioSource.Play();
                }
            }
            previousPanelName = currentPanelName;
        }
        // if(transform.childCount > 0)
        // {        
        //     if(transform.GetChild(0).GetComponent<MainViewCell>().itemIndex == 0)
        //         //|| transform.GetChild(sssComponent.DetermineNearestPanel()).GetComponent<MainViewCell>().itemIndex == spriteList.Count -1)
        //     //(sssComponent.DetermineNearestPanel() == (spriteList.Count -1)%sssComponent.Content.transform.childCount && transform.GetChild((spriteList.Count -1)%sssComponent.Content.transform.childCount).GetComponent<MainViewCell>().itemIndex == spriteList.Count - 1))
        //     {
        //         sssComponent.infinitelyScroll = false;
        //         _recyclableScrollRect.movementType = ScrollRect.MovementType.Clamped;
        //     }
        //     else
        //     {
        //         sssComponent.infinitelyScroll = true;
        //         _recyclableScrollRect.movementType = ScrollRect.MovementType.Elastic;
        //     }
        // }
        // if (isFirst && isContentLoaded)
        // {
        //     sssComponent.snappingSpeed = 10;
        //     isFirst = false;
        // }

    }

    IEnumerator LoadAssets()
    {  
        Loading.gameObject.SetActive(true);
        spriteOperationHandle = Addressables.LoadAssetsAsync<Object>(SceneChanger.currentCategory, null);
        yield return spriteOperationHandle;
        if(spriteOperationHandle.Status == AsyncOperationStatus.Succeeded)
        spriteOperationHandle.Completed += OnLoadAssets;
        else
        Debug.Log("Failed to Fatch");
    }
    void OnLoadAssets (AsyncOperationHandle<IList<Object>> spriteListHandle)
    {
        Loading.gameObject.SetActive(false);
        spriteList.Clear();
        audioClipList.Clear();
        for(int i = 0; i < spriteListHandle.Result.Count; i++)
        {
            bool isSpriteExists = false, isAudioExists = false;
            if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.Texture2D")
            {
                Texture2D texture = (Texture2D) spriteListHandle.Result[i];
                Sprite s = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
                //s.name = Regex.Replace(texture.name, @"\d", "");
                s.name = texture.name;
                if(i != 0)
                for (int j = 0; j < spriteList.Count; j++)
                {
                    if(spriteList[j].name == s.name) isSpriteExists = true;
                }
                if(!isSpriteExists)
                {
                    spriteList.Add(s);
                }
            }
            if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.AudioClip")
            {
                AudioClip audio = (AudioClip) spriteListHandle.Result[i];
                //audio.name = Regex.Replace(audio.name, @"\d", "");
                if(i != 0)
                for (int j = 0; j < audioClipList.Count; j++)
                {
                    if(audioClipList[j].name == audio.name) isAudioExists = true;
                }
                if(!isAudioExists)
                {
                    audioClipList.Add(audio);
                }
            }
        }
        if(SceneChanger.currentCategory == "Numbers")
        {
                        //this will arange the number images in currect order
            spriteList = spriteList.OrderBy(sprite => Regex.Replace(sprite.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
            audioClipList = audioClipList.OrderBy(clip => Regex.Replace(clip.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
            foreach(var s in spriteList)
                s.name = Regex.Replace(s.name, @"\d", "");
            foreach(var a in audioClipList)
                a.name = Regex.Replace(a.name, @"\d", "");
        }
        _recyclableScrollRect.enabled = true;
        sssComponent.enabled = true;
        _recyclableScrollRect.DataSource = this;

        
    }

    void OnGamesButtonClick()
    {
        if(!isGamesShown)
        {
            Quiz.LeanScale(new Vector3(1,1,1),0.5f);
            Matching.LeanScale(new Vector3(1,1,1),0.5f);
            Multiplayer.LeanScale(new Vector3(1,1,1),0.5f);
            Words.LeanScale(new Vector3(1,1,1),0.5f);
            isGamesShown = true;
        }
        else
        {
            Quiz.LeanScale(new Vector3(0,0,1),0.5f);
            Matching.LeanScale(new Vector3(0,0,1),0.5f);
            Multiplayer.LeanScale(new Vector3(0,0,1),0.5f);
            Words.LeanScale(new Vector3(0,0,1),0.5f);
            isGamesShown = false;
        }
    }
    public void ViewAll()
    {
        if(ScrollGrid.GetComponent<ScrollRect>().content.transform.childCount == 0)
        for(int i = 0; i < spriteList.Count; i++)
        {
            GameObject go = Instantiate(gridItem, ScrollGrid.transform.GetChild(0).GetChild(0));
            go.transform.GetChild(0).name = i.ToString();
            go.GetComponent<Button>().onClick.AddListener(OnGridItemClick);
            go.transform.GetChild(0).GetComponent<Image>().sprite = spriteList[i];
            go.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
        } 
        ScrollGrid.GetComponent<RectTransform>().localScale = Vector3.one;
        ScrollGrid.SetActive(true);
    }
    public void OnGridItemClick()
    {
        //ScrollGrid.LeanScale(Vector3.zero, 0.5f);
        ScrollGrid.SetActive(false);
        StartCoroutine("PanelSearch");
    }
    IEnumerator PanelSearch()
    {
        int i = int.Parse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).name);
        sssComponent.thresholdSnappingSpeed = 1000;
        
        while(sssComponent.Content.GetChild(sssComponent.DetermineNearestPanel()).GetComponent<MainViewCell>().itemIndex != i)
        {
            if(i < sssComponent.Content.GetChild(sssComponent.DetermineNearestPanel()).GetComponent<MainViewCell>().itemIndex)
            {
                sssComponent.GoToPreviousPanel();
                yield return null;
            }
            else
            {
                sssComponent.GoToNextPanel();
                yield return null;
            }
        } 
        sssComponent.thresholdSnappingSpeed = -1;
    }
    void OnDisable()
    {
        spriteOperationHandle.Completed -= OnLoadAssets;
    }
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    IEnumerator FatchRemoteConfigs()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://google.com");
        yield return www.SendWebRequest();
        if (!www.isNetworkError)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                    Dictionary<string, object> defaults = new Dictionary<string, object>();
                    defaults.Add("config_test_string", "default local string");
                    defaults.Add("config_test_int", 1);
                    defaults.Add("config_test_float", 1.0);
                    defaults.Add("config_test_bool", false); 
                    FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWith(atask => {
                        Debug.Log("RemoteConfig configured and ready!");
                        FetchDataAsync();
                    });
                } else {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
    }
    public Task FetchDataAsync() {
      Debug.Log("Fetching data...");
      //FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero) use it to get the latest data instantly
    //By Default the cacheExpiration time is 12 hours
      Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync();
      return fetchTask.ContinueWith(FetchComplete);
    }
    void FetchComplete(Task fetchTask) {
      if (fetchTask.IsCanceled) {
        Debug.Log("Fetch canceled.");
      } else if (fetchTask.IsFaulted) {
        Debug.Log("Fetch encountered an error.");
      } else if (fetchTask.IsCompleted) {
        Debug.Log("Fetch completed successfully!");
      }

      var info = FirebaseRemoteConfig.DefaultInstance.Info;
      switch (info.LastFetchStatus) {
        case LastFetchStatus.Success:
          FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWith(task => 
          {
            Debug.Log(System.String.Format("Remote data loaded and ready (last fetch time {0}).",info.FetchTime));
            RConfigs.jsonConfig = FirebaseRemoteConfig.DefaultInstance.GetValue("Configs").StringValue;
            RConfigs.M = FirebaseRemoteConfig.DefaultInstance.GetValue("isMultiplayerDisabled").BooleanValue;
            RConfigs.A = FirebaseRemoteConfig.DefaultInstance.GetValue("isAdsDisabled").BooleanValue;
          });
          break;
        case LastFetchStatus.Failure:
          switch (info.LastFetchFailureReason) {
            case FetchFailureReason.Error:
              Debug.Log("Fetch failed for unknown reason");
              break;
            case FetchFailureReason.Throttled:
              Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
              break;
          }
          break;
        case LastFetchStatus.Pending:
          Debug.Log("Latest Fetch call still pending.");
          break;
      }
    }
}
