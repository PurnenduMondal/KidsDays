using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class TapTilesCounter : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;
    public static int selectCount;
    public RandomContentGetter randomContentGetter;
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
            //Let the player1 tap the tiles
            if(results.Count == 3 && RandomContentGetter.isAudioPlayed)
            {
                if(results[2].gameObject.name.Contains("Image ("))
                {
                    if(RandomContentGetter.tilesToTap.Contains(results[2].gameObject.transform.GetChild(0).transform.GetChild(0).name))
                    {
                        if(randomContentGetter.p2points[0].value != RandomContentGetter.TapTilesMaxImage)
                        {
                            results[2].gameObject.GetComponent<Image>().color = new Color(0.0f,1f,0f,1f);
                            randomContentGetter.p1points[0].value += 1;
                            RandomContentGetter.tilesToTap.Remove(results[2].gameObject.transform.GetChild(0).transform.GetChild(0).name);
                            randomContentGetter.R1audioSource.clip = Resources.Load<AudioClip>("SFX/Success3");
                            randomContentGetter.R1audioSource.Play();
                        }
                    }
                    else
                    {
                        randomContentGetter.R1audioSource.clip = Resources.Load<AudioClip>("SFX/Failure");
                        randomContentGetter.R1audioSource.Play();
                    }
                }
            }
        }
    }
}
