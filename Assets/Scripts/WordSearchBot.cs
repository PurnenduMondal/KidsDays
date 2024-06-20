using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class WordSearchBot : MonoBehaviour
{
    bool isFirstRaycastObj = true;
    List<string> wordOnTheLine = new List<string>();
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    string previousRaycastObj = null;
    private LineRenderer line;
    private Vector3 thisBotPos;
    public Material material;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    Vector3 startPos,startingPoint,EndPoint,mouseVector,lineVector;
    GameObject startingObj;
    public int scoreCount = 0;
    bool isFirstPress = true,isSecondPress = false;
    int isLineVertical = -1;
    bool isScoreSaved = false;
    public GameObject scoreWindow,starContainer,starContainer2,PlayAgainButton;
    public AudioSource audioSource;
    public Image profilePic;
    int lineColorIndex;
    // Start is called before the first frame update
    void Start()
    {
        WordBot.currLines = 0;
        if(System.IO.File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            profilePic.sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
        }
        isScoreSaved = false;
        if(!SaveLoad.SaveExists("WordSearch"))
        {
            SaveLoad.Save<int>(0,"WordSearch");
        }
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        profilePic.gameObject.GetComponent<Button>().onClick.AddListener(()=>
        {
            SceneManager.LoadScene("Profile");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(WordSearch.isAlphaGridLoaded)
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            float Angle;
            if (Input.GetMouseButton(0))
            {
                m_PointerEventData.position = Input.mousePosition;
                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);
                if(results.Count > 0 && isFirstPress && results[0].gameObject.transform.parent.name == "AlphaGrid")
                {
                    startingObj = results[0].gameObject;
                    startingPoint = results[0].gameObject.transform.position;
                    previousRaycastObj = results[0].gameObject.name;
                    if (line == null)
                    {
                        lineColorIndex = UnityEngine.Random.Range(0, WordBot.lineColors.Count);
                        createLine();
                        startPos = Camera.main.ScreenToWorldPoint(results[0].gameObject.transform.position);
                    }
                    thisBotPos = Camera.main.ScreenToWorldPoint(startingPoint);
                    thisBotPos.z = 0;
                    startPos.z = 0f;
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, thisBotPos);
                    //transform.position = startingPoint;
                    isFirstPress = false;
                    isSecondPress = true;
                }
                else if(results.Count > 0 && isSecondPress && results[0].gameObject.name != previousRaycastObj && results[0].gameObject.transform.parent.name == "AlphaGrid")
                {
                    mouseVector = results[0].gameObject.transform.position - startingPoint;
                    
                    if(new Vector3(results[0].gameObject.transform.position.x, startingPoint.y,0) != startingPoint)
                    {
                        lineVector = new Vector3(results[0].gameObject.transform.position.x, startingPoint.y,0) - startingPoint;
                        float dotProduct = Vector3.Dot(mouseVector,lineVector);
                        float mouseVectorDistence = Vector3.Distance(results[0].gameObject.transform.position, startingPoint);
                        float lineVectorDistence =  Vector3.Distance(new Vector3(results[0].gameObject.transform.position.x, startingPoint.y,0), startingPoint);
                        Angle =(180/Mathf.PI) * (Mathf.Acos(dotProduct / (mouseVectorDistence * lineVectorDistence)));
                    }
                    else
                    {
                        Angle = 90.0f;
                    }
                    if(Angle < 45)
                    {
                        isLineVertical = 0;
                        EndPoint = new Vector3(results[0].gameObject.transform.position.x, startingPoint.y,0);
                        thisBotPos = Camera.main.ScreenToWorldPoint(EndPoint);
                        thisBotPos.z = 0;
                        line.SetPosition(1, thisBotPos);
                    }
                    else if(Angle > 45)
                    {
                        isLineVertical = 1;
                        EndPoint = new Vector3(startingPoint.x, results[0].gameObject.transform.position.y,0);
                        thisBotPos = Camera.main.ScreenToWorldPoint(EndPoint);
                        thisBotPos.z = 0;
                        line.SetPosition(1, thisBotPos);
                    }
                    else
                    {
                        isLineVertical = -1;
                        EndPoint = results[0].gameObject.transform.position;
                        thisBotPos = Camera.main.ScreenToWorldPoint(EndPoint);
                        thisBotPos.z = 0;
                        line.SetPosition(1, thisBotPos);
                    }
                }
                if(results.Count > 0 && results[0].gameObject.transform.parent.name == "AlphaGrid")
                previousRaycastObj = results[0].gameObject.name;
            }
            else if (Input.GetMouseButtonUp(0) && line )
            {
                isFirstPress = true;
                isSecondPress = false;
                m_PointerEventData.position = EndPoint;
                m_Raycaster.Raycast(m_PointerEventData, results);
                string searchedWord = null;
                int i = int.Parse(startingObj.name);
                if(results.Count > 0  && results[0].gameObject.transform.parent.name == "AlphaGrid")
                while(i != int.Parse(results[0].gameObject.name))
                {
                    try
                    {
                        searchedWord += GameObject.Find((i/10).ToString()+(i%10).ToString()).GetComponent<TextMeshProUGUI>().text;
                    }
                    catch
                    {
                        startingObject = null;
                        if(GameObject.Find("Line" + (3 - WordBot.currLines)) != null)
                        Destroy(GameObject.Find("Line" + (3 - WordBot.currLines)).gameObject);
                        return;
                    }
                    
                    if(isLineVertical == 0)
                    {
                        i += 1;
                    }
                    else if(isLineVertical == 1)
                    {
                        i += 10;
                    }
                    else
                    {
                        i += 11;
                    }
                }
                if(results.Count > 0  && results[0].gameObject.transform.parent.name == "AlphaGrid")
                searchedWord += GameObject.Find(results[0].gameObject.name).GetComponent<TextMeshProUGUI>().text;
                bool isObjectExist = false;
                try
                {
                    GameObject searchedObj = GameObject.Find(searchedWord);
                    searchedObj.transform.GetChild(1).GetComponent<Image>().color = WordBot.lineColors[lineColorIndex];

                    isObjectExist = true;
                }
                catch
                {
                    isObjectExist = false;
                }
                
                if(isObjectExist)
                {
                    var endPosition = Camera.main.ScreenToWorldPoint(results[0].gameObject.transform.position);
                    endPosition.z = 0;
                    line.SetPosition(1,endPosition);
                    WordBot.randomindexes.Remove(int.Parse(GameObject.Find(searchedWord).transform.GetChild(0).name));
                    WordBot.lineColors.RemoveAt(lineColorIndex);
                    startingObject = null;
                    line = null;
                    scoreCount++;
                    audioSource.clip = Resources.Load<AudioClip>("SFX/wSuccess");
                    audioSource.Play();
                    //save the current Score
                    SaveLoad.Save<int>(1+SaveLoad.Load<int>("WordSearch"),"WordSearch");
                    WordBot.currLines++;
                }
                else
                {
                    startingObject = null;
                    if(GameObject.Find("Line" + (3 - WordBot.currLines)) != null)
                    Destroy(GameObject.Find("Line" + (3 - WordBot.currLines)).gameObject);
                }
            }
        }
        if((scoreCount == 4 || WordBot.randomindexes.Count == 0) && !isScoreSaved)
        {
            scoreWindow.SetActive(true);
            // for(int i = 0; i < scoreCount; i++)
            // {
            //     starContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            // }
            switch (scoreCount)
            {
                case 1:
                    StartCoroutine(PlayScoreAudio(false));
                    starContainer2.transform.GetChild(0).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic();
                break;
                case 2:                        
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        StartCoroutine(PlayScoreAudio(false));
                        starContainer2.transform.GetChild(1).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic();
                    });
                break;
                case 3:                      
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        audioSource.Play();
                        starContainer2.transform.GetChild(1).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                        {                                
                            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                            audioSource.Play();
                            starContainer2.transform.GetChild(2).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic();
                        });
                    });
                break;
                case 4:                    
                    audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                    audioSource.Play();
                    starContainer2.transform.GetChild(0).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                    {
                        audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        audioSource.Play();
                        starContainer2.transform.GetChild(1).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                        {
                            audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                            audioSource.Play();
                            starContainer2.transform.GetChild(2).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic().setOnComplete(()=>
                            {
                                StartCoroutine(PlayScoreAudio(true));
                                starContainer2.transform.GetChild(3).LeanScale(0.7f*Vector3.one, 0.8f).setEaseOutElastic();
                            });
                        });
                    });
                break;
                default:
                    audioSource.clip = Resources.Load<AudioClip>("SFX/GameOver");
                    audioSource.Play();
                break;
            }
            PlayAgainButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play Again";
            PlayAgainButton.GetComponent<Button>().onClick.AddListener(()=> {
            SceneManager.LoadScene("Words");
            });
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
        line = new GameObject("Line" + (3 - WordBot.currLines)).AddComponent<LineRenderer>();
        line.material = material;
        line.material.color = WordBot.lineColors[lineColorIndex];
        if(WordBot.currLines == 0)
        {
            line.material.renderQueue = 2000;
        }
        else if(WordBot.currLines == 1)
        {
            line.material.renderQueue = 2450;
        }
        else 
        {
            line.material.renderQueue = 3000;
        }
        line.positionCount = 2;
        line.startWidth = 0.54f;
        line.endWidth = 0.54f;
        line.useWorldSpace = false;
        line.numCapVertices = 90;
        line.numCornerVertices = 90;
    }
}
