using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Text;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text.RegularExpressions;
public class RandomContentGetter : MonoBehaviour
{
    public static string[] Aquatic_Animals = {"Aquatic_Animals","CLOWNFISH", "CRAB", "DOLPHINE", "GOLDFISH", "OCTOPUS", "ORCA", "SEA TURTLE", "SEAHORSE", "SEAL", "SHARK", "SQUID", "STINGRAY", "TUNA", "WHALE" };
    public static string[] Berries_and_Nuts = {"Berries_and_Nuts","ACORN", "ALMOND", "BLACKBERRY", "BLUEBERRY", "CASHEW", "CHERRY", "GOOSEBERRY", "HAZELNUTS", "LYCHEE", "PEANUT", "PISTACHIO", "PLUM", "RASPBERRY", "STRAWBERRY", "WALNUT", };
    public static string[] Birds = {"Birds","CHICKEN", "CRANE", "CROW", "DUCK", "EAGLE", "FLAMINGO", "KINGFISHER", "MACAW", "OSTRICH", "OWL", "PARROT", "PEACOCK", "PENGUIN", "PIGEON", "SEAGULL", "SPARROW", "SWAN", "TURKEY", "VULTURE ", "WOODPECKER" };
    public static string[] Clothes = {"Clothes","BAGPACK", "BELT", "BLAZER", "CAP", "DRESS", "FLIPFLOPS", "GLASSES", "HAT", "JACKET", "JEANS", "NECKLACE", "RING", "SHOE", "SOCKS", "SUIT", "SWEATER", "T-Shirt", "TIE", "TUNIC" };
    //string[] Colors = {};
    public static string[] Domestic_Animals = {"Domestic_Animals","BULL", "CAMEL", "CAT", "COW", "DOG", "GOAT", "HORSE", "MOUSE", "PIG", "SHEEP" };
    public static string[] Flowers = {"Flowers","ASTER", "DAFFODIL", "DAISY", "DANDELION", "HIBISCUS", "JASMINE", "LOTUS", "MARIGOLD", "ORCHID", "POPPY", "ROSE", "SNOWDROP", "SUNFLOWER", "TULIPS" };
    public static string[] Fruits = {"Fruits","APPLE", "AVOCADO", "BANANA", "COCONUT", "GRAPES", "GUAVA", "KIWI", "LEMON", "MANGO", "MELON", "ORANGE", "PAPAYA", "PEACH", "PEAR", "PINEAPPLE", "POMEGRANATE", "WATERMELON" };
    public static string[] Home = {"Home","BED", "BOOKS", "BOTTLE", "BOWL", "BROOM", "BUCKET", "BULB", "CHAIR", "CLOCK", "COMPUTER", "COUCH", "CUP", "DOOR", "FAN", "GLASS", "HOUSE", "JUG", "PEN", "PENCIL", "PLATE", "SPOON", "TABLE", "TEAPOT", "WALL", "WINDOW" };
    public static string[] Insects = {"Insects","ANT", "BEE", "BEETLE", "BUTTERFLY", "CATERPILLAR", "COCKROACH", "CRICKET", "DRAGONFLY", "HORNET", "LADYBUG", "MANTIS", "MOSQUITO", "SCORPION", "SPIDER" };
    public static string[] Numbers = {"Numbers","EIGHT", "FIVE", "FOUR", "NINE", "ONE", "SEVEN", "SIX", "THREE", "TWO", "ZERO" };
    public static string[] Professions = {"Professions","ACCOUNTANT", "ARTIST", "COOK", "DOCTER", "FIREFIGHTER", "PILOT", "PLUMBER", "POLICE", "SCIENTIST", "TEACHER" };   
    //static string[] Shapes = {};
    public static string[] Snacks_and_Meals = {"Snacks_and_Meals","BREAD", "CAKE", "CHEESE", "CHIPS", "COFFEE", "COOKIES", "DONUTS", "EGG", "HONEY", "ICECREAM", "JELLY", "MILK", "NOODLES", "PANCAKE", "PIZZA", "POPCORNS", "RICE", "SANDWICH", "SOUP", "TORTILLA", };
    public static string[] Space = {"Space","EARTH", "JUPITER", "MARS", "MERCURY", "MOON", "NEPTUNE", "PLUTO", "SATURN", "SPACE", "SUN", "URANUS", "VENUS", };        
    public static string[] Vegetables = {"Vegetables","BEET", "BROCCOLI", "CABBAGE", "CAPSICUM", "CARROT", "CHILLI", "CORN", "CUCUMBER", "EGGPLANT", "GARLIC", "GINGER", "MUSHROOM", "ONION", "PEAS", "POTATO", "PUMPKIN", "RADISH", "TOMATO", };
    public static string[] Vehicles = {"Vehicles","AIRPLANE", "BICYCLE", "BUS", "CAR", "EXCAVATOR", "HELICOPTER", "LORRY", "MONSTER TRUCK", "MOTORCYCLE", "ROCKET", "SAIL SHIP", "SCOOTER", "SHIP", "SUBMARINE", "TANK", "TRACTOR", "TRAIN", "TRUCK", };
    public static string[] Wild_Animals = {"Wild_Animals","BAT", "BEAR", "CROCODILE", "DEER", "ELEPHANT", "FOX", "FROG", "GIRAFFE", "KANGAROO", "KOALA", "LION", "MONKEY", "PANDA", "RABBIT", "RHINO", "SNAIL", "SNAKE", "SQUIRREL", "TIGER", "TURTLE", "ZEBRA", };
    public static string[] Category_Icons_Names = {"Category_Icons" , "Aquatic_Animals", "Berries_and_Nuts", "Birds", "Clothes", "Domestic_Animals", "Flowers", "Fruits", "Home", "Insects", "Professions", "Snacks_and_Meals", "Space", "Vegetables", "Vehicles", "Wild_Animals"};
    public static List<string[]> Category_Icons = new List<string[]> {Aquatic_Animals, Berries_and_Nuts, Birds, Clothes, Domestic_Animals, Flowers, Fruits, Home, Insects,Numbers, Professions, Snacks_and_Meals, Space, Vegetables, Vehicles, Wild_Animals};

    public GameObject r3AlphaGridPrefab;
    public List<GameObject> r3AnsAlphaPos = new List<GameObject>();
    public static string[] r3RandCategory;
    public static Sprite tapSprite1, tapSprite2;
    public static List<Sprite> randomCategory = new List<Sprite>();
    public static List<Sprite> r2RandCategory = new List<Sprite>();
    public static int r1RandFileIndex = 0, tapItemIndex, tapItemIndex2;
    public static string randomFileName;
    public static List<string> tilesToTap = new List<string>(); 
    public List<GameObject> MatchedTilesPairs = new List<GameObject>();
    List<int> tilesToCountIndexList = new List<int>();
    List<string> tilesToTapBot = new List<string>();
    public static bool isAssetsLoaded;
    public static string tilePath = null; 
    public static Sprite tapTilesRandomSprite = null;
    public List<AudioClip> R1audioClips,R2audioClips,R3audioClips;
    public AudioSource R1audioSource, R2audioSource, R3audioSource;
    public static bool isAudioPlayed = false;
    public static int TapTilesMaxImage = 0;
    public static int rCountTilesMaxImage = 0;
    //Check the editor for pointerBarData values
    public List<Slider> p1points = new List<Slider>(4), p2points = new List<Slider>(4);
    //a string to distinguise between player1 and player2
    string commonSubstring = null;
    public static IDictionary<string, object> userData = new Dictionary<string, object>();
    public RConfigs configs;
    bool isCounted = false;
    bool first = true;
    float currentTime = 0.0f, endTime = 1.0f, waitingTime = 0.0f, waitingEndTime = 0.0f;
    public float tTimer = 0.0f,mTimer = 0.0f,cTimer = 0.0f;
    List <int> randomNumList = new List<int> ();
    public static List <int> GenerateRandomList(int minNumbers, int maxNumbers)
    {
        List <int> uniqueNumbers = new List<int> ();
        List <int> NumbersList = new List<int> ();
        for(int i = minNumbers; i < maxNumbers; i++)
        {
            uniqueNumbers.Add(i);
        }
        for(int i = minNumbers; i< maxNumbers; i ++){
        int ranNum = uniqueNumbers[Random.Range(0,uniqueNumbers.Count)];
        NumbersList.Add(ranNum);
        uniqueNumbers.Remove(ranNum);
        }
        return NumbersList; 
    }

    void awake()
    {
        TapTilesMaxImage = 0;
        rCountTilesMaxImage = 0;
        isAssetsLoaded = false;
    }
    IEnumerator Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        isAudioPlayed = false;
        p1points.Add(GameObject.Find("TapTilePoints").GetComponent<Slider>());
        p1points.Add(GameObject.Find("MatchTilePoints").GetComponent<Slider>());
        p1points.Add(GameObject.Find("CountTilePoints").GetComponent<Slider>());
        p1points.Add(GameObject.Find("SpellFinderPoints").GetComponent<Slider>());

        p2points.Add(GameObject.Find("TapTilePoints_p2").GetComponent<Slider>());
        p2points.Add(GameObject.Find("MatchTilePoints_p2").GetComponent<Slider>());
        p2points.Add(GameObject.Find("CountTilePoints_p2").GetComponent<Slider>());
        p2points.Add(GameObject.Find("SpellFinderPoints_p2").GetComponent<Slider>());

        if(TapTilesMaxImage == 0 && rCountTilesMaxImage == 0)
        {
            TapTilesMaxImage = 3;
            rCountTilesMaxImage = Random.Range(6, 12);
        }
        //if(transform.gameObject.name == "MatchTiles3" || transform.gameObject.name == "CountTiles" || gameObject.name == "TapTheTiles")
        //yield return LoadAssets();
        //yield return new WaitForSeconds(1.5f);
        yield return AssetLoader();
        if(transform.gameObject.name == "TapTheTiles")
        {
            userData.Clear();
            foreach(Transform child in transform)
            {
                LeanTween.scale(child.gameObject, Vector3.one,1f);
            }
        }
        if(transform.gameObject.name == "TapTheTiles (1)")
        {
            R1audioSource.clip = R1audioClips[0];
            R1audioSource.Play();
            yield return new WaitForSeconds(R1audioSource.clip.length);
            R1audioSource.clip = R1audioClips[1];
            R1audioSource.Play();
            yield return new WaitForSeconds(R1audioSource.clip.length);
            GameObject.Find("R1Img").LeanScale(new Vector3(0,1,1), 1f).setEaseInCubic().setOnComplete(() =>
            {
                isAudioPlayed = true;
            });
            endTime = Random.Range(RConfigs.tr1, RConfigs.tr2);
        }
        if(transform.gameObject.name == "MatchTiles3 (1)")
        {
            R2audioSource.clip = Resources.Load<AudioClip>("Match These Pics");
            R2audioSource.Play();
            GameObject images = GameObject.Find("1stColumn");
            images.transform.GetChild(0).LeanScale(Vector3.one, 0.5f).setEaseOutBounce().setOnComplete(() => 
            {
                images.transform.GetChild(1).LeanScale(Vector3.one, 0.5f).setEaseOutBounce().setOnComplete(() => 
                {
                    images.transform.GetChild(2).LeanScale(Vector3.one, 0.5f).setEaseOutBounce();
                });
            });
            yield return new WaitForSeconds(R2audioSource.clip.length - 1);
            GameObject.Find("R2Img").LeanScale(new Vector3(0,1,1), 1f).setEaseInCubic().setOnComplete(() =>
            {
                isAudioPlayed = true;
            });
        }
        if(transform.gameObject.name == "CountTiles")
        {
            foreach(Transform child in transform.GetChild(0))
            {
                LeanTween.scale(child.gameObject, Vector3.one,1f);
            }
            R3audioSource.clip = R3audioClips[0];
            R3audioSource.Play();
            yield return new WaitForSeconds(R3audioSource.clip.length);
            R3audioSource.clip = R3audioClips[1];
            R3audioSource.Play();
            yield return new WaitForSeconds(R3audioSource.clip.length);
            GameObject.Find("R3Img").LeanScale(new Vector3(0,1,1), 1f).setEaseInCubic().setOnComplete(() =>
            {
                isAudioPlayed = true;
            });
            endTime = Random.Range(RConfigs.cr1, RConfigs.cr2);
        }

        isAssetsLoaded = true;
    }
    bool isAnswered = false;
    //Count-Tiles Answer Options ButtonFunction only for the real Player(or the player 1)
    void ButtonFunction()
    {
        if(!isAnswered && p2points[2].value != p2points[1].value + 1)
        {
            if(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text == rCountTilesMaxImage.ToString())
            {
                GameObject.Find(rCountTilesMaxImage.ToString()+commonSubstring).GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                R3audioSource.clip = Resources.Load<AudioClip>("SFX/Success3");
                R3audioSource.Play();
                p1points[2].value += 1;
            }
            else
            {
                GameObject.Find(EventSystem.current.currentSelectedGameObject.name).GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
                R3audioSource.clip = Resources.Load<AudioClip>("SFX/Failure");
                R3audioSource.Play();
            }  
            isAnswered = true;          
        }
    }
    int i = 0;
    bool istTimerDisplayed = false, iscTimerDisplayed = false,ismTimerDisplayed = false;
    // Update is called once per frame
    void Update()
    {
        #region TimeTrackers
        if(transform.gameObject.name == "TapTheTiles" && p1points[0].value != TapTilesMaxImage)
        {
            if(p2points[0].value != TapTilesMaxImage && isAudioPlayed) tTimer += Time.deltaTime;
            else    tTimer = 0.0f;
        }
        else if(!istTimerDisplayed && transform.gameObject.name == "TapTheTiles" && tTimer > 0 && p1points[0].value == TapTilesMaxImage)  
        { userData.Add("tTimer", tTimer); istTimerDisplayed = true;}

        if(transform.gameObject.name == "MatchTiles3" && p1points[1].value != p1points[0].value + 3)
        {
            if(p2points[1].value != p2points[0].value + 3 && isAudioPlayed) mTimer += Time.deltaTime;
            else    mTimer = 0.0f;
        }
        else if(!ismTimerDisplayed && transform.gameObject.name == "MatchTiles3" && mTimer > 0 && p1points[1].value == p1points[0].value + 3)  
        { userData.Add("mTimer", mTimer); ismTimerDisplayed = true;}
         
        if(transform.gameObject.name == "CountTiles" && p1points[2].value != p1points[1].value + 1)
        {
            if(p2points[2].value != p2points[1].value + 1 && isAudioPlayed) cTimer += Time.deltaTime;
            else    cTimer = 0.0f;
        }
        else if(!iscTimerDisplayed && transform.gameObject.name == "CountTiles" && cTimer > 0)  
        { userData.Add("cTimer", cTimer); iscTimerDisplayed = true;}
        #endregion
        #region Bot logic
        //TapTile Bot logic (or the Player 2)
        if(transform.gameObject.name == "TapTheTiles (1)" && isAudioPlayed && p1points[0].value != TapTilesMaxImage)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= endTime && i < TapTilesMaxImage) 
            {
                if(first)
                {
                    randomNumList = GenerateRandomList(0,TapTilesMaxImage);
                    first = false;
                }

                GameObject.Find(tilesToTapBot[randomNumList[i]]).GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                p2points[0].value += 1;
                currentTime = 0;
                endTime = Random.Range(RConfigs.tr1, RConfigs.tr2);
                i++;
            }
        }
        //CountTiles Bot logic                                                     //if the opponent(player1) is not completed CountTile lavel 
        else if(transform.gameObject.name == "CountTiles (1)" && isAudioPlayed && p1points[2].value != p1points[1].value + 1)
        {                                                                          //check it by checking its pointBar slider value
            currentTime += Time.deltaTime;
            //Select all the Objects
            if(currentTime >= endTime && i < rCountTilesMaxImage) 
            {
                if(first)
                {
                    tilesToCountIndexList.Sort();
                    first = false;
                }
                string s = "Bordered Image ("+tilesToCountIndexList[i]+")_p2";
                GameObject.Find("Bordered Image ("+tilesToCountIndexList[i]+")_p2").GetComponent<Image>().color = new Color(0.9686f,0.439215f,0,1);
                endTime = Random.Range(RConfigs.cr1, RConfigs.cr2);
                currentTime = 0;
                i++;            
            } 
            //Press the Right Option and Finish the match by declaring the Winner
            if(i == rCountTilesMaxImage && !isCounted) 
            {
                GameObject.Find(rCountTilesMaxImage+"_p2").GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
                //Final increment of the Indicator
                p2points[2].value += 1;
                isCounted = true;
            }
        }
        #endregion
    }
    // void OnDestroy()
    // {
    //     if(gameObject.name == "CountTiles" || gameObject.name == "CountTiles (1)")
    //     {
    //         TapTilesMaxImage = 0;
    //         rCountTilesMaxImage = 0;
    //         r3RandCategory = null;
    //     }

    // }
    IEnumerator AssetLoader()
    {
        List <int> randomNumbersList = new List<int> ();
        if (gameObject.name == "TapTheTiles" || gameObject.name == "TapTheTiles (1)")
        {
            if(gameObject.name == "TapTheTiles")
            {
                commonSubstring = "";
                randomCategory.Clear();
                MainContentGetter.spriteList.ForEach((item) => {randomCategory.Add(item);});
                r1RandFileIndex = Random.Range(1, randomCategory.Count - 1);
                tapTilesRandomSprite = randomCategory[r1RandFileIndex];
            } 
            else
            {
                //wait for TapTheTiles object to finish loading
                yield return new WaitForSeconds(0.1f);
                commonSubstring = "_p2";
            }
            if(gameObject.name == "TapTheTiles (1)")
            foreach(var clip in MainContentGetter.audioClipList)
            {
                if(clip.name.Trim() == "_"+tapTilesRandomSprite.name)
                {
                    R1audioClips.Add(clip);
                    break;
                }
            }
            randomNumbersList = GenerateRandomList(0,9);
            for (int i = 0; i < TapTilesMaxImage; i++)
            {
                GameObject.Find("pic" + randomNumbersList[i]+commonSubstring).GetComponent<ProceduralImage>().sprite = tapTilesRandomSprite;
                //store the name of the Top most parent of the Answer Tile gameObjects for the player2
                if(gameObject.name == "TapTheTiles (1)") tilesToTapBot.Add(GameObject.Find("pic" + randomNumbersList[i]+commonSubstring).transform.parent.transform.parent.name);
                GameObject.Find("pic" + randomNumbersList[i]+commonSubstring).name = tapTilesRandomSprite.name+commonSubstring + i;
                //store the Answer Tile names
                if(gameObject.name == "TapTheTiles")
                tilesToTap.Add(tapTilesRandomSprite.name + i);
            }
            try
            {
                randomCategory.RemoveAt(r1RandFileIndex);
            }catch{}
            
            tapItemIndex = Random.Range(1, randomCategory.Count - 1);
            if(gameObject.name == "TapTheTiles")
            tapSprite1 = randomCategory[tapItemIndex];
            randomCategory.Remove(tapSprite1);
            tapItemIndex2 = Random.Range(1, randomCategory.Count - 1);
            if(gameObject.name == "TapTheTiles")
            tapSprite2 = randomCategory[tapItemIndex2];
            for (int i = 3; i <= 5; i++)
            {
                GameObject go = GameObject.Find("pic" + randomNumbersList[i]+commonSubstring);
                go.GetComponent<ProceduralImage>().sprite = tapSprite1;
                go.name = tapSprite1.name+commonSubstring; 
            }
            for (int i = 6; i <= 8; i++)
            {
                GameObject go = GameObject.Find("pic" + randomNumbersList[i]+commonSubstring);
                go.GetComponent<ProceduralImage>().sprite = tapSprite2;
                go.name = tapSprite2.name+commonSubstring; 
            }
        }
        else if(gameObject.name == "MatchTiles3" || gameObject.name == "MatchTiles3 (1)")
        {     
            if(gameObject.name == "MatchTiles3") {commonSubstring = "";}
            else {commonSubstring = "_p2"; yield return new WaitForSeconds(0.00000001f);}
            randomNumbersList = GenerateRandomList(0,3);
            List<string> randomNames = new List<string>();
            //int randomIndex = Random.Range(0, Category_Icons.Count - 1);
            r2RandCategory.Clear();
            MainContentGetter.spriteList.ForEach((item) => {r2RandCategory.Add(item);});
            for(int i = 0;i < 3;i++)
            {
                int rfileIndex = Random.Range(1, r2RandCategory.Count - 1);
                Sprite rSprite = r2RandCategory[rfileIndex];//Debug.Log(rfileIndex+": "+rFileName);
                r2RandCategory.Remove(rSprite);
                GameObject imageGameObject = GameObject.Find(i+"1st"+commonSubstring);
                GameObject NameGameObject = GameObject.Find(randomNumbersList[i]+"2nd"+commonSubstring);
                if(gameObject.name == "MatchTiles3 (1)")
                {
                    MatchedTilesPairs.Add(imageGameObject);
                    MatchedTilesPairs.Add(NameGameObject);
                }
                imageGameObject.GetComponent<ProceduralImage>().sprite = rSprite;
                imageGameObject.name = rSprite.name+"1st"+commonSubstring;
                //transform WORD to Word
                string itemName = rSprite.name;
                StringBuilder sb = new StringBuilder(itemName);
                for(int j = 0; j < itemName.Length; j++)
                {
                    if(j == 0)
                    { 
                        sb[j] = itemName[j];
                        continue;
                    }
                    sb[j] =  char.ToLower(itemName[j]);
                }
                NameGameObject.GetComponent<TextMeshProUGUI>().text = sb.ToString();
                NameGameObject.name = rSprite.name+"2nd"+commonSubstring;
            }
        }
        else if (gameObject.name == "CountTiles" || gameObject.name == "CountTiles (1)")
        {
            if(gameObject.name == "CountTiles")
            { 
                commonSubstring = "";
            }
            else 
            {
                commonSubstring = "_p2";
                yield return new WaitForSeconds(0.001f);
            }
            List<Sprite> randomCategory = new List<Sprite>(MainContentGetter.spriteList.Count);
            MainContentGetter.spriteList.ForEach((item) => {randomCategory.Add(item);});
            Sprite sprite = randomCategory[Random.Range(1, randomCategory.Count - 1)];
            
            if(gameObject.name == "CountTiles")   
            foreach(var clip in MainContentGetter.audioClipList)
            {
                if(clip.name == "_"+sprite.name)
                {
                    R3audioClips.Add(clip);
                    break;
                }
            }     
            randomNumbersList = GenerateRandomList(0,15);
            for(int i = 0; i < rCountTilesMaxImage; i ++)
            {
                GameObject.Find("Image ("+randomNumbersList[i]+")"+commonSubstring).GetComponent<Image>().sprite = sprite;
                if(gameObject.name == "CountTiles (1)") tilesToCountIndexList.Add(randomNumbersList[i]);
                GameObject.Find("Image ("+randomNumbersList[i]+")"+commonSubstring).name = sprite.name+commonSubstring;
            }
            for(int i = rCountTilesMaxImage; i < 15; i ++)
            {
                //GameObject.Find("Bordered Image ("+randomNumbersList[i]+")").SetActive(false);
                GameObject.Find("Bordered Image ("+randomNumbersList[i]+")"+commonSubstring).GetComponent<Image>().color = new Color(0,0,0,0);
                GameObject.Find("Bordered Image ("+randomNumbersList[i]+")"+commonSubstring).transform.GetChild(0).GetComponent<Image>().color = new Color(0,0,0,0);
                GameObject.Find("Bordered Image ("+randomNumbersList[i]+")"+commonSubstring).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(0,0,0,0);
            }                
            
            //Generate 4 random Options to Choose...
            randomNumbersList = GenerateRandomList(rCountTilesMaxImage - 1, rCountTilesMaxImage + 3);
            for(int i = 0; i < 4; i++)
            {
                GameObject.Find("Button ("+i+")"+commonSubstring).GetComponentInChildren<TextMeshProUGUI>().text = randomNumbersList[i].ToString();
                if(transform.gameObject.name == "CountTiles")
                GameObject.Find("Button ("+i+")"+commonSubstring).GetComponent<Button>().onClick.AddListener(ButtonFunction);
                GameObject.Find("Button ("+i+")"+commonSubstring).name = randomNumbersList[i].ToString() + commonSubstring;
            }
        } 
        yield return null;
    }


    // AsyncOperationHandle<IList<Object>> spriteOperationHandle;
    // IEnumerator LoadAssets()
    // {  
    //     foreach (var sArray in RandomContentGetter.Category_Icons)
    //     {
            
    //         if(sArray.Contains("Birds"))
    //         {
    //             MainContentGetter.filesTofatch = sArray;
    //             MainContentGetter.spriteList.Clear();
    //             MainContentGetter.audioClipList.Clear();
    //             break;
    //         }
    //     }
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
    //     if(SceneChanger.currentCategory == "Numbers")
    //     {
    //         MainContentGetter.spriteList = MainContentGetter.spriteList.OrderBy(sprite => Regex.Replace(sprite.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
    //         MainContentGetter.audioClipList = MainContentGetter.audioClipList.OrderBy(clip => Regex.Replace(clip.name, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
    //         foreach(var s in MainContentGetter.spriteList)
    //             s.name = Regex.Replace(s.name, @"\d", "");
    //         foreach(var a in MainContentGetter.audioClipList)
    //             a.name = Regex.Replace(a.name, @"\d", "");
    //     }
    // }
}