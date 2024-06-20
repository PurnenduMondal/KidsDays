using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text.RegularExpressions;
using UnityEngine.Analytics;
public class GameController : MonoBehaviour
{
    public float p1FinalScore = 0, p2FinalScore = 0;
    
    public Image p1Pic,p2Pic;
    public TextMeshProUGUI p1Name, p2Name;
    public WinnerAnime winnerAnime;
    List<Slider> p1score = new List<Slider>(4),p2score = new List<Slider>(4);
    public GameObject playerPickerGo,round1,round2,round3,round4;
    public TextMeshProUGUI finalScores1,finalScores2;
    AnalyticsResult analyticsResult;
    public PlayerPicker playerPicker;
    bool isR1Completed = false,isR2Completed = false, isR3Completed = false,isR4Completed = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!SaveLoad.SaveExists("MultiPlayer"))
        {
            SaveLoad.Save<int>(0, "MultiPlayer");
        }
        p1score.Add(GameObject.Find("TapTilePoints").GetComponent<Slider>());
        p1score.Add(GameObject.Find("MatchTilePoints").GetComponent<Slider>());
        p1score.Add(GameObject.Find("CountTilePoints").GetComponent<Slider>());
        p1score.Add(GameObject.Find("SpellFinderPoints").GetComponent<Slider>());

        p2score.Add(GameObject.Find("TapTilePoints_p2").GetComponent<Slider>());
        p2score.Add(GameObject.Find("MatchTilePoints_p2").GetComponent<Slider>());
        p2score.Add(GameObject.Find("CountTilePoints_p2").GetComponent<Slider>());
        p2score.Add(GameObject.Find("SpellFinderPoints_p2").GetComponent<Slider>());
        //round1.SetActive(true);
        //yield return LoadAssets();
        //yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPicker.isPlayersReady)
        {
            p1Pic.sprite = playerPicker.p1Pic.GetComponent<Image>().sprite;  
            p1Name.text = playerPicker.p1Name.GetComponent<TextMeshProUGUI>().text;
            p2Pic.sprite = playerPicker.finalP2Pic;  
            p2Name.text = playerPicker.p2Name.GetComponent<TextMeshProUGUI>().text;
            playerPickerGo.SetActive(false);
            round1.SetActive(true);
            playerPicker.isPlayersReady = false;
        }
        if(RandomContentGetter.TapTilesMaxImage != 0)
        {
            if(!isR1Completed && (p1score[0].value == RandomContentGetter.TapTilesMaxImage || p2score[0].value == RandomContentGetter.TapTilesMaxImage))
            {
                p1score[1].value = p1score[0].value;
                p2score[1].value = p2score[0].value;
                StartCoroutine(startRound2());
                isR1Completed = true;
            }
            if(!isR2Completed && (p1score[1].value == p1score[0].value + 3 || p2score[1].value == p2score[0].value + 3))
            {
                isR2Completed = true;
                p1score[2].value = p1score[1].value;
                p2score[2].value = p2score[1].value;
                StartCoroutine(startRound3());
            }
            if(!isR3Completed && (p1score[2].value == p1score[1].value + 1 || p2score[2].value == p2score[1].value + 1))
            {
                p1score[3].value = p1score[2].value;
                p2score[3].value = p2score[2].value;
                StartCoroutine(startRound4());
                isR3Completed = true;
            }
            if(!isR4Completed && (p1score[3].value == p1score[2].value + 3 || p2score[3].value == p2score[2].value + 3))
            {
                p1FinalScore = p1score[3].value;
                p2FinalScore = p2score[3].value;
                StartCoroutine(OnRound4Ended());
                isR4Completed = true;
            }
        }
    }
    IEnumerator OnRound4Ended()
    {
        yield return new WaitForSeconds(1f);
        finalScores1.text = p1FinalScore+"/10";
        finalScores2.text = p2FinalScore+"/10";
        finalScores1.gameObject.SetActive(true);
        finalScores2.gameObject.SetActive(true);
        if(p1FinalScore > p2FinalScore && !winnerAnime.enabled)
        {
            RandomContentGetter.userData.Add("IsWinner","Yes");
            round4.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("SFX/Victory");
            round4.GetComponent<AudioSource>().Play();
            finalScores1.color = Color.green;
            finalScores2.color = Color.red;
            winnerAnime.playerPic = winnerAnime.transform.GetChild(4).gameObject;
            winnerAnime.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = p1Name.text;
            winnerAnime.playerName = winnerAnime.transform.GetChild(5).gameObject;
            winnerAnime.playerPic.GetComponent<Button>().onClick.AddListener(()=>{
                SceneManager.LoadScene("Profile");
            });
            
            SaveLoad.Save<int>(SaveLoad.Load<int>("MultiPlayer")+1,"MultiPlayer");
            winnerAnime.playerPic.GetComponent<Image>().sprite = p1Pic.sprite;
            winnerAnime.enabled = true;
        }
        else if(p1FinalScore < p2FinalScore && !winnerAnime.enabled)
        {
            round4.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("SFX/GameOver");
            round4.GetComponent<AudioSource>().Play();
            RandomContentGetter.userData.Add("IsWinner","No");
            finalScores1.color = Color.red;
            finalScores2.color = Color.green;
            winnerAnime.playerPic = winnerAnime.transform.GetChild(6).gameObject;
            winnerAnime.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = p2Name.text;
            winnerAnime.playerName = winnerAnime.transform.GetChild(5).gameObject;
            winnerAnime.playerPic.GetComponent<Image>().sprite = p2Pic.sprite;
            winnerAnime.enabled = true;
        }
        else
        {
            Debug.Log("Its a Draw");
        }
        analyticsResult = Analytics.CustomEvent(SceneChanger.currentCategory,RandomContentGetter.userData);
        Debug.Log(analyticsResult);
    }
    IEnumerator startRound2()
    {
        yield return new WaitForSeconds(2f);
        round1.SetActive(false);
        round2.SetActive(true);
    }
    IEnumerator startRound3()
    {
        yield return new WaitForSeconds(2f);
        round2.SetActive(false);
        round3.SetActive(true);
    }
    IEnumerator startRound4()
    {
        yield return new WaitForSeconds(2f);
        round3.SetActive(false);
        round4.SetActive(true);
    }
// AsyncOperationHandle<IList<Object>> spriteOperationHandle;
//     IEnumerator LoadAssets()
//     {  
//         foreach (var sArray in RandomContentGetter.Category_Icons)
//         {
            
//             if(sArray.Contains("Numbers"))
//             {
//                 MainContentGetter.filesTofatch = sArray;
//                 MainContentGetter.spriteList.Clear();
//                 MainContentGetter.audioClipList.Clear();
//             }
//         }
//         spriteOperationHandle = Addressables.LoadAssetsAsync<Object>("Numbers", null);
//         yield return spriteOperationHandle;
//         if(spriteOperationHandle.Status == AsyncOperationStatus.Succeeded)
//         spriteOperationHandle.Completed += OnLoadAssets;
//         else
//         Debug.Log("Failed to Fatch");
//     }
//     void OnLoadAssets (AsyncOperationHandle<IList<Object>> spriteListHandle)
//     {
        
//         for(int i = 0; i < spriteListHandle.Result.Count; i++)
//         {
//             bool isSpriteExists = false, isAudioExists = false;
//             if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.Texture2D")
//             {
//                 Texture2D texture = (Texture2D) spriteListHandle.Result[i];
//                 Sprite s = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
//                 //s.name = Regex.Replace(texture.name, @"\d", "");
//                 s.name = texture.name;
//                 if(i != 0)
//                 for (int j = 0; j < MainContentGetter.spriteList.Count; j++)
//                 {
//                     if(MainContentGetter.spriteList[j].name == s.name) isSpriteExists = true;
//                 }
//                 if(!isSpriteExists)
//                 {
//                     MainContentGetter.spriteList.Add(s);
//                 }
//             }
//             if(spriteListHandle.Result[i].GetType().ToString() == "UnityEngine.AudioClip")
//             {
//                 AudioClip audio = (AudioClip) spriteListHandle.Result[i];
//                 //audio.name = Regex.Replace(audio.name, @"\d", "");
//                 if(i != 0)
//                 for (int j = 0; j < MainContentGetter.audioClipList.Count; j++)
//                 {
//                     if(MainContentGetter.audioClipList[j].name == audio.name) isAudioExists = true;
//                 }
//                 if(!isAudioExists)
//                 {
//                     MainContentGetter.audioClipList.Add(audio);
//                 }
//             }
//         }
//         //if(SceneChanger.objectName == "Numbers")
//         //{
//             MainContentGetter.spriteList = MainContentGetter.spriteList.OrderBy(sprite => Regex.Replace(sprite.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
//             MainContentGetter.audioClipList = MainContentGetter.audioClipList.OrderBy(clip => Regex.Replace(clip.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
//             foreach(var s in MainContentGetter.spriteList)
//                 s.name = Regex.Replace(s.name, @"\d", "");
//             foreach(var a in MainContentGetter.audioClipList)
//                 a.name = Regex.Replace(a.name, @"\d", "");
//         //}
//     }
}
