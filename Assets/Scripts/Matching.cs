using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class Matching : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    static bool isAssetsLoaded;
    List <int> randomNumList = new List<int> ();
    List<GameObject> MatchedTilesPairs = new List<GameObject>();
    PointerEventData m_PointerEventData;
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    Vector3 startPos;
    List<string> MatchedObjects = new List<string>();
    int scoreCount = 0;
    bool isScoreSaved;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject scoreWindow,starContainer,starContainer2;
    public static bool isFirstPlay = true;
    public Image profilePic;

    // Start is called before the first frame update

    IEnumerator Start()
    {

        if(System.IO.File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            profilePic.sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
        }
        isScoreSaved = false;
        if(!SaveLoad.SaveExists("Matching"))
        {
            SaveLoad.Save<int>(0,"Matching");
        }
        //Screen.orientation = ScreenOrientation.Portrait;
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        isAssetsLoaded = false;
        yield return AssetLoader();
        isAssetsLoaded = true;
        profilePic.gameObject.GetComponent<Button>().onClick.AddListener(()=>
        {
            SceneManager.LoadScene("Profile");
        });
        if(isFirstPlay)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            isFirstPlay = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData.position = Input.mousePosition;
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            if(results.Count > 0 )
            if(results[0].gameObject.name.Contains("2nd") && !MatchedObjects.Contains(results[0].gameObject.name))
            {
                for(int i = 0; i < MainContentGetter.audioClipList.Count; i++)
                {
                    if(MainContentGetter.audioClipList[i].name == 
                            "_"+(results[0].gameObject.name.Split(new string[] { "2nd" },StringSplitOptions.None)[0]))
                    {
                        audioSource.clip = MainContentGetter.audioClipList[i];
                        audioSource.Play();
                    }
                }
            }
            if(results.Count > 0 )
            if(results[0].gameObject.name.Contains("1st") && !MatchedObjects.Contains(results[0].gameObject.name))
            {
                for(int i = 0; i < MainContentGetter.audioClipList.Count; i++)
                {
                    if(MainContentGetter.audioClipList[i].name == 
                            "_"+(results[0].gameObject.name.Split(new string[] { "1st" },StringSplitOptions.None)[0]))
                    {
                        audioSource.clip = MainContentGetter.audioClipList[i];
                        audioSource.Play();
                    }
                }
                if (line == null)
                {
                    startingObject = results[0].gameObject.name;
                    createLine();
                    //Getting world position of the image gameObject's right edge from the screen Position of the gameObject
                    startPos = Camera.main.ScreenToWorldPoint(new Vector3(results[0].gameObject.transform.position.x +(Screen.width*0.13194444444444f),results[0].gameObject.transform.position.y,0.0f));
                }
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                startPos.z = 0f;
                line.SetPosition(0, startPos);
                line.SetPosition(1, mousePos);
            }

        }
        else if (Input.GetMouseButtonUp(0) && line )
        {
            m_PointerEventData.position = Input.mousePosition;
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);                               
            if(results.Count > 0 && !MatchedObjects.Contains(results[0].gameObject.name))
            {
                //A new line is born
                if(results[0].gameObject.name.Contains("2nd"))
                {
                    //Getting world position of spelling gameObject's left edge from the screen Position of the gameObject
                    var endPosition = Camera.main.ScreenToWorldPoint(new Vector3 (results[0].gameObject.transform.position.x - (Screen.width*0.14583333333333f),results[0].gameObject.transform.position.y ,0.0f));
                    endPosition.z = 0;
                    line.SetPosition(1,endPosition);
                    
                                                                  //Green:  R         G           B
                    //GameObject.Find(startingObject).transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0.098039f,0.6392156f, 0.2156862f);
                    //check if startingObjectName(after ramoving "1st") == currentObjectName(after ramoving "2nd")
                    if(startingObject.Split(new string[] { "1st" },StringSplitOptions.None)[0] == results[0].gameObject.name.Split(new string[] { "2nd" },StringSplitOptions.None)[0])
                    {    
                        MatchedObjects.Add(startingObject);
                        MatchedObjects.Add(results[0].gameObject.name); 
                        GameObject.Find(startingObject).transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0,0.7924528f,0.04971686f);
                        results[0].gameObject.transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0,0.7924528f,0.04971686f);
                        line.material.color = new Color(0,0.7924528f,0.04971686f);
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success3");
                        audioSource.Play();
                        scoreCount++;
                        startingObject = null;
                        line = null;
                        currLines++;
                    }   
                    else
                    {
                        line.material.color = new Color(1f, 0.2311321f, 0.2311321f, 1f);
                        GameObject imgObj = GameObject.Find(startingObject).transform.parent.transform.parent.gameObject,
                                textObj = results[0].gameObject.transform.parent.transform.parent.gameObject;
                        textObj.GetComponentInParent<ProceduralImage>().color = new Color(1f, 0.2311321f, 0.2311321f, 1f);
                        imgObj.GetComponentInParent<ProceduralImage>().color = new Color(1f, 0.2311321f, 0.2311321f, 1f);
                        startingObject = null;
                        line = null;
                        
                        GameObject.Find("Line" + currLines).LeanAlpha(0,1f).setOnComplete(() => 
                        {
                            Destroy(GameObject.Find("Line" + (currLines - 1)));
                            textObj.GetComponentInParent<ProceduralImage>().color = new Color(0.8117647f, 0.8117647f, 0.8117647f, 1f);
                            imgObj.GetComponentInParent<ProceduralImage>().color = new Color(0.8117647f, 0.8117647f, 0.8117647f, 1f);
                        });
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Failure");
                        audioSource.Play();
                        currLines++;
                        scoreCount--;
                    }
                }
                else
                {
                    startingObject = null;
                    if(GameObject.Find("Line" + currLines) != null)
                    Destroy(GameObject.Find("Line" + currLines).gameObject);
                }
            }
            else
            {
                startingObject = null;
                //SleepforSeconds(results[0].gameObject);
                //results[0].gameObject.GetComponentInParent<Image>().color = new Color(1f, 0f, 0f, 1f);
                if(GameObject.Find("Line" + currLines) != null)
                Destroy(GameObject.Find("Line" + currLines).gameObject);
            }
        }
        else if (Input.GetMouseButton(0) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(1, mousePos);

        }
        if(MatchedObjects.Count == 10 && !isScoreSaved)
        {
            // for(int i = 0; i < 5; i++)
            // {
            //     if(i < scoreCount)
            //         starContainer.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
            //     starContainer.transform.GetChild(i).GetComponent<Image>().preserveAspect = true;
            // }
            scoreWindow.LeanScale(Vector3.one, 0.0001f).setOnComplete
            (() => { HandleTextFile.PositionAndRect(); 
            switch (scoreCount)
            {
                case 1:
                    StartCoroutine(PlayScoreAudio(false));
                    starContainer2.transform.GetChild(0).LeanScale(Vector3.one, 0.8f).setEaseOutElastic();
                break;
                case 2:                        
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        StartCoroutine(PlayScoreAudio(false));
                        starContainer2.transform.GetChild(1).LeanScale(Vector3.one, 0.8f).setEaseOutElastic();
                    });
                break;
                case 3:                      
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        audioSource.Play();
                        starContainer2.transform.GetChild(1).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                        {                                
                            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                            audioSource.Play();
                            starContainer2.transform.GetChild(2).LeanScale(Vector3.one, 0.8f).setEaseOutElastic();
                        });
                    });
                break;
                case 4:                    
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        audioSource.Play();
                        starContainer2.transform.GetChild(1).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                        {
                            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                            audioSource.Play();
                            starContainer2.transform.GetChild(2).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                            {
                                StartCoroutine(PlayScoreAudio(true));
                                starContainer2.transform.GetChild(3).LeanScale(Vector3.one, 0.8f).setEaseOutElastic();
                            });
                        });
                    });
                break;
                case 5:                      
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        audioSource.Play();
                        starContainer2.transform.GetChild(1).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                        {
                            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                            audioSource.Play();
                            starContainer2.transform.GetChild(2).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                            {
                                audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                                audioSource.Play();
                                starContainer2.transform.GetChild(3).LeanScale(Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                                {
                                    StartCoroutine(PlayScoreAudio(true));
                                    starContainer2.transform.GetChild(4).LeanScale(Vector3.one, 0.8f).setEaseOutElastic();
                                });
                            });
                        });
                    });
                break;
                default:
                    audioSource.clip = Resources.Load<AudioClip>("SFX/GameOver");
                    audioSource.Play();
                break;
            }
            });
            if(scoreCount > 0)
            SaveLoad.Save<int>(scoreCount+SaveLoad.Load<int>("Matching"),"Matching");
            isScoreSaved = true;
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
    void createLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.transform.parent = GameObject.Find("Lines").transform;
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.12f;
        line.endWidth = 0.12f;
        line.useWorldSpace = false;
        line.numCapVertices = 90;
        line.numCornerVertices = 90;
        for(int i = 0; i < GameObject.Find("Lines").transform.childCount; i++)
        {
            if(GameObject.Find("Lines").transform.GetChild(i).GetComponent<LineRenderer>().material.color.a == 0)
            {
                Destroy(GameObject.Find("Lines").transform.GetChild(i).gameObject);
            }
        }
    }
    List <int> GenerateRandomList(int minNumbers, int maxNumbers)
    {
        List <int> uniqueNumbers = new List<int> ();
        List <int> NumbersList = new List<int> ();
        for(int i = minNumbers; i < maxNumbers; i++)
        {
            uniqueNumbers.Add(i);
        }
        for(int j = minNumbers; j<maxNumbers; j++)
        {
            int ranNum = uniqueNumbers[UnityEngine.Random.Range(0,uniqueNumbers.Count)];
            NumbersList.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
        return NumbersList; 
    }
    IEnumerator AssetLoader()
    {
        List <int> randomNumbersList = new List<int> ();
        Sprite randomImage;
        randomNumbersList = GenerateRandomList(0,5);
        List<string> randomNames = new List<string>();
        List<Sprite> randomCategory = MainContentGetter.spriteList;
        for(int i = 0;i < 5; i++)
        {
            var randomFileIndex = UnityEngine.Random.Range(0, randomCategory.Count);
            randomImage = randomCategory[randomFileIndex];
            int aRandomIndex = randomNumbersList[i];
            randomCategory.RemoveAt(randomFileIndex);
            // if(randomNames.Contains(randomFileName) && randomFileName.Length < 10) continue;
            // randomNames.Add(randomFileName);
            GameObject imageGameObject = GameObject.Find(i+"1st");
            GameObject NameGameObject = GameObject.Find(aRandomIndex+"2nd");
            if(gameObject.name == "MatchTiles3 (1)")
            {
                MatchedTilesPairs.Add(imageGameObject);
                MatchedTilesPairs.Add(NameGameObject);
            }
            imageGameObject.GetComponent<ProceduralImage>().sprite = randomImage;
            imageGameObject.GetComponent<ProceduralImage>().preserveAspect = true;
            imageGameObject.name = randomImage.name+"1st";
            string itemName = randomImage.name;
            StringBuilder sb = new StringBuilder(itemName);
            for(int j = 0; j < itemName.Length; j++)
            {
                if(j == 0)
                { 
                    sb[j] = itemName[j];
                    continue;
                }
                sb[j] =  char.ToLower(itemName[j]);
            }
            NameGameObject.GetComponent<TextMeshProUGUI>().text = sb.ToString();
            NameGameObject.name = randomImage.name+"2nd";
        }
        yield return null;
    }
    
}
