using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;
public class PlayerCell : MonoBehaviour,ICell
{
    public Image image;
    public int _cellIndex;
    public void ConfigureCell(Sprite s,int cellIndex)
    {
        _cellIndex = cellIndex;
        image.sprite = s;
        transform.name = s.name;
    }
}
