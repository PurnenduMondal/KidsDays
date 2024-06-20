using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerAnime : MonoBehaviour
{
    public GameObject playerName,playerPic,winnerText,playAgainButton,backButton,starTrophy,winnerBackground;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        gameObject.LeanScale(new Vector3(1,1,1),0.5f).setOnComplete(() => {
            playerPic.SetActive(true);
            playerPic.LeanScale(new Vector3(1.2f,1.2f,1), 1);
            playerPic.LeanMoveLocal(new Vector3(0,165,0),1).setOnComplete(() =>{
                winnerText.SetActive(true);
                winnerText.LeanScale(new Vector3(2,2,1),0.5f).setOnComplete(() => {
                    starTrophy.SetActive(true);
                    starTrophy.LeanScale(new Vector3(1,1,1),0.5f);
                    winnerBackground.SetActive(true);
                    winnerBackground.LeanScale(new Vector3(1.2f,1.2f,1),0.5f);
                    playerName.SetActive(true);
                    playerName.LeanScale(new Vector3(1.5f,1.5f,1),0.5f);
                });
                playAgainButton.SetActive(true);
                backButton.SetActive(true);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
