// using System.Collections;
// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.UI; 
// using UnityEngine.EventSystems;
// using TMPro;
// using UnityEngine.UI.ProceduralImage;

// public class R3SpellLine : MonoBehaviour
// {
//     public RandomContentGetter randomContentGetter;
//     // Start is called before the first frame update
//     List<GameObject> raycastList = new List<GameObject>();
//     GraphicRaycaster m_Raycaster;
//     EventSystem m_EventSystem;
//     PointerEventData m_PointerEventData;
//     private LineRenderer line;
//     private Vector3 mousePos;
//     public Material material;
//     private int currLines = 0;
//     Vector3 lastMouseCoordinate = Vector3.zero;
//     string startingObject = null;
//     List<string> MatchedObjects = new List<string>();
//     Vector3 startPos;
//     List<GameObject> lines = new List<GameObject>();
//     int i = 0;
//     void Start()
//     {
//         m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
//         m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
//     }
 
//     void Update()
//     {
//         if(gameObject.name == "AlphaGrid")
//         {
//             //Set up the new Pointer Event
//             m_PointerEventData = new PointerEventData(m_EventSystem);

//             //Create a list of Raycast Results
//             List<RaycastResult> results = new List<RaycastResult>();
//             if (Input.GetMouseButtonDown(0))
//             {
//                 m_PointerEventData.position = Input.mousePosition;
//                 //Raycast using the Graphics Raycaster and mouse click position
//                 m_Raycaster.Raycast(m_PointerEventData, results);
//                 // if(results.Count > 0 )
//                 // if(results[0].gameObject.name.Contains(randomContentGetter.r3FileName1[0].ToString()) && !results[1].gameObject.transform.parent.gameObject.name.Contains("_p2"))
//                 // {
//                 //     if (line == null)
//                 //     {
//                 //         startingObject = results[0].gameObject.name;
//                 //         createLine();
//                 //         startPos = Camera.main.ScreenToWorldPoint(results[0].gameObject.transform.position);
//                 //     }
//                 //     mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                 //     mousePos.z = 0;
//                 //     startPos.z = 0f;
//                 //     line.SetPosition(0, startPos);
//                 //     line.SetPosition(1, mousePos);
//                 // }

//             }
//             else if (Input.GetMouseButtonUp(0) && line )
//             {
//                 m_PointerEventData.position = Input.mousePosition;
//                 //Raycast using the Graphics Raycaster and mouse click position
//                 m_Raycaster.Raycast(m_PointerEventData, results);
//                 if(results.Count > 0)
//                 {

//                     //A new line is born
//                     // if(results[0].gameObject.name == (randomContentGetter.r3FileName1[randomContentGetter.r3FileName1.Length - 1].ToString()) && currLines == 0 && randomContentGetter.r3FileName1.Length == raycastList.Count)
//                     // {
//                     //     //Getting world position of spelling gameObject's left edge from the screen Position of the gameObject
//                     //     var endPosition = Camera.main.ScreenToWorldPoint(results[0].gameObject.transform.position);
//                     //     endPosition.z = 0;
//                     //     line.SetPosition(1,endPosition);
//                     //     //randomContentGetter.pointerBarData[2].value += 1;
//                     //     startingObject = null;
//                     //     line = null;
//                     //     currLines++;
//                     // }
//                     // else
//                     // {
//                     //     startingObject = null;
//                     //     if(GameObject.Find("Line" + currLines) != null)
//                     //     Destroy(GameObject.Find("Line" + currLines).gameObject);
//                     // }
//                 }
//                 else
//                 {
//                     startingObject = null;
//                     //SleepforSeconds(results[0].gameObject);
//                     //results[0].gameObject.GetComponentInParent<Image>().color = new Color(1f, 0f, 0f, 1f);
//                     if(GameObject.Find("Line" + currLines) != null)
//                     Destroy(GameObject.Find("Line" + currLines).gameObject);
//                 }
//             }
//             if (Input.GetMouseButton(0) )//&& line
//             {
//                 m_PointerEventData.position = Input.mousePosition;
//                 //Raycast using the Graphics Raycaster and mouse click position
//                 m_Raycaster.Raycast(m_PointerEventData, results);
                
//                 if(results.Count > 0 && i < RandomContentGetter.r3FileName1.Length)
//                 if(RandomContentGetter.r3FileName1[i].ToString().Equals(results[0].gameObject.name))
//                 {
//                     results[1].gameObject.GetComponent<ProceduralImage>().color = new Color(0.235294117647f,0.69f,0.262745f,1);
//                     if(i != 0 && i != RandomContentGetter.r3FileName1.Length -1)
//                         results[1].gameObject.GetComponent<OnlyOneEdgeModifier>().Radius = 0;
//                     if(i == RandomContentGetter.r3FileName1.Length -1)
//                         results[1].gameObject.GetComponent<OnlyOneEdgeModifier>().Side = OnlyOneEdgeModifier.ProceduralImageEdge.Right;
//                     if(randomContentGetter.p2points[2].value != randomContentGetter.p2points[1].value + RandomContentGetter.r3FileName1.Length)
//                     {
//                         randomContentGetter.p1points[2].value += 1;
//                     }
//                     raycastList.Insert(i,results[0].gameObject);
//                     results[0].gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1);
//                     results[0].gameObject.GetComponent<TextMeshProUGUI>().fontSize = 50;
//                     i++;
//                 }

//                 // mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                 // mousePos.z = 0;
//                 // line.SetPosition(1, mousePos);
//             }
//         }  
//     } 
//     void createLine()
//     {
//         line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
//         line.material = material;
//         line.positionCount = 2;
//         line.startWidth = 0.2f;
//         line.endWidth = 0.2f;
//         line.useWorldSpace = false;
//         line.numCapVertices = 90;
//         line.numCornerVertices = 90;
//         line.startWidth = 0.8f;
//         line.endWidth = 0.8f;
//         lines.Add(line.gameObject);
//     }
//     void OnDisable()
//     {
//         foreach(GameObject go in lines)
//         {
//             Destroy(go);
//         }
//     }
// }
