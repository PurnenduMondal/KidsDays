using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class CountTilesHelper : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    public RandomContentGetter randomContentGetter;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    // Start is called before the first frame update
    void Start()
    {
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData.position = Input.mousePosition;
            m_Raycaster.Raycast(m_PointerEventData, results);
            
            if(results.Count >= 3)
            {
                if(results[2].gameObject.name.Contains("Bordered Image"))
                {
                    float childAlpha = results[2].gameObject.transform.GetChild(0).GetComponent<Image>().color.a;
                    if(childAlpha != 0.0f && !results[0].gameObject.name.Contains("_p2"))
                    {
                        float ObjectAlphaColor = results[2].gameObject.GetComponent<Image>().color.a;
                        if(ObjectAlphaColor == 0.0f)
                        {
                            results[2].gameObject.GetComponent<Image>().color = new Color(0.9686f,0.439215f,0,1);
                            randomContentGetter.R3audioSource.clip = Resources.Load<AudioClip>("SFX/Button");
                            randomContentGetter.R3audioSource.Play();
                        }
                        else
                        {
                            results[2].gameObject.GetComponent<Image>().color = new Color(0,0,0,0);  
                            randomContentGetter.R3audioSource.clip = Resources.Load<AudioClip>("SFX/Button");
                            randomContentGetter.R3audioSource.Play();            
                        }
                    }
                }
            }
        }
    }
}
