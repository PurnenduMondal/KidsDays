using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Android;
using UnityEngine.EventSystems;
public class ProfileScores : MonoBehaviour
{
    public TextMeshProUGUI quizScore,MatchingScore,WordScore,MultiScore;
    public TMP_InputField profileName;
    public Image profilePic;
    public static bool startCamera = false;
    // GraphicRaycaster m_Raycaster;
    // EventSystem m_EventSystem;
    // PointerEventData m_PointerEventData;
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    // Start is called before the first frame update
    void Start()
    {
        // m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        // m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if(SaveLoad.SaveExists("Quiz"))
            quizScore.text =( SaveLoad.Load<int>("Quiz")).ToString();
        if(SaveLoad.SaveExists("Matching"))
            MatchingScore.text =( SaveLoad.Load<int>("Matching")).ToString();
        if(SaveLoad.SaveExists("WordSearch"))
            WordScore.text =( SaveLoad.Load<int>("WordSearch")).ToString();
        if(SaveLoad.SaveExists("MultiPlayer"))
            MultiScore.text =( SaveLoad.Load<int>("MultiPlayer")).ToString();
        if(File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            profilePic.sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
        }
        if(File.Exists(Application.persistentDataPath + "/saves/profileName.txt"))
        {
            profileName.text = SaveLoad.Load<string>("profileName");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // m_PointerEventData = new PointerEventData(m_EventSystem);
        // //Create a list of Raycast Results
        // List<RaycastResult> results = new List<RaycastResult>();
        
                
        //     if (Input.GetMouseButtonDown(0)){                
        //         m_PointerEventData.position = Input.mousePosition;
        //         m_Raycaster.Raycast(m_PointerEventData, results);
        //     if(results.Count > 0){
        //         foreach(var r in results)
        //         {
        //             Debug.Log(r.gameObject.name);
        //         }
        //     }
        // }
    }
    public void OpenCameraScene()
    {
        startCamera = true;
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        else
        {
            SceneManager.LoadScene("PictureTaking");
        }
        
    }
    void OnApplicationFocus(bool focus)
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera) && startCamera)
        {
            startCamera = false;
            SceneManager.LoadScene("PictureTaking");
        }
    }
    public void OnProfileNameChanged()
    {
        SaveLoad.Save<string>(profileName.text,"profileName");
    }

}
