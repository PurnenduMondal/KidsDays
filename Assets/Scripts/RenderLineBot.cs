using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
public class RenderLineBot : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    Vector3 originalPosition;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    private LineRenderer line;
    private Vector3 botPos;
    public Material material;
    private int currLines = 0;
    Vector3 lastMouseCoordinate = Vector3.zero;
    
    string startingObject = null;
    List<string> MatchedObjects = new List<string>();
    public RandomContentGetter randomContentGetter;
    Vector3 startPos;
    List<GameObject> lines = new List<GameObject>();
    bool isLineCreated = true;
    int i = 0;
    public RectTransform rt;
    public GameObject dot;
    public List<GameObject> randomPoints;
    float currentTime = 0, endTime = 1;
    void Start()
    {
        originalPosition = transform.position;
    }
    void SpawnRandomDot(int n)
    {
        randomPoints.Clear();
        foreach(Transform child in rt.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < n; i++) 
        {
            Vector3 spawnPosition = GetBottomLeftCorner(rt) - new Vector3(Random.Range(0, rt.rect.x), Random.Range(0, rt.rect.y), 0);
            var newDot = Instantiate(dot, spawnPosition, Quaternion.identity,rt.transform);
            randomPoints.Add(newDot);
        }
    }
    Vector3 GetBottomLeftCorner(RectTransform rt)
    {
        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);
        return v[0];
    }
    
    IEnumerator StartLine(int j)
    {
        float nextLineWaitingTime = Random.Range(RConfigs.rr3,RConfigs.rr4);
        yield return new WaitForSeconds(nextLineWaitingTime);
        if(j == 4) SpawnRandomDot(0);
        else  SpawnRandomDot(Random.Range(0,2));
        transform.position = randomContentGetter.MatchedTilesPairs[j].transform.position;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if(i < 6  && RandomContentGetter.isAudioPlayed && currentTime >= 3.5f&& randomContentGetter.p1points[1].value != randomContentGetter.p1points[0].value + 3)//
        {
            if(RandomContentGetter.isAssetsLoaded )
            {
                if(isLineCreated)
                {
                    StartCoroutine(StartLine(i));
                    isLineCreated = false;
                }
            }
            //Move this gameobject to the center of corresponding spelling object to create a line
            if (transform.position == randomContentGetter.MatchedTilesPairs[i].transform.position) 
            {
                if(randomPoints.Count == 0)
                {
                    LeanTween.move(gameObject, randomContentGetter.MatchedTilesPairs[i+1].transform.position, Random.Range(RConfigs.rr1, RConfigs.rr2));
                }
                else if(randomPoints.Count == 1)
                {
                    LeanTween.move(gameObject, randomPoints[0].transform.position, Random.Range(RConfigs.rr1, RConfigs.rr2)).setEaseOutQuart().setOnComplete(() => 
                    {
                        LeanTween.move(gameObject, randomContentGetter.MatchedTilesPairs[i+1].transform.position, Random.Range(RConfigs.rr1, RConfigs.rr2));
                    });
                }
                else if(randomPoints.Count == 2)
                {
                    LeanTween.move(gameObject, randomPoints[0].transform.position, 1.5f).setEaseOutQuart().setOnComplete(() => 
                    {
                        LeanTween.move(gameObject, randomPoints[1].transform.position, 1.5f).setEaseOutQuart().setOnComplete(() =>
                        {
                            LeanTween.move(gameObject, randomContentGetter.MatchedTilesPairs[i+1].transform.position, 1.5f);
                        });
                    });                    
                }

                if (line == null)
                {
                    createLine();
                }
                //Getting world position of the image gameObject's right edge from the screen Position of the gameObject
                startPos = Camera.main.ScreenToWorldPoint(new Vector3(randomContentGetter.MatchedTilesPairs[i].transform.position.x + 75,randomContentGetter.MatchedTilesPairs[i].transform.position.y,0.0f));
                botPos = Camera.main.ScreenToWorldPoint(transform.position);
                botPos.z = 0;
                startPos.z = 0;
                line.SetPosition(0, startPos);
                line.SetPosition(1, botPos);
            }
            //Connecting the image and its spelling with a new line
            else if (transform.position == randomContentGetter.MatchedTilesPairs[i+1].transform.position && !isLineCreated)
            { 
                //Getting world position of spelling gameObject's left edge from the screen Position of the gameObject
                var endPosition = Camera.main.ScreenToWorldPoint(new Vector3 (randomContentGetter.MatchedTilesPairs[i+1].transform.position.x - 100,randomContentGetter.MatchedTilesPairs[i+1].transform.position.y,0.0f));
                endPosition.z = 0;
                line.SetPosition(1,endPosition);                                                                //Green:  R         G           B
                line.material.color = new Color(0,0.7924528f,0.04971686f);
                //RandomContentGetter.MatchedTilesPairs[i].transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0.098039f,0.6392156f, 0.2156862f);
                randomContentGetter.MatchedTilesPairs[i+1].transform.parent.transform.parent.GetComponentInParent<ProceduralImage>().color = new Color(0.098039f,0.6392156f, 0.2156862f);
                startingObject = null;
                line = null;
                randomContentGetter.p2points[1].value += 1;
                currLines++;
                isLineCreated = true;
                
                i += 2;
            }
            // move the end Point the line to the currect position
            else if (line)
            {
                var endPoint = Camera.main.ScreenToWorldPoint(transform.position);
                endPoint.z = 0;
                line.SetPosition(1, endPoint);
            }
        }
        else
        {
            transform.position = originalPosition;
        }
    } 
    void createLine()
    {
        line = new GameObject("BotLine" + currLines).AddComponent<LineRenderer>();
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
