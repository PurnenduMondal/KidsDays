using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class WordSearch : MonoBehaviour
{
    public List<string> fileNames = new List<string>();//{"CRAB", "DOLPHINE", "ORCA", "OCTOPUS", "GOLDFISH", "SEAHORSE", "SEAL", "SHARK", "SQUID", "STINGRAY", "TUNA", "WHALE" };
    bool first = false,second = false, isWordInserted = false,is3rdWordInserted = false,is4thWordInserted = false,third = false,fourth = false,fiveth = false,sixth = false;
    public int startingRow0 = 0, startingCol0 = 0, startingRow1 = 0, startingCol1 = 0,startingRow3 = 0, startingCol3 = 0,startingRow2 = 0, startingCol2 = 0;
    public static bool isAlphaGridLoaded = false;
    int  maxRow = 6, maxColumn = 7;
    List<int> occupiedRows = new List<int>(), occupiedCols = new List<int>();
    public bool is2ndVertical = false, isRolledBack = false;
    public AudioClip audioClip;
    public AudioSource audioSource;
    string smallWord;
    public GameObject[] image,imageText,imageBg,imageTextBg;
    public GameObject alphaGrid;
    public bool revealAnswer = false;
    public static bool isFirstPlay = true;
    public int endingRow2 = 0, endingCol2 = 0,endingRow3 = 0,endingCol3 = 0;
    public Image profilePic;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        //folderName = MainContentGetter.filesTofatch[0];
        int k = 0;
            for(int i = 0; i < maxRow; i++)
            {
                for(int j = 0; j < maxColumn ; j++)
                {
                    alphaGrid.transform.GetChild(k).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    alphaGrid.transform.GetChild(k).gameObject.name = (i).ToString()+j.ToString();
                    k++;
                }
            }
                
        float timer = 0;        
        while( fileNames.Count != 4)
        {
            timer += Time.deltaTime;
            string randStr = MainContentGetter.spriteList[UnityEngine.Random.Range(0, MainContentGetter.spriteList.Count)].name;
            if(!fileNames.Contains(randStr) && randStr.Length < maxColumn)
            {
                fileNames.Add(randStr.ToUpper());
            }
        }
        List<string> fileNames2 = new List<string>();
        for(int i = 0; i < 4; i++)
        {
            for(int j = i+1; j < 4; j++)
            {
                for(int l = 0; l < fileNames[j].Length; l++)
                {
                    if(fileNames[i].Contains(fileNames[j][l].ToString()) && fileNames2.Count != 4)
                    {
                        if(!fileNames2.Contains(fileNames[i]))
                        fileNames2.Add(fileNames[i]);
                        if(!fileNames2.Contains(fileNames[j]))
                        fileNames2.Add(fileNames[j]);
                    }
                }
            }
        }
        if(fileNames2.Count != 4)
        {
            for(int i = 0; i < 4; i++)
            {
                if(!fileNames2.Contains(fileNames[i]))
                fileNames2.Add(fileNames[i]);
            }
        }
        fileNames = fileNames2;
        // int m = 0;
        // //put the longest string into fileNames[1] 
        // for(int j = 0; j < fileNames.Count; j++)
        // {
        //     if(fileNames[j].Length > fileNames[m].Length)
        //     {
        //         m = j;
        //     }
        // }
        // if(m != 1)
        // {
        //     string temp = null;
        //     temp = fileNames[1];
        //     fileNames[1] = fileNames[m];
        //     fileNames[m] = temp;
        // }
        // //put the 2nd Longest string into fileNames[3]
        // m = 0;
        // for (int i = 0; i < fileNames.Count; i++)
        // {
        //     if(fileNames[i].Length > fileNames[m].Length && i != 1) 
        //     {
        //         m = i;
        //     }
        // }
        // if(m != 3)
        // {
        //     string temp1 = null;
        //     temp1 = fileNames[3];
        //     fileNames[3] = fileNames[m];
        //     fileNames[m] = temp1;            
        // }
        foreach(var s in fileNames)
        yield return LoadAssets();
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(first)
        {
            startingRow0 = Random.Range(0,3);
            startingCol0 = Random.Range(0, maxColumn - fileNames[0].Length);
            for(int i = 0; i < fileNames[0].Length; i++)
            {
                GameObject.Find((startingRow0).ToString()+(startingCol0+i).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[0][i].ToString();
            }
            occupiedRows.Add(startingRow0);
            first = false;
            second = true;
            return;
        }
        if(second)
        {
            for(int i = 0; i < fileNames[1].Length; i++)
            {
                for(int j = 0; j < fileNames[0].Length; j++)
                {
                    //find a intersection character between two words
                    if(fileNames[1][i] == fileNames[0][j] && !isWordInserted)
                    {
                        startingRow1 = startingRow0 - i;
                        startingCol1 = startingCol0 + j;
                        int k = startingRow1;
                        int q = 0;
                        if(startingRow1 + fileNames[1].Length -1 <= maxRow - 1 && startingRow0 - i >= 0)
                        {
                            while (q < fileNames[1].Length)
                            {
                                GameObject.Find((k).ToString()+(startingCol1).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[1][q].ToString();
                                k++;
                                q++;
                            }
                            is2ndVertical = true;
                            occupiedCols.Add(startingCol1);
                            isWordInserted = true;
                        }
                    }
                }
            }
            if(!isWordInserted)
            {
                do
                {
                    startingRow1 = Random.Range(0,maxRow);
                }while(occupiedRows.Contains(startingRow1));

                startingCol1 = Random.Range(0, maxColumn - fileNames[1].Length +1 );
                for(int j = 0; j < fileNames[1].Length; j++)
                {
                    GameObject.Find((startingRow1).ToString()+(startingCol1+j).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[1][j].ToString();
                }
                occupiedRows.Add(startingRow1);
                isWordInserted = true;
                is2ndVertical = false;
            }
            second = false;
            third = true;
        }
        if(third)
        {
            for(int i = 0; i < fileNames[2].Length; i++)
            {
                for(int j = 0; j < fileNames[0].Length; j++)
                {
                    if(fileNames[2][i] == fileNames[0][j] && !is3rdWordInserted)
                    {
                        isRolledBack = false;
                        startingRow2 = startingRow0 - i;
                        startingCol2 = startingCol0 + j;
                        int r = startingRow2;
                        int q = 0;
                        if(startingRow2 + fileNames[2].Length - 1 < maxRow && startingRow0 - i >= 0 && !occupiedCols.Contains(startingCol2))
                        {
                            while (q < fileNames[2].Length)
                            {   
                                TextMeshProUGUI cell = GameObject.Find((r).ToString()+(startingCol2).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                                if(cell.text != "" && cell.text != fileNames[2][q].ToString())
                                {
                                    if(q != 0)
                                    {
                                        int p = r - 1;
                                        while(p >= startingRow2)
                                        {
                                            if(p != startingRow1)
                                            GameObject.Find((p).ToString()+(startingCol2).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = "";
                                            p--;
                                            
                                        }    
                                    }
                                    isRolledBack = true;
                                }
                                if(!isRolledBack)
                                cell.text = fileNames[2][q].ToString();
                                r++;
                                q++;
                                if(q == fileNames[2].Length && !isRolledBack) 
                                {
                                    endingRow2 = startingRow2 + fileNames[2].Length - 1;
                                    endingCol2 = startingCol2;
                                    occupiedCols.Add(startingCol2);
                                    is3rdWordInserted = true;
                                }
                            }
                        }
                    }
                }
            }
            if(!is2ndVertical)
            {
                for(int i = 0; i < fileNames[2].Length; i++)
                {
                    for(int j = 0; j < fileNames[1].Length; j++)
                    {
                        isRolledBack = false;
                        if(fileNames[2][i] == fileNames[1][j] && !is3rdWordInserted)
                        {
                            startingRow2 = startingRow1 - i;
                            startingCol2 = startingCol1 + j;
                            int r = startingRow2;
                            
                            int q = 0;
                            if(startingRow2 + fileNames[2].Length - 1 < maxRow && startingRow1 - i >= 0)
                            {

                                while (q < fileNames[2].Length)
                                {
                                    TextMeshProUGUI cell = GameObject.Find((r).ToString()+(startingCol2).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                                    
                                    if(cell.text != "" && cell.text != fileNames[2][q].ToString())
                                    {
                                        if(q != 0)
                                        {
                                            int p = r - 1;
                                            while(p >= startingRow2)
                                            {
                                                if(p != startingRow1)
                                                GameObject.Find((p).ToString()+(startingCol2).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = "";
                                                p--;
                                                
                                            }
                                        }
                                        isRolledBack = true;
                                    }

                                    if(!isRolledBack)
                                    cell.text = fileNames[2][q].ToString();                                    
                                    r++;
                                    q++;
                                    if(q == fileNames[2].Length && !isRolledBack)
                                    {
                                        endingRow2 = startingRow2 + fileNames[2].Length - 1;
                                        endingCol2 = startingCol2;
                                        occupiedCols.Add(startingCol2);
                                        is3rdWordInserted = true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            if(is2ndVertical)
            {
                for(int i = 0; i < fileNames[2].Length; i++)
                {
                    for(int j = 0; j < fileNames[1].Length; j++)
                    {
                        if(fileNames[2][i] == fileNames[1][j] && !is3rdWordInserted)
                        {
                            startingCol2 = startingCol1 - i;
                            startingRow2 = startingRow1 + j;
                            int c = startingCol2;
                            int q = 0;
                            if(startingCol2 + fileNames[2].Length - 1 <= maxColumn - 1 && startingCol1 - i >= 0 && !occupiedRows.Contains(startingRow2))
                            {

                                while (q < fileNames[2].Length)
                                {
                                    GameObject.Find((startingRow2).ToString()+(c).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[2][q].ToString();
                                    c++;
                                    q++;
                                    if(q == fileNames[2].Length) 
                                    {
                                        endingRow2 = startingRow2;
                                        endingCol2 = startingCol2 + fileNames[2].Length - 1;
                                        occupiedRows.Add(startingRow2);
                                        is3rdWordInserted = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(!is3rdWordInserted)
            {
                List<int> alphaTable = new List<int>();
                for(int i = 0; i< maxRow; i++)
                {
                    for(int j = 0; j <= maxColumn-fileNames[2].Length; j++)
                    {
                        bool isSafeToAdd = true;
                        for(int k = j; k < j + fileNames[2].Length ;k++)
                        {
                            if(GameObject.Find(i.ToString()+k.ToString()).GetComponentInChildren<TextMeshProUGUI>().text != "")
                            {
                                isSafeToAdd = false;
                            }
                        }
                        if(GameObject.Find(i.ToString()+j.ToString()).GetComponentInChildren<TextMeshProUGUI>().text == "" && isSafeToAdd)
                            alphaTable.Add((i*10)+j);
                    }
                }
                try
                {
                    var randCellIndix = alphaTable[Random.Range(0, alphaTable.Count)];
                    startingRow2 = randCellIndix / 10;
                    startingCol2 = randCellIndix % 10;
                }
                catch
                {
                    SceneManager.LoadScene("Words");
                }
                for(int j = 0; j < fileNames[2].Length; j++)
                {
                    try
                    {
                        var cell = GameObject.Find((startingRow2).ToString()+(startingCol2+j).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                        if(cell.text == "")
                            GameObject.Find((startingRow2).ToString()+(startingCol2+j).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[2][j].ToString();
                    }
                    catch
                    {
                        SceneManager.LoadScene("Words");
                    }
                }
                occupiedRows.Add(startingRow2);
                endingRow2 = startingRow2;
                endingCol2 = startingCol2 + fileNames[2].Length - 1;
                is3rdWordInserted = true;
            }
            third = false;
            fourth = true;
        }
        if(fourth)
        {
            bool is4thWordVertical = false;
            if(!is4thWordInserted)
            {
                
                List<int> alphaTable = new List<int>();
                for(int i = 0; i< maxRow; i++)
                {
                    for(int j = 0; j <= maxColumn-fileNames[3].Length; j++)
                    {
                        bool isSafeToAdd = true;
                        for(int k = j; k < j + fileNames[3].Length ;k++)
                        {
                            if(GameObject.Find(i.ToString()+k.ToString()).GetComponentInChildren<TextMeshProUGUI>().text != "")
                            {
                                isSafeToAdd = false;
                            }
                        }
                        if(GameObject.Find(i.ToString()+j.ToString()).GetComponentInChildren<TextMeshProUGUI>().text == "" && isSafeToAdd)
                        {
                            alphaTable.Add((i*10)+j);
                            is4thWordVertical = false;
                        }
                            
                    }
                }
                if(alphaTable.Count == 0)
                {
                    for(int i = 0; i< maxColumn; i++)
                    {
                        for(int j = 0; j <= maxRow-fileNames[3].Length; j++)
                        {
                            bool isSafeToAdd = true;
                            for(int k = j; k < j + fileNames[3].Length ;k++)
                            {
                                if(GameObject.Find(k.ToString()+i.ToString()).GetComponentInChildren<TextMeshProUGUI>().text != "")
                                {
                                    isSafeToAdd = false;
                                }
                            }
                            if(GameObject.Find(j.ToString()+i.ToString()).GetComponentInChildren<TextMeshProUGUI>().text == "" && isSafeToAdd)
                            {
                                alphaTable.Add((j*10)+i);
                                is4thWordVertical = true;
                            }   
                        }
                    }
                }
                try
                {
                    var randCellIndix = alphaTable[Random.Range(0, alphaTable.Count)];
                    startingRow3 = randCellIndix / 10;
                    startingCol3 = randCellIndix % 10;
                }
                catch
                {
                    SceneManager.LoadScene("Words");
                }
                if(is4thWordVertical)
                {
                    for(int j = 0; j < fileNames[3].Length; j++)
                    {
                        var cell = GameObject.Find((startingRow3+j).ToString()+(startingCol3).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                        if(cell.text == "")
                            GameObject.Find((startingRow3+j).ToString()+(startingCol3).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[3][j].ToString();
                    }
                    endingRow3 = startingRow3 + fileNames[3].Length - 1;
                    endingCol3 = startingCol3;
                }
                else
                {
                    for(int j = 0; j < fileNames[3].Length; j++)
                    {
                        var cell = GameObject.Find((startingRow3).ToString()+(startingCol3+j).ToString()).GetComponentInChildren<TextMeshProUGUI>();
                        if(cell.text == "")
                            GameObject.Find((startingRow3).ToString()+(startingCol3+j).ToString()).GetComponentInChildren<TextMeshProUGUI>().text = fileNames[3][j].ToString();
                    }
                    endingRow3 = startingRow3;
                    endingCol3 = startingCol3 + fileNames[3].Length - 1;
                }
                is4thWordInserted = true;
            }
            else
            {
                SceneManager.LoadScene("Words");
            }
            fourth = false;
            fiveth = true;
        }
        if(fiveth)
        {
            List<string> allAlphaList = new List<string>(){"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
            for(int i = 0; i < 4; i ++)
            {
                for(int j = 0; j < fileNames[i].Length; j++)
                {
                    string temp = fileNames[i][j].ToString();
                    if(allAlphaList.Contains(temp))
                    {
                        allAlphaList.Remove(temp);
                    }
                }
            }
            for(int i = 0; i < maxRow; i++)
            {
                for(int j = 0; j < maxColumn ; j++)
                {
                    if(GameObject.Find(i.ToString()+j.ToString()).GetComponentInChildren<TextMeshProUGUI>().text == "")
                    {
                        GameObject.Find(i.ToString()+j.ToString()).GetComponentInChildren<TextMeshProUGUI>().text = allAlphaList[Random.Range(0,allAlphaList.Count)];
                    }
                    
                }
            }
            if(isFirstPlay)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                isFirstPlay = false;
            }

            fiveth = false;
            isAlphaGridLoaded = true;
        }
    }
    IEnumerator LoadAssets()
    {
        for(int i = 0; i < 4; i++)
        {
            //string filePath = "Assets/Resources_moved/Images/Aquatic_Animals/"+fileNames[i]+".png";
            Sprite sprite = null;
            GameObject tempI = image[i];
            GameObject tempT = imageText[i];
            foreach(var s in MainContentGetter.spriteList)
            {
                if(s.name == fileNames[i])
                    sprite = s;
            }
            //Addressables.LoadAssets<Sprite>(filePath, sprite =>
            //{
                tempI.GetComponent<Image>().sprite = sprite;
                tempI.GetComponent<Image>().preserveAspect = true;
                tempT.GetComponent<TextMeshProUGUI>().text = sprite.name;
                tempT.transform.parent.parent.name = sprite.name;
            //});
        }

        yield return null;
    }
}
