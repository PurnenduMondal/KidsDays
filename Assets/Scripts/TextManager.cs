using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI.Core;
using Febucci.UI;
//using DanielLochner.Assets.SimpleScrollSnap;

public class TextManager : MonoBehaviour
{
    private SimpleScrollSnap sss;
    GameObject textObject;
    GameObject formattedText;
    public TextMeshProUGUI tmPro,SpellButtonText;
    int currentPanelIndex;
    string currentPanelName = null, previousPanelName = null;
    GameObject scroll_Snap;
    bool once = false,isAnimPressed = false,isSpellOn = false;
    List<AudioClip> ac = new List<AudioClip>();
    public AudioSource audioSource;
    // Start is called before the first frame updates
    void Start()
    {
        isSpellOn = false;
        textObject = GameObject.FindWithTag("SpellingObj");
        tmPro = textObject.GetComponent<TextMeshProUGUI>();
        scroll_Snap = GameObject.FindWithTag("ScrollSnap");
        sss = scroll_Snap.GetComponent<SimpleScrollSnap>();
        ac = MainContentGetter.audioClipList;

    }


    // Update is called once per frame
    void Update()
    {

    }
    // IEnumerator WaitForTheScrollingToEnd()
    // {
    //     while (sss.thresholdSnappingSpeed == -1)
    //         yield return new WaitForSeconds(0.01f);
    //     audioSource.Play();
    // }
    // public void setColor()
    // {
    //     tmPro.color = new Color32(255, 0, 0, 255);
    // }
    // public void StartText()
    // {
    //     textAnimatorPlayer.ShowText(currentPanelName) ;
    // // }
    // public void StartAnimate()
    // {
    //     isAnimPressed = true;
    // }
    // public void StartingAnime()
    // {
    //     if (!isAnimPressed) return;
    //     if (currentPanelIndex == sss.NumberOfPanels - 1) isAnimPressed = false;
    //     else sss.GoToNextPanel();
    // }
}
