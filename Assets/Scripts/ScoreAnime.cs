using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnime : MonoBehaviour
{
    public RectTransform pointIdicators,finalPointsAnime,finalPoints,part1,part2,part3;
    public Slider pointBar0,pointBar1,pointBar2;
    public HorizontalLayoutGroup pointIdicatorsHorizontalLayout;
    bool first = true;
    public bool isAnimeStopped = false;
    bool second = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    float RoundUp(double n)
    {
    double left = System.Math.Floor(n);
    double right = n - left;

    if(right >= 0.1 && right < 0.5)
        {
            left = left + 0.5;
        }
    else if(right >= 0.5 && right < 0.9999)
        {
            left = left + 1.0; 
        }
        return  (float) left;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointIdicatorsHorizontalLayout.spacing >= -80)
        {
            pointIdicatorsHorizontalLayout.spacing -= 60*Time.deltaTime;
        }

        if(first && pointIdicatorsHorizontalLayout.spacing < -80)
        {
            pointIdicators.gameObject.SetActive(false);
            finalPointsAnime.gameObject.SetActive(true);
            //Change the size and position
            part1.sizeDelta = new Vector2(RoundUp( (double) (pointBar0.value/pointBar0.maxValue)* 130),part1.rect.height);
            part2.sizeDelta = new Vector2(RoundUp( (double) (pointBar1.value/pointBar1.maxValue)* 130),part2.rect.height);
            part3.sizeDelta = new Vector2(RoundUp( (double) (pointBar2.value/pointBar2.maxValue)* 130),part3.rect.height);
            part1.anchoredPosition  = new Vector3(part1.rect.width/2, -15, 0);
            part2.anchoredPosition  = new Vector3(130+part2.rect.width/2, -15, 0);
            part3.anchoredPosition  = new Vector3(260+part3.rect.width/2, -15, 0);
            //Debug.Log(part2.position);
            //part2.gameObject.transform.(part1.rect.width+part2.rect.width/2,2f);
            // LeanTween.move(part2.gameObject,new Vector3(part1.rect.width+part2.rect.width/2, -15, 0),2).setOnComplete(() => 
            // {
            //     LeanTween.moveX(part3.gameObject ,part1.rect.width+part2.rect.width+part3.rect.width/2, 2);
            // });
            first = false;
        }
        //place part1, part2 and part3 right next to each other
        if(!first && part2.anchoredPosition.x > part1.rect.width+part2.rect.width/2)
        {
            Vector3 newPos = new Vector3(part2.anchoredPosition.x - 100*Time.deltaTime, -15,0); 
            part2.anchoredPosition = newPos;
        }
        else if(!first && part2.anchoredPosition.x <= part1.rect.width+part2.rect.width/2)
        {
            second = true;
        }
        if(second && part3.anchoredPosition.x > part1.rect.width+part2.rect.width+part3.rect.width/2)
        {
            Vector3 newPos = new Vector3(part3.anchoredPosition.x - 100*Time.deltaTime, -15,0); 
            part3.anchoredPosition = newPos;
        }
        // else if (second && part3.anchoredPosition.x < part1.rect.width+part2.rect.width+part3.rect.width/2)
        // {
        //     finalPointsAnime.gameObject.SetActive(false);
        //     finalPoints.gameObject.GetComponent<Slider>().maxValue = pointBar0.maxValue + pointBar1.maxValue + pointBar2.maxValue;
        //     finalPoints.gameObject.GetComponent<Slider>().value = pointBar0.value + pointBar1.value + pointBar2.value;
        //     finalPoints.gameObject.SetActive(true);
        //     isAnimeStopped = true;
        // }

    }
}
