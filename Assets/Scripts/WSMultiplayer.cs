using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text.RegularExpressions;
using UnityEngine.AddressableAssets;
public class WSMultiplayer : MonoBehaviour
{
    public List<string> objectNames = new List<string>();
    bool first = false,second = false, isWordInserted = false,is3rdWordInserted = false,third = false,fourth = false,fiveth = false;
    public int startingRow0 = 0, startingCol0 = 0, startingRow1 = 0, startingCol1 = 0,startingRow2 = 0, startingCol2 = 0;
    public int endingRow0 = 0, endingCol0 = 0;
    public static bool isAlphaGridLoaded = false, isAudioPlayed = false;
    List<int> occupiedRows = new List<int>(), occupiedCols = new List<int>();
    public bool is2ndWordVertical = false, isRolledBack = false;
    public GameObject alphaGrid,wordContainer;
    string commonString = null;
    public AudioSource R4audioSource;
    public AudioClip R4audioClips;
    // Start is called before the first frame update
    void Start()
    {
        isAudioPlayed = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        //yield return LoadAssets();
        //yield return new WaitForSeconds(1.5f);
        if(alphaGrid.name.Contains("_p2"))
        {
            commonString = "_p2";
        }
        else
        {
            commonString = "";
        }
        //folderName = MainContentGetter.filesTofatch[0];
        while( objectNames.Count != 3)
        {
            string randStr = MainContentGetter.spriteList[UnityEngine.Random.Range(0, MainContentGetter.spriteList.Count)].name;//RandomContentGetter.Aquatic_Animals[UnityEngine.Random.Range(0, RandomContentGetter.Aquatic_Animals.Length)];
            if(!objectNames.Contains(randStr) && randStr.Length < 9)
            {
                objectNames.Add(randStr);
            }
        }
        wordContainer.GetComponent<TextMeshProUGUI>().text = "";
        for(int i = 0; i < objectNames.Count; i++)
            wordContainer.GetComponent<TextMeshProUGUI>().text = wordContainer.GetComponent<TextMeshProUGUI>().text+objectNames[i]+" ";
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(first)
        { 
            for(int i = 0; i < alphaGrid.transform.childCount; i++)
            {
                alphaGrid.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            startingRow1 = Random.Range(0,5);
            startingCol1 = Random.Range(0, 8 - objectNames[1].Length);
            for(int i = 0; i < objectNames[1].Length; i++)
            {
                GameObject.Find((startingRow1).ToString()+(startingCol1+i).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = objectNames[1][i].ToString();
            }
            occupiedRows.Add(startingRow1);
            first = false;
            second = true;
            return;
        }
        if(second)
        {
            // for(int i = 0; i < objectNames[2].Length; i++)
            // {
            //     for(int j = 0; j < objectNames[1].Length; j++)
            //     {
            //         //find a intersection/common character between two words
            //         if(objectNames[1][j] == objectNames[2][i] && !isWordInserted)
            //         {
            //             startingRow2 = startingRow1 - i;
            //             startingCol2 = startingCol1 + j;
                        
            //             int k = startingRow2;
            //             int q = 0;
            //             if(startingRow2 + objectNames[2].Length -1 < 5 && startingRow1 - i >= 0)
            //             {
            //                 while (q < objectNames[2].Length)
            //                 {
            //                     GameObject.Find((k).ToString()+(startingCol2).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = objectNames[2][q].ToString();
            //                     k++;
            //                     q++;
            //                 }
            //                 is2ndWordVertical = true;
            //                 occupiedCols.Add(startingCol2);
            //                 isWordInserted = true;
            //             }
            //         }
            //     }
            // }
            //if no Common Alphabet found between objectNames[1] and objectNames[2]
            if(!isWordInserted)
            {
                do
                {
                    startingRow2 = Random.Range(0,5);
                }while(occupiedRows.Contains(startingRow2));

                startingCol2 = Random.Range(0, 8 - objectNames[2].Length +1 );
                for(int j = 0; j < objectNames[2].Length; j++)
                {
                    GameObject.Find((startingRow2).ToString()+(startingCol2+j).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = objectNames[2][j].ToString();
                }
                occupiedRows.Add(startingRow2);
                isWordInserted = true;
                is2ndWordVertical = false;
            }
            second = false;
            third = true;
        }
        if(third)
        {
            // for(int i = 0; i < objectNames[0].Length; i++)
            // {
            //     for(int j = 0; j < objectNames[1].Length; j++)
            //     {
            //         if(objectNames[1][j] == objectNames[0][i] && !is3rdWordInserted)
            //         {
            //             isRolledBack = false;
            //             startingRow0 = startingRow1 - i;
            //             startingCol0 = startingCol1 + j;
                        
            //             int r = startingRow0;
            //             int q = 0;
            //             if(startingRow0 + objectNames[0].Length - 1 < 5 && startingRow1 - i >= 0 && !occupiedCols.Contains(startingCol0))
            //             {
            //                 while (q < objectNames[0].Length)
            //                 {   
            //                     TextMeshProUGUI cell = GameObject.Find((r).ToString()+(startingCol0).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>();
                            
            //                     if(cell.text != "" && cell.text != objectNames[0][q].ToString())
            //                     {
            //                         //current cell contains a letter from objectNames[2] So undo all changes(RollBack)
            //                         if(q != 0)
            //                         {
            //                             int p = r - 1;
            //                             while(p >= startingRow0)
            //                             {
            //                                 if(p != startingRow2)
            //                                 GameObject.Find((p).ToString()+(startingCol0).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = "";
            //                                 p--;
            //                             }
            //                         }
            //                         isRolledBack = true;
            //                     }
            //                     if(!isRolledBack)
            //                     {
            //                         cell.text = objectNames[0][q].ToString();
            //                     }
                                    
            //                     r++;
            //                     q++;
            //                     if(q == objectNames[0].Length && !isRolledBack) 
            //                     {
            //                         occupiedCols.Add(startingCol0);
            //                         endingRow0 = startingRow0 + objectNames[0].Length - 1;
            //                         endingCol0 = startingCol0; 
            //                         is3rdWordInserted = true;
            //                     }
            //                 }

            //             }
            //         }
            //     }
            // }
            // if(!is2ndWordVertical)
            // {
            //     for(int i = 0; i < objectNames[0].Length; i++)
            //     {
            //         for(int j = 0; j < objectNames[2].Length; j++)
            //         {
            //             //if objectNames[2] is horizontally placed and there is a common character between objectNames[0] and objectNames[2]
            //             //then objectNames[0] will be placed vertically, colomn index will be fixed
            //             isRolledBack = false;
            //             if(objectNames[2][j] == objectNames[0][i] && !is3rdWordInserted)
            //             {
            //                 startingRow0 = startingRow2 - i;
            //                 startingCol0 = startingCol2 + j;
            //                 int r = startingRow0;
                            
            //                 int q = 0;
            //                 if(startingRow0 + objectNames[0].Length - 1 < 5 && startingRow2 - i >= 0)
            //                 {

            //                     while (q < objectNames[0].Length)
            //                     {
            //                         TextMeshProUGUI cell = GameObject.Find((r).ToString()+(startingCol0).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>();
                                    
            //                         if(  cell.text != "" && cell.text != objectNames[0][q].ToString())
            //                         {
            //                             if(q != 0 )
            //                             {
            //                                 int p = r - 1;
            //                                 while(p >= startingRow0)
            //                                 {
            //                                     if(p != startingRow2)
            //                                     GameObject.Find((p).ToString()+(startingCol0).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = "";
            //                                     p--;
            //                                 }  
            //                             }
            //                             isRolledBack = true;
            //                         }

            //                         if(!isRolledBack)
            //                         cell.text = objectNames[0][q].ToString();                                    
            //                         r++;
            //                         q++;
            //                         if(q == objectNames[0].Length && !isRolledBack)
            //                         {
            //                             occupiedCols.Add(startingCol0);
            //                             endingRow0 = startingRow0 + objectNames[0].Length - 1;
            //                             endingCol0 = startingCol0;
            //                             is3rdWordInserted = true;
            //                         }

            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            // if(is2ndWordVertical)
            // {
            //     for(int i = 0; i < objectNames[0].Length; i++)
            //     {
            //         for(int j = 0; j < objectNames[2].Length; j++)
            //         {
            //             if(objectNames[2][j] == objectNames[0][i] && !is3rdWordInserted)
            //             {
            //                 startingCol0 = startingCol2 - i;
            //                 startingRow0 = startingRow2 + j;
            //                 int c = startingCol0;
            //                 int q = 0;
            //                 if(startingCol2 + objectNames[0].Length - i -1 <= 7 && startingCol2 - i >= 0 && !occupiedRows.Contains(startingRow0))
            //                 {

            //                     while (q < objectNames[0].Length)
            //                     {
            //                         GameObject.Find((startingRow0).ToString()+(c).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = objectNames[0][q].ToString();
            //                         c++;
            //                         q++;
            //                         if(q == objectNames[0].Length) 
            //                         {
            //                             occupiedRows.Add(startingRow0);
            //                             endingRow0 = startingRow0;
            //                             endingCol0 = startingCol0 + objectNames[0].Length - 1;
            //                             is3rdWordInserted = true;
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            if(!is3rdWordInserted)
            {
                List<int> alphaTable = new List<int>();
                for(int i = 0; i< 5; i++)
                {
                    for(int j = 0; j <= 8-objectNames[0].Length; j++)
                    {
                        bool isSafeToAdd = true;
                        for(int k = j; k < j + objectNames[0].Length ;k++)
                        {
                            if(GameObject.Find(i.ToString()+k.ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text != "")
                            {
                                isSafeToAdd = false;
                            }
                        }
                        if(GameObject.Find(i.ToString()+j.ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text == "" && isSafeToAdd)
                            alphaTable.Add((i*10)+j);
                    }
                }
                var randCellIndix = alphaTable[Random.Range(0, alphaTable.Count)];
                startingRow0 = randCellIndix / 10;
                startingCol0 = randCellIndix % 10;

                for(int j = 0; j < objectNames[0].Length; j++)
                {
                    var cell = GameObject.Find((startingRow0).ToString()+(startingCol0+j).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>();
                    if(cell.text == "")
                        GameObject.Find((startingRow0).ToString()+(startingCol0+j).ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = objectNames[0][j].ToString();
                }

                occupiedRows.Add(startingRow0);
                endingRow0 = startingRow0;
                endingCol0 = startingCol0 + objectNames[0].Length - 1;
                is3rdWordInserted = true;
            }
            third = false;
            fourth = true;
        }
        if(fourth)
        {
            List<string> allAlphaList = new List<string>(){"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
            //remove the characters that are present in objectNames for simplicity
            for(int i = 0; i < 3; i ++)
            {
                for(int j = 0; j < objectNames[i].Length; j++)
                {
                    string temp = objectNames[i][j].ToString();
                    if(allAlphaList.Contains(temp))
                    {
                        allAlphaList.Remove(temp);
                    }
                }
            }
            //put ramdom charecters in empty cells
            for(int i = 0; i< 5; i++)
            {
                for(int j = 0; j<8 ; j++)
                {
                    if(GameObject.Find(i.ToString()+j.ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text == "")
                    {
                        GameObject.Find(i.ToString()+j.ToString()+commonString).GetComponentInChildren<TextMeshProUGUI>().text = allAlphaList[Random.Range(0,allAlphaList.Count)];
                    }
                }
            }

            fourth = false;
            fiveth = true;
            isAlphaGridLoaded = true;
        }
        if(fiveth)
        {
            if(gameObject.name == "SpellFinder")
            StartCoroutine(PlayAudio());
            fiveth = false;
        }
    }
    IEnumerator PlayAudio()
    {
        LeanTween.scale(transform.GetChild(0).gameObject, Vector3.one,.5f);
        R4audioSource.clip = R4audioClips;
        R4audioSource.Play();
        yield return new WaitForSeconds(R4audioSource.clip.length);
        GameObject.Find("R4Img").LeanScale(new Vector3(0,1,1), 1f).setEaseInCubic().setOnComplete(() =>
        {
            isAudioPlayed = true;
        });
    }
    

    // AsyncOperationHandle<IList<Object>> spriteOperationHandle;
    // IEnumerator LoadAssets()
    // {  

    //     spriteOperationHandle = Addressables.LoadAssetsAsync<Object>("Birds", null);
    //     yield return spriteOperationHandle;
    //     //if(spriteOperationHandle.Status == AsyncOperationStatus.Succeeded)
    //     spriteOperationHandle.Completed += OnLoadAssets;
    //     //else
    //     //Debug.Log("Failed to Fatch");
    // }
    // void OnLoadAssets (AsyncOperationHandle<IList<Object>> spriteListHandle)
    // {
        
    //     for(int i = 0; i < spriteListHandle.Result.Count; i++)
    //     {
    //         if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.Texture2D")
    //         {
    //             Texture2D texture = (Texture2D) spriteListHandle.Result[i];
    //             Sprite s = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
    //             //s.name = Regex.Replace(texture.name, @"\d", "");
    //             s.name = texture.name;
    //             if(!MainContentGetter.spriteList.Contains(s))
    //             {
    //                 MainContentGetter.spriteList.Add(s);
    //             }
    //         }
    //         if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.AudioClip")
    //         {
    //             AudioClip audio = (AudioClip) spriteListHandle.Result[i];
    //             //audio.name = Regex.Replace(audio.name, @"\d", "");

    //             if(!MainContentGetter.audioClipList.Contains(audio))
    //             {
    //                 MainContentGetter.audioClipList.Add(audio);
    //             }
    //         }
    //     }
    // }
}
