using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using TMPro;
public class HandleTextFile : MonoBehaviour
{
    float prvScrWidth = 0;
    float prvScrHeight = 0;
    public TextMeshProUGUI[] imageTexts;
    private static void PutRecursively(Transform obj, StreamWriter writer)  
    {  
        if(obj.name.Contains("Image") && !obj.name.Contains("Procedural"))
        {
            float xPos = 0, width = 0;
            float yPos = 0, height = 0;
            //float fontSize = 0;
            xPos = obj.position.x/Screen.width; width = obj.GetComponent<RectTransform>().rect.width/Screen.width;
            yPos = obj.position.y/Screen.height; height = obj.GetComponent<RectTransform>().rect.height/Screen.height;
            //fontSize = obj.GetComponent<TextMeshProUGUI>().fontSize / Screen.width;
            // if(obj.name.Contains("Procedural"))
            // writer.WriteLine
            // (
            //     "GameObject.Find(\""+obj.name+"\").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*"+width+"f, Screen.height*"+height+"f);\n"
            // );
            // else
            writer.WriteLine
            (
                //"GameObject.Find(\""+obj.name+"\").GetComponent<TextMeshProUGUI>().fontSize = Screen.width*"+fontSize+"f;"
                "GameObject.Find(\""+obj.name+"\").transform.position = new Vector3(Screen.width*"+xPos+"f,Screen.height*"+yPos+"f,0);"
                //"GameObject.Find(\""+obj.name+"\").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*"+width+"f, Screen.height*"+height+"f);\n"
            );
            xPos = yPos = width = height = 0;
        }
        // store the children in the to do stack  
        if (obj.childCount == 0)  
            return;  
        for (int i = 0; i < obj.childCount; i++)  
            PutRecursively(obj.GetChild(i), writer);  
    } 
    // Start is called before the first frame update
    void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        // string path = "Assets/Resources/Test.txt";
        // //Write some text to the test.txt file
        // StreamWriter writer = new StreamWriter(path, true);
        // PutRecursively(GameObject.Find("Canvas").transform, writer);
        // writer.Close();
        prvScrHeight = Screen.height;
        prvScrWidth = Screen.width;
        PositionAndRect();
        for(int i = 0; i < 5; i++)
        {
            imageTexts[i].fontSize = Screen.width*0.04722222f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(!(Screen.orientation == ScreenOrientation.Portrait))
        // {
        //     Screen.orientation = ScreenOrientation.Portrait;
        // }
        
        if(Screen.height != prvScrHeight && Screen.width != prvScrWidth)
        {
            PositionAndRect();
            for(int i = 0; i < 5; i++)
            {
                imageTexts[i].fontSize = Screen.width*0.04722222f;
            }
            prvScrHeight = Screen.height;
            prvScrWidth = Screen.width;
        }
    }

    public static void PositionAndRect()
    {
        GameObject.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 0").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.25f, Screen.height*0.125f);
        GameObject.Find("Image (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 1").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.25f, Screen.height*0.125f);
        GameObject.Find("Image (2)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 2").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.25f, Screen.height*0.125f);
        GameObject.Find("Image (3)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 3").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.25f, Screen.height*0.125f);
        GameObject.Find("Image (4)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 4").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.25f, Screen.height*0.125f);
        GameObject.Find("ImageText").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.3055556f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 5").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.125f);
        GameObject.Find("ImageText (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.3055556f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 6").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.125f);
        GameObject.Find("ImageText (2)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.3055556f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 7").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.125f);
        GameObject.Find("ImageText (3)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.3055556f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 8").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.125f);
        GameObject.Find("ImageText (4)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.3055556f, Screen.height*0.140625f);
        GameObject.Find("Procedural Image 9").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.2777778f, Screen.height*0.125f);
        GameObject.Find("Image").transform.position = new Vector3(Screen.width*0.1597222f,Screen.height*0.8984375f,0);
        GameObject.Find("Image (1)").transform.position = new Vector3(Screen.width*0.1597222f,Screen.height*0.7226563f,0);
        GameObject.Find("Image (2)").transform.position = new Vector3(Screen.width*0.1597222f,Screen.height*0.546875f,0);
        GameObject.Find("Image (3)").transform.position = new Vector3(Screen.width*0.1597222f,Screen.height*0.3710938f,0);
        GameObject.Find("Image (4)").transform.position = new Vector3(Screen.width*0.1597222f,Screen.height*0.1953125f,0);
        GameObject.Find("ImageText").transform.position = new Vector3(Screen.width*0.8263889f,Screen.height*0.8984375f,0);
        GameObject.Find("ImageText (1)").transform.position = new Vector3(Screen.width*0.8263889f,Screen.height*0.7226563f,0);
        GameObject.Find("ImageText (2)").transform.position = new Vector3(Screen.width*0.8263889f,Screen.height*0.546875f,0);
        GameObject.Find("ImageText (3)").transform.position = new Vector3(Screen.width*0.8263889f,Screen.height*0.3710938f,0);
        GameObject.Find("ImageText (4)").transform.position = new Vector3(Screen.width*0.8263889f,Screen.height*0.1953125f,0);

        GameObject.Find("ScoreWindow").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
        GameObject.Find("ScoreWindow").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*1f, Screen.height*1f);
        GameObject.Find("ProfilePic").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.703125f,0);
        GameObject.Find("ProfilePic").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height*0.15625f, Screen.height*0.15625f);
        GameObject.Find("Stars").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
        GameObject.Find("Stars").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*1f, Screen.height*0.140625f);
        GameObject.Find("s1").transform.position = new Vector3(Screen.width*0.1388889f,Screen.height*0.5f,0);
        GameObject.Find("s1").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("s2").transform.position = new Vector3(Screen.width*0.3194444f,Screen.height*0.5f,0);
        GameObject.Find("s2").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("s3").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
        GameObject.Find("s3").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("s4").transform.position = new Vector3(Screen.width*0.6805556f,Screen.height*0.5f,0);
        GameObject.Find("s4").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("s5").transform.position = new Vector3(Screen.width*0.8611111f,Screen.height*0.5f,0);
        GameObject.Find("s5").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("Stars (1)").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
        GameObject.Find("Stars (1)").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*1f, Screen.height*0.140625f);
        GameObject.Find("ss1").transform.position = new Vector3(Screen.width*0.1388889f,Screen.height*0.5f,0);
        GameObject.Find("ss1").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("ss2").transform.position = new Vector3(Screen.width*0.3194444f,Screen.height*0.5f,0);
        GameObject.Find("ss2").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("ss3").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
        GameObject.Find("ss3").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("ss4").transform.position = new Vector3(Screen.width*0.6805556f,Screen.height*0.5f,0);
        GameObject.Find("ss4").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("ss5").transform.position = new Vector3(Screen.width*0.8611111f,Screen.height*0.5f,0);
        GameObject.Find("ss5").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.1666667f, Screen.height*0.09375f);
        GameObject.Find("Play Again").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.25f,0);
        GameObject.Find("Play Again").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.7361111f, Screen.height*0.09375f);
        GameObject.Find("Back").transform.position = new Vector3(Screen.width*0.5f,Screen.height*0.1210938f,0);
        GameObject.Find("Back").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width*0.4444444f, Screen.height*0.09375f);
        GameObject.Find("(TMP)0").GetComponent<TextMeshProUGUI>().fontSize = Screen.width*0.0833333333333333f;
        GameObject.Find("(TMP)1").GetComponent<TextMeshProUGUI>().fontSize = Screen.width*0.0833333333333333f;
    }
}
