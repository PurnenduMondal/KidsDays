using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GraphicRecaster : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    GameObject m_PointerObj;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        //Fetch the pointer object from the Canvas
        m_PointerObj = GameObject.Find("PointerObj");
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = m_PointerObj.transform.position;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (var result in results)
            {
                if(!result.gameObject.name.Equals("PointerObj"))
                {
                    Debug.Log("Hit " + result.gameObject.name);
                }
                
            }
        //}
    }
}
