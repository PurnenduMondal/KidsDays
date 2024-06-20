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

public class MainViewCell : MonoBehaviour,ICell
{
    public Image image;
    int _cellIndex;
    public int itemIndex;
    public static bool isFirstCell = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigureCell(Sprite s,int cellIndex)
    {
        _cellIndex = cellIndex;
        image.sprite = s;
        itemIndex = MainContentGetter.spriteList.IndexOf(s);
        transform.name = s.name;
        transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
        
    }
}
