using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
public class QuizScript : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    Sprite randomSprite;
    bool isAnswered = false;
    bool isQuizLoaded = false;
    public AudioSource audioSource;
    AudioClip ansAudio;
    public GameObject imageName;
    public AudioClip[] audioClips;
    public GameObject[] images;
    List<Vector3> savedPositions = new List<Vector3>();
    bool firstRun = true;
    bool isUnLoadCompeted = false;
    List<string> loadedFiles = new List<string>();
    public int scoreCounter = 0;
    public bool isCurrect = true,isScoreSaved = false,isQuizComplete = false,stopRaycasting = false;
    List<Sprite> remainingSprites = new List<Sprite>();
    public GameObject pointWindow,starsObj;
    public Image profilePic;
    // Start is called before the first frame update
    void Start()
    {
        remainingSprites = MainContentGetter.spriteList;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        isScoreSaved = false;
        if(!SaveLoad.SaveExists("Quiz"))
        {
            SaveLoad.Save<int>(0,"Quiz");
        }
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        foreach(var image in images)
        {
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2, Screen.height/2);
            image.transform.localScale = new Vector3(0.3f,0.3f,1);
        }
        for(int i = 0; i < 4; i++)
            {
                string binaryStr = System.Convert.ToString(i,2);
                int n = int.Parse(binaryStr),h = 0,w = 0,hsign=1,wsign=1;
                h=n%10;
                n=n/10;
                w=n%10;
                if(h == 1) hsign = -1;
                if(w == 1) wsign = -1;
                //put the images at the Corners of the screen
                images[i].transform.localPosition = new Vector3 (wsign*Screen.width*0.75f, hsign*Screen.height*0.75f,0f);
            } 
        
        //images[0].transform.localPosition = new Vector3 (Screen.width*0.75f, Screen.height*0.75f,0f);
        //images[1].transform.localPosition = new Vector3 (Screen.width*0.75f, -Screen.height*0.75f,0f);
        //images[2].transform.localPosition = new Vector3 (-Screen.width*0.75f, Screen.height*0.75f,0f);
        //images[3].transform.localPosition = new Vector3 (-Screen.width*0.75f, -Screen.height*0.75f,0f);
  
        // LeanTween.moveLocal(images[0],new Vector3(Screen.width*0.25f,Screen.height*0.25f,0),1.5f).setEaseOutCubic();
        // LeanTween.scale(images[0],new Vector3(1,1,1),1.5f).setEaseInExpo();
        // LeanTween.moveLocal(images[2],new Vector3(-Screen.width*0.25f, Screen.height*0.25f,0),1.5f).setEaseOutCubic();
        // LeanTween.scale(images[2],new Vector3(1,1,1),1.5f).setEaseInExpo();
        // LeanTween.moveLocal(images[1],new Vector3 (Screen.width*0.25f, -Screen.height*0.25f,0f),1.5f).setEaseOutCubic();
        // LeanTween.scale(images[1],new Vector3(1,1,1),1.5f).setEaseInExpo();
        // LeanTween.moveLocal(images[3],new Vector3 (-Screen.width*0.25f, -Screen.height*0.25f,0f),1.5f).setEaseOutCubic();
        // LeanTween.scale(images[3],new Vector3(1,1,1),1.5f).setEaseInExpo();

    }
    int k = 0;
    // Update is called once per frame
    void Update()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        if(k < 6)
        {
            if(!isQuizLoaded)
            {
                //reset the isCurrect value
                isCurrect = true;
                stopRaycasting = false;
                //Load A quiz
                StartCoroutine("LoadSpritesAndAudio");
                isQuizLoaded = true;
            }
            
            if(isAnswered)
            {
                //unLoad current Quiz
                StartCoroutine("UnloadSpritesAndAudio");
                isAnswered = false;
            }
            if(isUnLoadCompeted)
            {
                isQuizLoaded = false;
                k++;
                isUnLoadCompeted = false;
            }
        }
        if(isQuizComplete && !isScoreSaved)
        {
            pointWindow.SetActive(true);
            int stars = 0;
            if(scoreCounter <= 2)
            {
                stars = 1;
                StartCoroutine(PlayScoreAudio(false));
                LeanTween.scale(starsObj.transform.GetChild(0).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic();
            }
            else if(scoreCounter <= 4)
            {
                stars = 2;
                audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                audioSource.Play();
                LeanTween.scale(starsObj.transform.GetChild(0).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic().setOnComplete(() => 
                {
                    audioSource.Play();
                    LeanTween.scale(starsObj.transform.GetChild(1).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic();
                });
            }   
            else if(scoreCounter <= 6)
            {
                stars = 3;
                audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                audioSource.Play();
                LeanTween.scale(starsObj.transform.GetChild(0).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic().setOnComplete(() => 
                {
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    LeanTween.scale(starsObj.transform.GetChild(1).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic().setOnComplete(() => 
                    {
                        StartCoroutine(PlayScoreAudio(true));
                        LeanTween.scale(starsObj.transform.GetChild(2).gameObject, 1.5f*Vector3.one,1f).setEaseOutElastic();
                    });
                });
            } 
            else
            {
                audioSource.clip = Resources.Load<AudioClip>("SFX/GameOver");
                audioSource.Play();
                stars = 0;
            }
            
            if(System.IO.File.Exists(Application.persistentDataPath + "/saves/photo.png"))
            {
                profilePic.sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
            }
            // for(int i = 0; i < stars; i++)
            // {
            //     starsObj.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            // }     
            SaveLoad.Save<int>(stars+SaveLoad.Load<int>("Quiz"),"Quiz");
            isScoreSaved = true;
        }

        if (Input.GetMouseButtonUp(0) && !stopRaycasting)
        {
            m_PointerEventData.position = Input.mousePosition;
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);                               //check if startingObjectName(after ramoving "1st") == currentObjectName(after ramoving "2nd")
            if(results.Count > 0)
            {
                ImageButtonWrapper(results[0].gameObject.name);
            }
        }    
    }
    IEnumerator  PlayScoreAudio(bool isSuccess)
    {
        if(isSuccess)
        {
            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length - 3f);
            audioSource.clip = Resources.Load<AudioClip>("SFX/Victory");
            audioSource.Play();
        }
        else
        {
            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.clip = Resources.Load<AudioClip>("SFX/GameOver");
            audioSource.Play();
        }
    }
    IEnumerator UnloadSpritesAndAudio()
    {
        
        for(int i = 0; i < 4; i++)
        {
            string binaryStr = System.Convert.ToString(i,2);
            int n = int.Parse(binaryStr),h = 0,w = 0,hsign=1,wsign=1;
            h=n%10;
            n=n/10;
            w=n%10;
            if(h == 1) hsign = -1;
            if(w == 1) wsign = -1;
            //images[i].transform.localPosition = new Vector3 (wsign*Screen.width*0.25f, hsign*Screen.height*0.25f,0f);
            LeanTween.moveLocal(images[i],new Vector3(wsign*Screen.width*0.75f,hsign*Screen.height*0.75f,0),1.5f).setEaseOutCubic();
            LeanTween.scale(images[i],new Vector3(.3f,.3f,1),1.5f).setEaseInExpo();
        }
        yield return new WaitForSeconds(1.5f);
        loadedFiles.Clear();
        savedPositions.Clear();
        isUnLoadCompeted = true;
    }
    IEnumerator LoadSpritesAndAudio()
    {
        
        int i = 0,randIndex;
        randIndex = UnityEngine.Random.Range(0,4);
        while(i < 4) 
        {
            
            //string fileName = MainContentGetter.filesTofatch[UnityEngine.Random.Range(1, MainContentGetter.filesTofatch.Length)];
            Sprite sprite = remainingSprites[UnityEngine.Random.Range(0, remainingSprites.Count)];
            if(!loadedFiles.Contains(sprite.name))
            {
                //filePath = "Assets/Resources_moved/Images/"+MainContentGetter.filesTofatch[0]+"/"+fileName+".png";
                GameObject img = images[i];
                // yield return Addressables.LoadAssetsAsync<Sprite>(filePath, sprite =>
                // {
                
                img.name = i.ToString();
                img.transform.GetChild(0).name = sprite.name;
                img.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                img.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1);
                img.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
                // });
                if(i == randIndex)
                {
                    foreach(var clip in MainContentGetter.audioClipList)
                    {
                        if("_"+sprite.name == clip.name)
                        {
                            audioClips[1] = clip;
                            randomSprite = sprite;
                        } 
                    }
                }
                i++;
            }
            loadedFiles.Add(sprite.name);
        }
        remainingSprites.Remove(randomSprite);

        //randomSprite.name = loadedFiles[UnityEngine.Random.Range(0,loadedFiles.Count)];
        //filePath = "Assets/Resources_moved/Images/"+MainContentGetter.filesTofatch[0]+"/"+"_"+randomSprite.name+".mp3";
        //yield return Addressables.LoadAssetsAsync<AudioClip>(filePath, audioClip =>
        //{
        //audioClips[1] = audioClip;
        //});


        for(int j = 0; j < 4; j++)
        {
            string binaryStr = System.Convert.ToString(j,2);
            int n = int.Parse(binaryStr),h = 0,w = 0,hsign=1,wsign=1;
            h=n%10;
            n=n/10;
            w=n%10;
            if(h == 1) hsign = -1;
            if(w == 1) wsign = -1;
            //images[i].transform.localPosition = new Vector3 (wsign*Screen.width*0.75f, hsign*Screen.height*0.75f,0f);
            savedPositions.Add(new Vector3(wsign*Screen.width*0.25f,hsign*Screen.height*0.25f,0));
            LeanTween.moveLocal(images[j],new Vector3(wsign*Screen.width*0.25f,hsign*Screen.height*0.25f,0),1.2f).setEaseOutCubic();
            LeanTween.scale(images[j],new Vector3(1,1,1),1.2f).setEaseInExpo();
        }
        yield return new WaitForSeconds(1.2f);
        yield return StartCoroutine("PlayQuizAudioClips");
        
    }
    IEnumerator PlayQuizAudioClips()
    {
        //Play "Where is the"
        audioSource.clip = audioClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        //Play Image Name
        audioSource.clip = audioClips[1];
        audioSource.Play();
        ansAudio = audioClips[1];
        audioClips[1] = null;
    }
    void ImageButtonWrapper(string tappedImage)
    {
        StartCoroutine("RayCastResult", tappedImage);
    }
    IEnumerator RayCastResult(string tappedImage)
    {
        if(tappedImage.Equals(randomSprite.name))
        {//Currect Image
            stopRaycasting = true;
            audioSource.clip = audioClips[UnityEngine.Random.Range(2,6)];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            for(int i = 0; i < 4; i++)
            {
                //make all images invisible except TappedImage
                if(i != int.Parse(GameObject.Find(tappedImage).transform.parent.name))
                {
                    string binaryStr = System.Convert.ToString(i,2);
                    int n = int.Parse(binaryStr),h = 0,w = 0,hsign=1,wsign=1;
                    h=n%10;
                    n=n/10;
                    w=n%10;
                    if(h == 1) hsign = -1;
                    if(w == 1) wsign = -1;
                    //images[i].transform.localPosition = new Vector3 (wsign*Screen.width*0.25f, hsign*Screen.height*0.25f,0f);
                    LeanTween.moveLocal(images[i],new Vector3(wsign*Screen.width*0.75f,hsign*Screen.height*0.75f,0),.3f).setEaseOutCubic();
                }
            }     
            //Position the TappedImage at the center of the screen   
            GameObject.Find(tappedImage).transform.parent.LeanMove(new Vector3(Screen.width*0.5f, Screen.height*0.5f,0),.5f);
            //Make it Bigger
            GameObject.Find(tappedImage).transform.parent.LeanScale(new Vector3(2,2,1),.5f);
            yield return new WaitForSeconds(.5f);
            imageName.GetComponentInChildren<TextMeshProUGUI>().text = tappedImage;
            imageName.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0,0,1,1);
            imageName.SetActive(true);
            audioSource.clip = ansAudio;
            audioSource.Play();
            yield return new WaitForSeconds(1);
            GameObject.Find(tappedImage).transform.parent.localPosition = savedPositions[int.Parse(GameObject.Find(tappedImage).transform.parent.name)];
            GameObject.Find(tappedImage).transform.parent.localScale = new Vector3(1,1,1);
            imageName.SetActive(false);
            randomSprite = null;
            isAnswered = true;
            if(isCurrect)
            {
                scoreCounter++;
            }
            if(k == 5)
            {
                isQuizComplete = true;
            }
            
        }
        else
        {
            //Try Again...
            audioSource.clip = audioClips[6];
            audioSource.Play();
            isCurrect = false;
        }
    }
}