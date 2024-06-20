using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using PolyAndCode.UI;
using TMPro;
using System;
public class PlayerPicker : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    public GameObject p1Pic,p2Pic,p1Name,p2Name,p1,p2,CategoryImage;
    public Sprite finalP2Pic;
    AsyncOperationHandle<IList<Sprite>> operationHandle;
    public SimpleScrollSnap simpleScrollSnap;
    public GameObject scrollContent;
    public List<Sprite> playerList = new List<Sprite>();
    public bool isPlayersReady = false;
    public List<int> nxtIndxRange;
    int currentPlayerCellIndex = -1;
    bool isPlayerSelected = false;
    List<int> rangeIndex = new List<int>(2);
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(!SaveLoad.SaveExists("RangeAndPindex"))
        {

            SaveLoad.Save<List<int>>(new List<int>{10,0},"RangeAndPindex");
        }
        foreach(var s in recyclerContent.spriteList)
        {
            if(s.name == SceneChanger.currentCategory)
            {
               CategoryImage.GetComponent<Image>().sprite = s;
            }
        }
        yield return StartCoroutine("AssetLoader");
        if(System.IO.File.Exists(Application.persistentDataPath + "/saves/photo.png"))
        {
            p1Pic.GetComponent<Image>().sprite = SaveLoad.LoadSprite(Application.persistentDataPath + "/saves/photo.png");
        }
        if(System.IO.File.Exists(Application.persistentDataPath + "/saves/profileName.txt"))
        {
            p1Name.GetComponent<TextMeshProUGUI>().text = SaveLoad.Load<string>("profileName");
        }

        
    }
    float currentTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= 2)
        {
            nxtIndxRange = SaveLoad.Load<List<int>>("RangeAndPindex");
            currentPlayerCellIndex = scrollContent.transform.GetChild(simpleScrollSnap.DetermineNearestPanel()).GetComponent<PlayerCell>()._cellIndex;
            //Debug.Log(currentPlayerCellIndex);
            if(currentPlayerCellIndex == nxtIndxRange[1]%10)
            {
                p2.transform.GetChild(0).GetComponent<ScrollRect>().StopMovement();
                p2Name.GetComponent<TextMeshProUGUI>().text = scrollContent.transform.GetChild(simpleScrollSnap.DetermineNearestPanel()).name.Split(new String[] {"seed"}, StringSplitOptions.None)[0];
                if( nxtIndxRange[1] == 59 &&   nxtIndxRange[0] == 60 && !isPlayerSelected)
                {
                    nxtIndxRange[1] = 0;
                    nxtIndxRange[0] = 10;
                    SaveLoad.Save<List<int>>(nxtIndxRange,"RangeAndPindex");
                }
                else if(( nxtIndxRange[1]) <  nxtIndxRange[0] - 1 && !isPlayerSelected)
                {
                    nxtIndxRange[1]++;
                    SaveLoad.Save<List<int>>(nxtIndxRange,"RangeAndPindex");
                }
                else if(( nxtIndxRange[1]) ==  nxtIndxRange[0] - 1 && !isPlayerSelected)
                {
                    nxtIndxRange[1]++; 
                    nxtIndxRange[0] += 10;
                    SaveLoad.Save<List<int>>(nxtIndxRange,"RangeAndPindex");
                } 
                isPlayerSelected = true;
            }   
        }
        if(currentTime >= 4)
        {
            finalP2Pic = GameObject.Find(simpleScrollSnap.Content.transform.GetChild(simpleScrollSnap.DetermineNearestPanel()).name).GetComponent<Image>().sprite;
            isPlayersReady = true;
        }
    }

    public int GetItemCount()
    {
        return 10000;
    }
    public void SetCell(ICell cell, int index)
    {
        var item = cell as PlayerCell;
        item.ConfigureCell(playerList[index % 10], index % 10);
    }
    IEnumerator AssetLoader()
    {
        //loadingObj.gameObject.SetActive(true);
        operationHandle = Addressables.LoadAssetsAsync<Sprite>("Players",null);

        yield return operationHandle;
        if(operationHandle.Status == AsyncOperationStatus.Succeeded)
            operationHandle.Completed += OnLoadAssets;
        else
            Debug.Log("Failed to Fatch");
    }
    void OnLoadAssets (AsyncOperationHandle<IList<Sprite>> spriteListHandle)
    {
        //loadingObj.gameObject.SetActive(false);
        for(int i = ( SaveLoad.Load<List<int>>("RangeAndPindex")[0]) - 10; i <  SaveLoad.Load<List<int>>("RangeAndPindex")[0]; i++)
        {
            playerList.Add(spriteListHandle.Result[i]);
        }
        //playerList = GenerateRandomList(playerList);
        _recyclableScrollRect.enabled = true;
        simpleScrollSnap.enabled = true;
        _recyclableScrollRect.DataSource = this;
        LeanTween.moveLocal(p1,new Vector3(-500,0,0),1).setEaseInOutBounce().setEaseOutBack();
        LeanTween.moveLocal(p2,new Vector3(500,0,0),1).setEaseInOutBounce().setEaseOutBack();
        p2.transform.GetChild(0).GetComponent<SimpleScrollSnap>().AddVelocity(UnityEngine.Random.Range(2500, 10000) * Vector2.up);
    }
    public static List <Sprite> GenerateRandomList(List<Sprite> inputList)
    {
        List <Sprite> uniqueNumbers = new List<Sprite> ();
        List <Sprite> NumbersList = new List<Sprite> ();
        uniqueNumbers = inputList;
        for(int i = 0; i< inputList.Count; i++)
        {
            Sprite ranName = uniqueNumbers[UnityEngine.Random.Range(0,uniqueNumbers.Count)];
            NumbersList.Add(ranName);
            uniqueNumbers.Remove(ranName);
        }
        return NumbersList; 
    }
}
