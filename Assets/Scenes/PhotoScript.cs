using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
public class PhotoScript : MonoBehaviour
{
    public GameObject cameraView,captureButton,infoText,deletePic;
    WebCamTexture webCamTexture;
    bool isCameraStarted = false;
    public static bool isFirstTime = false;
    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCameraStarted)
        {
            if(File.Exists(Application.persistentDataPath + "/saves/photo.png"))
            {
                deletePic.gameObject.SetActive(true);
            }
            else
            {
                deletePic.gameObject.SetActive(false);
            }
            //Debug.Log(Screen.height); 720
            cameraView.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height, Screen.height);
            GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            captureButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height*0.2083333333333333f, Screen.height*0.2083333333333333f);
            deletePic.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height*0.1666666666666667f, Screen.height*0.1666666666666667f);
            infoText.GetComponent<TextMeshProUGUI>().fontSize = Screen.height*0.05f;
            WebCamDevice[] devices = WebCamTexture.devices;
            for(int i = 0; i < devices.Length; i++)
            {
                if(devices[i].isFrontFacing)
                    webCamTexture = new WebCamTexture(devices[i].name);
            }
            //webCamTexture = new WebCamTexture(devices[0].name);
            webCamTexture.requestedFPS = 1f;
            webCamTexture.requestedWidth = Screen.height;
            webCamTexture.requestedHeight = Screen.width;
            //GetComponent<Image>().sprite.texture = webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
            webCamTexture.Play();
            isCameraStarted = true;
        }
        if(webCamTexture.isPlaying)
        GetComponent<RawImage>().texture = webCamTexture;
        //GetComponent<RawImage>().SizeToParent();
    }
    public void CapturePic()
    {
        StartCoroutine("OnButtonPressed");
    }
    IEnumerator OnButtonPressed()
    {
        yield return StartCoroutine("TakePhoto");
        SceneManager.LoadScene("Profile");
    }
    public void OnDeleteButtonClick()
    {
        if(File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            File.Delete(Application.persistentDataPath + "/saves/photo.png");
            SceneManager.LoadScene("Profile");
        }
        else
        {
            SceneManager.LoadScene("Profile");
        }
    }
    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

     yield return new WaitForEndOfFrame(); 
        float aspectRatio = (float) Screen.height/Screen.width;
        float picWidth = 190*(aspectRatio);
        
        SaveLoad.Save<float>(picWidth,"PicWidth");
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        Texture2D modifiedTexture =  rotateTexture(B83.TextureTools.TextureTools.ResampleAndCrop(photo, Screen.width, Screen.width),false);
        
        //Encode to a PNG
        byte[] bytes = modifiedTexture.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes(Application.persistentDataPath + "/saves/" + "photo.png", bytes);
    }
    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
     {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}

