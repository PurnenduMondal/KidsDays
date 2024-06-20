using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
using TMPro;
public class WordBot : MonoBehaviour
{
    public AnimationCurve speedCurve,pathCurve;
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    private LineRenderer line;
    private float previousAlphaPos;
    public Material material;
    public static int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    Vector3 thisGameObjectsCurrentPosition;
    public WordSearch wordSearch;
    List<GameObject> lines = new List<GameObject>();
    public static List<Color> lineColors = new List<Color>();
    public static List<int> randomindexes = new List<int>();
    Vector3 initialPosition,finalPosition;
    bool isLineCreated = true;
    int i = 0;
    int rIndex;
    int[] leanTweenTypes = {6,7,10,12,2};
    public Button helpButton;
    bool isReadyToMove = true;
    public void ShowAnswer()
    {
        if(isReadyToMove)
        {
            wordSearch.revealAnswer = true;
            isReadyToMove = false;
            StartCoroutine(StartWaitng());
        }
    }
    IEnumerator StartWaitng()
    {
        yield return new WaitForSeconds(1.5f);
        isReadyToMove = true;
        isLineCreated = true;
    }
    void Start()
    {
        //lineColors.Add(new Color(147f/255f,134f/255f,255f/255f,255f/255f));
        lineColors.Add(new Color(0f/255f,0.8323439f,0.8584906f,255f/255f));
        lineColors.Add(new Color(255f/255f,178f/255f,97f/255f,255f/255f));
        lineColors.Add(new Color(191f/255f,124f/255f,255f/255f,255f/255f));
        //lineColors.Add(new Color(255f/255f,131f/255f,168f/255f,255f/255f));
        //lineColors.Add(new Color(29f/255f,245f/255f,80f/255f,255f/255f));
        lineColors.Add(new Color(0.002881876f,0.7830189f,0,1));
        
        randomindexes = RandomContentGetter.GenerateRandomList(0,4);//new List<int>(){0,1,2,3};
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        //transform.position = Find;
    }
    
    void Update()
    {
        if(wordSearch.revealAnswer && randomindexes.Count != 0)
        {
            
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = transform.position;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            if(isLineCreated)
            {
                rIndex = Random.Range(0,randomindexes.Count);
                var speed = Random.Range(1.0f,1.5f);
                switch(randomindexes[rIndex])
                {
                    
                  case 2:   
                        initialPosition = GameObject.Find(wordSearch.startingRow2.ToString()+wordSearch.startingCol2.ToString()).transform.position; 
                        finalPosition = GameObject.Find(wordSearch.endingRow2.ToString()+wordSearch.endingCol2.ToString()).transform.position;
                        transform.position = initialPosition;
                        LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                        break;
                  case 0:   
                        initialPosition = GameObject.Find(wordSearch.startingRow0.ToString()+wordSearch.startingCol0.ToString()).transform.position; 
                        finalPosition = GameObject.Find(wordSearch.startingRow0.ToString() + (wordSearch.startingCol0+wordSearch.fileNames[0].Length - 1).ToString()).transform.position;
                        transform.position = initialPosition;
                        LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                        break;
                  case 1: 
                        initialPosition = GameObject.Find(wordSearch.startingRow1.ToString()+wordSearch.startingCol1.ToString()).transform.position;
                        if(wordSearch.is2ndVertical)
                            finalPosition = GameObject.Find((wordSearch.startingRow1+ wordSearch.fileNames[1].Length - 1).ToString()+(wordSearch.startingCol1).ToString()).transform.position;
                        else
                            finalPosition = GameObject.Find((wordSearch.startingRow1).ToString()+(wordSearch.startingCol1 + wordSearch.fileNames[1].Length - 1).ToString()).transform.position;
                        
                        transform.position = initialPosition;
                        LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                        break;
                  case 3:   
                        initialPosition = GameObject.Find(wordSearch.startingRow3.ToString()+wordSearch.startingCol3.ToString()).transform.position; 
                        finalPosition = GameObject.Find((wordSearch.endingRow3).ToString() + (wordSearch.endingCol3).ToString()).transform.position;
                        transform.position = initialPosition;
                        LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                        break;
                }
                isLineCreated = false;
            }
            if(!isLineCreated)
            {
                if(results.Count > 1)
                if(results[1].gameObject.transform.position == initialPosition)
                {
                    if (line == null)
                    {
                        createLine();
                    }
                    var startPos = Camera.main.ScreenToWorldPoint(initialPosition);
                    var endPos = Camera.main.ScreenToWorldPoint(transform.position);
                    endPos.z = 0;
                    startPos.z = 0;
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, endPos);
                }
                else if (results[1].gameObject.transform.position == finalPosition && line)
                { 
                    var endPos = Camera.main.ScreenToWorldPoint(finalPosition);
                    endPos.z = 0;
                    line.SetPosition(1,endPos);
                    line = null;
                    currLines++;
                    GameObject searchedObj = GameObject.Find(wordSearch.fileNames[randomindexes[rIndex]]);
                    searchedObj.transform.GetChild(1).GetComponent<Image>().color = lineColors[rIndex];
                    lineColors.RemoveAt(rIndex);
                    
                    wordSearch.revealAnswer = false;
                    randomindexes.RemoveAt(rIndex);
                    
                }
                else if (line)
                {
                    var endPos = Camera.main.ScreenToWorldPoint(transform.position);
                    endPos.z = 0;
                    line.SetPosition(1, endPos);
                }
            }
        }
    } 

    void createLine()
    {
        line = new GameObject("Line" + (3 - currLines)).AddComponent<LineRenderer>();
        line.material = material;
        line.material.color = lineColors[rIndex];
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
        lines.Add(line.gameObject);
    }

}
