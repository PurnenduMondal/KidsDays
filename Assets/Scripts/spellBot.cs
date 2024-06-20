using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
using TMPro;
public class spellBot : MonoBehaviour
{
    public AnimationCurve speedCurve,pathCurve;
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    private LineRenderer line;
    private float previousAlphaPos;
    public Material material;
    public int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    Vector3 thisGameObjectsCurrentPosition;
    public WSMultiplayer wsmultiplayer;
    List<GameObject> lines = new List<GameObject>();
    List<int> randomindexes = new List<int>();
    Vector3 initialPosition,finalPosition;
    bool isLineCreated = true;
    public Slider p2points;
    public Slider[] p1Points;
    int i = 0;
    float currentTime, endTime;
    int[] leanTweenTypes = {6,7,10,12,2};
    void Start()
    {
        line = null;
        currentTime = 0.0f;
        endTime = 1.0f;
        endTime = Random.Range(RConfigs.sr1,RConfigs.sr2);
        randomindexes = RandomContentGetter.GenerateRandomList(0,3);
    }
    
    void Update()
    {
        if(WSMultiplayer.isAudioPlayed && p1Points[1].value != p1Points[0].value + 3)
        {
            var speed = Random.Range(RConfigs.sr3,RConfigs.sr4);
            currentTime += Time.deltaTime;
            if(i < 3 && currentTime >= endTime)
            {
                if(isLineCreated && line == null)
                {
                    switch(randomindexes[i])
                    {
                        
                    case 0:   
                            initialPosition = GameObject.Find(wsmultiplayer.startingRow0.ToString()+wsmultiplayer.startingCol0.ToString()+"_p2").transform.position; 
                            finalPosition = GameObject.Find(wsmultiplayer.endingRow0.ToString()+wsmultiplayer.endingCol0.ToString()+"_p2").transform.position;
                            transform.position = initialPosition;
                            LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                            break;
                    case 1:   
                            initialPosition = GameObject.Find(wsmultiplayer.startingRow1.ToString()+wsmultiplayer.startingCol1.ToString()+"_p2").transform.position; 
                            finalPosition = GameObject.Find(wsmultiplayer.startingRow1.ToString() + (wsmultiplayer.startingCol1+wsmultiplayer.objectNames[1].Length - 1).ToString()+"_p2").transform.position;
                            transform.position = initialPosition;
                            LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                            break;
                    case 2: 
                            initialPosition = GameObject.Find(wsmultiplayer.startingRow2.ToString()+wsmultiplayer.startingCol2.ToString()+"_p2").transform.position;
                            if(wsmultiplayer.is2ndWordVertical)
                                finalPosition = GameObject.Find((wsmultiplayer.startingRow2+ wsmultiplayer.objectNames[2].Length - 1).ToString()+(wsmultiplayer.startingCol2).ToString()+"_p2").transform.position;
                            else
                                finalPosition = GameObject.Find((wsmultiplayer.startingRow2).ToString()+(wsmultiplayer.startingCol2 + wsmultiplayer.objectNames[2].Length - 1).ToString()+"_p2").transform.position;
                            
                            transform.position = initialPosition;
                            LeanTween.move(gameObject, finalPosition, speed).setEase((LeanTweenType) leanTweenTypes[Random.Range(0,leanTweenTypes.Length)]);
                            break;
                    }
                    createLine();                      
                    var startPos = Camera.main.ScreenToWorldPoint(initialPosition);
                    var endPos = Camera.main.ScreenToWorldPoint(transform.position);
                    endPos.z = 0;
                    startPos.z = 0;
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, endPos);
                    isLineCreated = false;
                }
                if(!isLineCreated && line)
                {
                    if (transform.position == finalPosition)
                    { 
                        var endPos = Camera.main.ScreenToWorldPoint(finalPosition);
                        endPos.z = 0;
                        line.SetPosition(1,endPos);
                        line = null;
                        currLines++;
                        isLineCreated = true;
                        string[] objectNames = wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text.Split(' ');

                        wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text = "";
                        for(int j = 0; j < 3; j++)
                        {
                            if(randomindexes[i] == j)
                            wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text += "<color=green>"+objectNames[j]+"</color> ";
                            else
                            wsmultiplayer.wordContainer.GetComponent<TextMeshProUGUI>().text += objectNames[j]+" ";
                        }
                        endTime = Random.Range(RConfigs.sr1,RConfigs.sr2);
                        currentTime = 0;
                        p2points.value += 1;
                        i++;
                    }
                    else
                    {
                        var endPos = Camera.main.ScreenToWorldPoint(transform.position);
                        endPos.z = 0;
                        line.SetPosition(1, endPos);
                    }
                }
            }
        }
    } 

    void createLine()
    {
        line = new GameObject("BotLine" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.8f;
        line.endWidth = 0.8f;
        line.useWorldSpace = false;
        line.numCapVertices = 90;
        line.numCornerVertices = 90;
        lines.Add(line.gameObject);
    }

}