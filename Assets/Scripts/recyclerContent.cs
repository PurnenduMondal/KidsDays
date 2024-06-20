using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class recyclerContent : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    AsyncOperationHandle<IList<Sprite>> operationHandle;
    public GameObject loadingObj,Canvas;
    public static List<Sprite> spriteList = new List<Sprite>();
    public Image profilePic;
    void Awake()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            profilePic.sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine("AssetLoader");
        
        _recyclableScrollRect.enabled = true;
        _recyclableScrollRect.DataSource = this;
    }

    public int GetItemCount()
    {
        return spriteList.Count;
    }
    public void SetCell(ICell cell, int index)
    {
        var item = cell as ImageCell;
        item.ConfigureCell(spriteList[index], index);
    }

    // private static void PutRecursively(Transform obj)  
    // {  
    //     //string tabs = new string('\t', Level);  
    //     Debug.Log(obj.name);   
    //     // store the children in the to do stack  
    //     if (obj.childCount == 0)  
    //         return;  
    //     for (int i = 0; i < obj.childCount; i++)  
    //         PutRecursively(obj.GetChild(i));  
    // } 

    IEnumerator AssetLoader()
    {
        string[] filesTofatch = null;

        filesTofatch = RandomContentGetter.Category_Icons_Names;
        
        
        loadingObj.gameObject.SetActive(true);
        operationHandle = Addressables.LoadAssetsAsync<Sprite>(filesTofatch[0],null);

        yield return operationHandle;
        if(operationHandle.Status == AsyncOperationStatus.Succeeded)
        operationHandle.Completed += OnLoadAssets;
        else
        Debug.Log("Failed to Fatch");
        //yield return null;
    }
    void OnLoadAssets (AsyncOperationHandle<IList<Sprite>> spriteListHandle)
    {
        loadingObj.gameObject.SetActive(false);
        List<int> rLocations = new List<int>();
        rLocations = RandomContentGetter.GenerateRandomList(0,spriteListHandle.Result.Count);
        for(int i = 0; i < spriteListHandle.Result.Count; i++)
        {
            spriteList.Add(spriteListHandle.Result[rLocations[i]]);
        }
        // Sprite tempSprite = null;
        
        // for(int i = 0; i < spriteListHandle.Result.Count; i++)
        // {
        //     tempSprite = spriteListHandle.Result[i];
        //     GameObject item = Instantiate(itemprefab, transform, false);
        //     item.transform.GetChild(0).GetComponent<Image>().sprite = tempSprite;
        //     item.transform.GetChild(0).gameObject.name = tempSprite.name;
        //     item.GetComponent<Button>().onClick.AddListener(GameObject.Find("SceneChanger").GetComponent<SceneChanger>().LoadMainView);
        //     item.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
        //     item.name = (i).ToString();
        //     item.GetComponent<Image>().color = new Color(0.8549f,0.89f,1,0);
        //     item.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
        //     StartCoroutine("FadeIn",item);
        // }
    }
    void OnDisable()
    {
        operationHandle.Completed -= OnLoadAssets;
    }
}
