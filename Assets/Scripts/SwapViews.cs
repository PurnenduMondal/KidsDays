using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using DanielLochner.Assets.SimpleScrollSnap;

public class SwapViews : MonoBehaviour
{
    GameObject scroll_Snap, Spellit,PlayAnime,ViewAllButtonText,Test,SpellingObj,ScrollView;
    UnityEngine.UI.Text ButtonTextComponent;
    SimpleScrollSnap sssComponent;
    int currentPanelindex;
    bool isPressing = false, longPress = false; 
    public bool isClicked = false;
    float totalDownTime = 0;
    const float clickDuration = 0.75f;
    
   
    void Start()
    {
        ScrollView = GameObject.FindWithTag("ScrollView");
        ScrollView.SetActive(false);
        scroll_Snap = GameObject.FindWithTag("ScrollSnap");
        
        Spellit = GameObject.FindWithTag("Spellit");
        PlayAnime = GameObject.FindWithTag("PlayAnime");
        ViewAllButtonText = GameObject.FindWithTag("ViewAllButtonText");
        ButtonTextComponent = ViewAllButtonText.GetComponentInChildren<UnityEngine.UI.Text>();
        Test = GameObject.FindWithTag("Test");
        SpellingObj = GameObject.FindWithTag("SpellingObj");
        sssComponent = scroll_Snap.GetComponent<SimpleScrollSnap>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isPressing)
        {
            totalDownTime += Time.deltaTime;
        
            if(totalDownTime >= clickDuration)
            {
                longPress = true;
                
                Debug.Log(": Long Clicked");
            }
        }
        
    }
    public void OnPointerDown(GameObject obj)
    {
        totalDownTime = 0;
        isPressing = true;
        longPress = false;
        
    }
    public void OnPointerUp()
    {
        
        
        isPressing = false;
    }


    public void ChangeView(GameObject gameObject)
    {
        
        switch(gameObject.name) 
        {
            case "View All":
                currentPanelindex = scroll_Snap.GetComponent<SimpleScrollSnap>().CurrentPanel;
                scroll_Snap.SetActive(false);
                Spellit.SetActive(false);
                PlayAnime.SetActive(false);
                Test.SetActive(false);
                SpellingObj.SetActive(false);
                gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = "Back";
                gameObject.name = "Back";
                ScrollView.SetActive(true);
                isClicked = true;
              
                break;
            default:
                
                if(gameObject.name != "Back" && !longPress)
                {
                    scroll_Snap.GetComponent<SimpleScrollSnap>().GoToPanel(int.Parse(gameObject.transform.GetChild(0).name));  
                    ScrollView.SetActive(false);
                    scroll_Snap.SetActive(true);
                    Spellit.SetActive(true);
                    PlayAnime.SetActive(true);
                    Test.SetActive(true);
                    SpellingObj.SetActive(true);
                    longPress = false;
                    ButtonTextComponent.text = "View All";
                    ViewAllButtonText.name = "View All";
                }
                else if(gameObject.name == "Back")
                {
                    scroll_Snap.GetComponent<SimpleScrollSnap>().GoToPanel(currentPanelindex);
                    ScrollView.SetActive(false);
                    scroll_Snap.SetActive(true);
                    Spellit.SetActive(true);
                    PlayAnime.SetActive(true);
                    Test.SetActive(true);
                    SpellingObj.SetActive(true);
                    ButtonTextComponent.text = "View All";
                    ViewAllButtonText.name = "View All";
                }
                isClicked = false;

                break;
        }

    }

}
