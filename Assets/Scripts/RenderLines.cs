using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;

public class RenderLines : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    public RandomContentGetter randomContentGetter;
    Vector3 startPos;
    List<GameObject> lines = new List<GameObject>();
    void Start()
    {
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }
 
    void Update()
    {
        if(transform.gameObject.name == "MatchTiles3" && RandomContentGetter.isAudioPlayed && randomContentGetter.p2points[1].value != randomContentGetter.p2points[0].value + 3)
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
                            randomContentGetter.R2audioSource.clip = MainContentGetter.audioClipList[i];
                            randomContentGetter.R2audioSource.Play();
                        }
                    }
                }
                if(results.Count > 0 )
                if(results[0].gameObject.name.Contains("1st") && !MatchedObjects.Contains(results[0].gameObject.name) && !results[0].gameObject.name.Contains("_p2"))
                {
                    for(int i = 0; i < MainContentGetter.audioClipList.Count; i++)
                    {
                        if(MainContentGetter.audioClipList[i].name == 
                                "_"+(results[0].gameObject.name.Split(new string[] { "1st" },StringSplitOptions.None)[0]))
                        {
                            randomContentGetter.R2audioSource.clip = MainContentGetter.audioClipList[i];
                            randomContentGetter.R2audioSource.Play();
                        }
                    }
                    startingObject = results[0].gameObject.name;
                    if (line == null)
                    {
                        createLine();
                        //Getting world position of the image gameObject's right edge from the screen Position of the gameObject
                        startPos = Camera.main.ScreenToWorldPoint(new Vector3(results[0].gameObject.transform.position.x + 75,results[0].gameObject.transform.position.y,0.0f));
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
                m_Raycaster.Raycast(m_PointerEventData, results);                               //check if startingObjectName(after ramoving "1st") == currentObjectName(after ramoving "2nd")
                if(
                    results.Count > 0 && 
                    !MatchedObjects.Contains(results[0].gameObject.name) && 
                    startingObject.Split(new string[] { "1st" },StringSplitOptions.None)[0] == results[0].gameObject.name.Split(new string[] { "2nd" },StringSplitOptions.None)[0]
                    )
                {
                    //A new line is born
                    if(results[0].gameObject.name.Contains("2nd"))
                    {
                        //Getting world position of spelling gameObject's left edge from the screen Position of the gameObject
                        var endPosition = Camera.main.ScreenToWorldPoint(new Vector3 (results[0].gameObject.transform.position.x - 112,results[0].gameObject.transform.position.y,0.0f));
                        endPosition.z = 0;
                        line.SetPosition(1,endPosition);
                        MatchedObjects.Add(startingObject);
                        MatchedObjects.Add(results[0].gameObject.name);  
                        line.material.color = new Color(0,0.7924528f,0.04971686f);
                        //GameObject.Find(startingObject).transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0.098039f,0.6392156f, 0.2156862f);
                        results[0].gameObject.transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0.098039f,0.6392156f, 0.2156862f);
                        randomContentGetter.p1points[1].value += 1;
                        startingObject = null;
                        line = null;
                        randomContentGetter.R2audioSource.clip = Resources.Load<AudioClip>("SFX/Success");
                        randomContentGetter.R2audioSource.Play();
                        currLines++;
                    }
                    if(results[0].gameObject.name.Contains("1st"))
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
                    {
                        GameObject.Find("Line" + currLines).GetComponent<LineRenderer>().material.color = new Color(1f, 0.2311321f, 0.2311321f, 1f);
                        GameObject.Find("Line" + currLines).LeanAlpha(0, 0.5f).setOnComplete(() => 
                        {
                            if(GameObject.Find("Line" + currLines) != null)
                            Destroy(GameObject.Find("Line" + currLines));    
                            if(results.Count > 0)
                            if(!(results[0].gameObject.name.Contains("1st")))  
                            {                            
                                randomContentGetter.R2audioSource.clip = Resources.Load<AudioClip>("SFX/Failure");
                                randomContentGetter.R2audioSource.Play();
                            }                

                        });

                    }
                }
            }
            else if (Input.GetMouseButton(0) && line)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                line.SetPosition(1, mousePos);
            }
        }  
    } 
    void createLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = false;
        line.numCapVertices = 90;
        line.numCornerVertices = 90;
        lines.Add(line.gameObject);
    }
    void OnDisable()
    {
        foreach(GameObject go in lines)
        {
            Destroy(go);
        }
    }
}
