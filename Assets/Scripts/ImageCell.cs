using UnityEngine;
using PolyAndCode.UI;
using UnityEngine.UI;

public class ImageCell : MonoBehaviour,ICell
{
    public Image image;
    private int _cellIndex;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GameObject.Find("SceneChanger").GetComponent<SceneChanger>().LoadMainView);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigureCell(Sprite s,int cellIndex)
    {
        _cellIndex = cellIndex;
        //GetComponent<RectTransform>().sizeDelta = new Vector2(350,350);
        //GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(350,350);
        image.sprite = s;
        transform.GetChild(0).name = s.name;
    }
}
