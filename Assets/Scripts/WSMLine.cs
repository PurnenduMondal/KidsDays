using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class WSMLine : MonoBehaviour
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
    public int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    Vector3 startPos,startingPoint,EndPoint,mouseVector,lineVector;
    public TextMeshProUGUI wordList;
    GameObject startingObj;
    bool isFirstPress = true,isSecondPress = false;
    int isLineVertical = -1;
    bool isScoreSaved = false;
    public Image profilePic;
    public Slider[] p2Points,p1Points;
    public WSMultiplayer wsmultiplayer;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }
    public float sTimer = 0.0f;
    bool issTimerDisplayed = false;
    // Update is called once per frame
    void Update()
    {
        if(p1Points[1].value != p1Points[0].value + 3)
        {
            if(p2Points[1].value != p2Points[0].value + 3 && WSMultiplayer.isAudioPlayed) sTimer += Time.deltaTime;
            else    sTimer = 0.0f;
        }else if(!issTimerDisplayed && sTimer > 0)  
        { RandomContentGetter.userData.Add("sTimer", sTimer); issTimerDisplayed = true;}

        if(WSMultiplayer.isAudioPlayed && p2Points[1].value != p2Points[0].value + 3)
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
                        //To Find the angle between two vectors (a horizontal line and mouse pointer. startingPoint is the intersection)
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
                        //Line is 
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
                        searchedWord += GameObject.Find((i/10).ToString()+(i%10).ToString()).GetComponent<TextMeshProUGUI>().text.ToString();
                    }
                    catch
                    {
                        startingObject = null;
                        if(GameObject.Find("Line" + currLines) != null)
                        Destroy(GameObject.Find("Line" + currLines).gameObject);
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
                searchedWord += GameObject.Find(results[0].gameObject.name).GetComponent<TextMeshProUGUI>().text.ToString();
                bool isObjectExist = false;
                if(searchedWord != null)
                if(searchedWord.Equals(wordList.text.Split(' ')[0]) || searchedWord.Equals(wordList.text.Split(' ')[1]) || searchedWord.Equals(wordList.text.Split(' ')[2]))
                    isObjectExist = true;
                else
                    isObjectExist = false;

                
                if(isObjectExist)
                {
                    var endPosition = Camera.main.ScreenToWorldPoint(results[0].gameObject.transform.position);
                    endPosition.z = 0;
                    line.SetPosition(1,endPosition);
                    startingObject = null;
                    line = null;
                    p1Points[1].value += 1;
                    string[] objectNames = wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text.Split(' ');
                    wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text = "";
                    for(int j = 0; j < 3; j++)
                    {
                        if(objectNames[j].Contains(searchedWord))
                        wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text += "<color=green>"+objectNames[j]+"</color> ";
                        else
                        wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text += objectNames[j]+" ";
                    }
                    wsmultiplayer.R4audioSource.clip = Resources.Load<AudioClip>("SFX/Success3");
                    wsmultiplayer.R4audioSource.Play();
                    currLines++;
                }
                else
                {
                    startingObject = null;
                    if(GameObject.Find("Line" + currLines) != null)
                    Destroy(GameObject.Find("Line" + currLines).gameObject);
                    wsmultiplayer.R4audioSource.clip = Resources.Load<AudioClip>("SFX/Failure");
                    wsmultiplayer.R4audioSource.Play();
                }
            }

        }
    }

        void createLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.useWorldSpace = false;
        line.numCapVertices = 90;
        line.numCornerVertices = 90;
        line.startWidth = 0.8f;
        line.endWidth = 0.8f;
    }
}
