using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class PointerMover : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    // Start is called before the first frame update
    PointerEventData m_PointerEventData, m_PointerEventDataOr;
    DragAndDrop dragAndDrop;
    DragAndDropCell dropCell;
    string isDragBegin = "init";
    float speed;
    void Start()
    {
        speed = 50 * Time.deltaTime;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(0, speed, 0);
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventDataOr = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = transform.position;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (var result in results)
        {
            Debug.Log(result.gameObject.name);
            if(result.gameObject.name.Equals("Mg"))
            {
                m_PointerEventDataOr.position = result.gameObject.transform.position;
                result.gameObject.GetComponent<DragAndDrop>().OnBeginDrag(m_PointerEventDataOr);
                isDragBegin = "Start";
                dragAndDrop = result.gameObject.GetComponent<DragAndDrop>();
                speed = -50 * Time.deltaTime;
            }
            if(result.gameObject.name.Equals("Blue"))
            {
                dropCell = result.gameObject.GetComponent<DragAndDropCell>();
                
                isDragBegin = "Stop";
            }
            
        }
        if(isDragBegin.Equals("Start"))
        {
            dragAndDrop.OnDrag(m_PointerEventDataOr);
        }
        else if(isDragBegin.Equals("Stop"))
        {
            dropCell.OnDrop(m_PointerEventDataOr);
            Image myImage = dragAndDrop.gameObject.GetComponent<Image>();
		    myImage.color = new Color(1f, 1f, 1f, 1f);
            dragAndDrop.OnEndDrag(m_PointerEventDataOr);
            isDragBegin = "init";
        }
    }
}
