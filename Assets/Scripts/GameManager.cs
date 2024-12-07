using Cinemachine;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.UI;

///<summary>
///
/// Fire removed
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("General")]
    public Player player_1;
    public Enemy_AI player_2;
    int turn = 0;
    public CinemachineVirtualCamera cam;
    public Camera cam_;
    [SerializeField] private Transform End_Point; // Camera player pos
    public Transform Main_Point;
    
    public Transform PlayerStuff;
    public Level_Data level_Data;

    [HideInInspector] public bool isChecking = false;
    public GameObject FlamesUI;
    public GameObject ShieldUI;

    [Header("Abilities usage")]
    public int Fire_Uses;
    public int Burst_Uses;

    [Header("Power Ups")]
    [Header("Sheild")]
    public GameObject SheildObj;
    public int Sheild_UsagePerGame = 2;
    private int current_Usage_Sheild;
    public int Sheild_count; // sheild block shots

    [Header("Freez")]
    public GameObject FreezObj;
    public int Freez_count; // freez upcoming shots
    private int current_Usage_freeze;
    public int Freez_UsagePerGame = 2;

    [Header("Tiny Shots")]
    public int TinyShots_count; // make upcoming shots tiny and reduce damage by 50%
    private int current_Usage_TinyShots;
    public int TinyShots_UsagePerGame = 3;

    [Header("Tiny Ship")]
    public GameObject PlayerShip;
    public int TinyShip_count; // make ship tiny
    private int current_Usage_TinyShip;
    public int TinyShip_UsagePerGame = 3;

    [Header("Power Ups Cost")]
    public Cost Sheild_cost = new Cost(500, 3);
    public Cost Freez_cost = new Cost(500, 3);
    public Cost TinyShots_cost = new Cost(300, 1);
    public Cost TinyShip_cost = new Cost(300, 1);

    [Header("Currency")]
    public int Coins = 50000;
    public int Diamond = 1000;
    public int Coins_Start;

    [Header("Abilites Cost")]
    public Cost Fire_Cost = new Cost(60, 0);
    public Cost Burst_Cost = new Cost(120, 0);

    [Header("Current level")]
    public int Current_Level = 0;

    [Header("Ships")]
    public GameObject[] Ships;

    [Header("Quests")]
    public DateTime lastLogin;
    public List<int> currentQuests;
    public QuestData questsData;
    public int WinCount;
    public int FireShots;
    public bool MissShot;
    public int noMissShots;
    public int ReadNotification;

    [Header("Gifts")]
    public List<int> currentGifts;
    public GiftsData giftsData;

    [Header("Account")]
    public int totalMatchLost;
    public int totalMatchWin;
    public int FireShotsHit;
    public int TotalShotsFired;
    public int TotalShotsHit;
    public int TotalShotsMiss;
    [Header("Game Phase")]
    public GamePhase phase;
    [Space(20)]

    public Shop shop;
    public UI_Controller uI_Controller;
    public Upgrades upgrades;


    int first;
    

    [Header("Tutoriols")]
    public Transform Hand;
    public Transform Pointer;
    int tut;
    public CanvasGroup captain;
    public TMPro.TMP_Text tutText;
    public CanvasGroup ReadyTut;
    public GameObject TutObj;
    public Button Next;

    [Header("Audio & Music")]
    public SoundEffects Soundeffects;
    public float MusicVolume = 0.5f;
    public float SoundVolume = 0.5f;
    public AudioSource MusicSource;
    public Slider MusicSlider;
    public Slider SoundSlider;
    public GameObject AudioInstance;
    public AudioSource OceanBackGround; // Backgound sound Effect
    int end;



    private void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
            if (PlayerPrefs.GetInt("First") == 0)
            {
                PlayerPrefs.SetInt("First", 1);
                setDefaultData();
            }
        }
        else
        {            
            Destroy(gameObject);
        }
        LoadData();
    }

    private void Start()
    {
        ShowTut();        
        phase = GamePhase.MainMenu;        
    }

    public void Play()
    {
        UI_Controller.instance.Menu_BG.interactable = true;
        UI_Controller.instance.Menu_BG.blocksRaycasts = true;
        UI_Controller.instance.Menu_BG.DOFade(1, 0.5f);
        UI_Controller.instance.Level_Counter.text = $"{GetLevel}";
        UI_Controller.instance.SetAbilitesCount();
    }

    public int GetLevel
    {
        get
        {
            return player_2.levels.Get_Level(Current_Level).level;
        }
    }
    void MovePlayerAndCamera()
    {
        PlayerStuff.localPosition = new Vector3(0, 0, 0);
        End_Point.localPosition = new Vector3(-49.97f, 2.18f, -18.4f);
        Level lvl = level_Data.Get_Level(Current_Level);
        PlayerStuff.localPosition = new Vector3(-lvl.distance, 0, 0);
        End_Point.localPosition = new Vector3(End_Point.localPosition.x - lvl.distance, End_Point.localPosition.y, -18.4f);
    }
    public void Start_Game()
    {        
        //ui controller
        UI_Controller.instance.Main_Menu.DOFade(0, 0.3f);
        UI_Controller.instance.Main_Menu.interactable = false;
        UI_Controller.instance.Main_Menu.blocksRaycasts = false;
        UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(false);

        MusicSource.DOFade(0, 2);
        Crab.instance.Escaped = true;

        MovePlayerAndCamera();
        phase = GamePhase.Start;
        //get abilites if exited
        Check_Abilites();
        UI_Controller.instance.Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            cam_.GetComponent<CameraFollow>().SetTarget(null);
            cam_.transform.DOMove(End_Point.localPosition, 5).OnComplete(() =>
            {
                UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(true);
                Get_Ready_UI(1);
                phase = GamePhase.ReadyPhase;
                player_1.enabled = true;
                player_1.GetComponent<Rotate_Object>().enabled = true;
                player_2.enabled = false;
                cam_.GetComponent<CinemachineBrain>().enabled = true;
                cam.gameObject.SetActive(true);
                cam.Follow = player_1.transform.parent.transform;
                MusicSource.clip = Soundeffects.BattleMusic;
                MusicSource.DOFade(MusicVolume, 2);
                MusicSource.Play();
                //cam_.GetComponent<CameraFollow>().SetTarget(player_1.transform.parent.transform);
            });
        });
        UI_Controller.instance.Menu_BG.interactable = false;
        UI_Controller.instance.Menu_BG.blocksRaycasts = false;
        
        Coins_Start = Coins;
        Reset_Ships(false);
        ResetPowerUpsCurrent();
        player_2.ResetPowerUpsEnemy();
        Set_Player();
        player_1.GetAllSkins();
        player_2.Get_Stats(Current_Level);
        turn = 2;
        MissShot = false;
        UI_Controller.instance.SetPlayerStats();        
    }

    public void Check_Turn()
    {
        turn++;
        if (turn % 2 == 0)
        {
            player_1.enabled = true;
            player_1.GetComponent<Rotate_Object>().enabled = true;
            player_2.enabled = false;
            cam.Follow = player_1.transform.parent.transform;            
            Check_Abilites();
            Get_Ready_UI(1);
            phase = GamePhase.ReadyPhase;
            UncheckPowerUps();
            if (SheildObj.activeInHierarchy)
                SheildObj.SetActive(false);
            if (PlayerShip.transform.localScale.x == 2)
            {
                Ship ship = Ships[0].GetComponent<Ship>();
                ship.Floating = true;
                PlayerShip.transform.DOScale(new Vector3(4.3f, 4.3f, 1), 0.2f);
                PlayerShip.transform.localPosition = new Vector3(PlayerShip.transform.localPosition.x, -1.73f, PlayerShip.transform.localPosition.z);
            }
            
        }
        else
        {
            player_1.enabled = false;
            player_1.GetComponent<Rotate_Object>().enabled = false;
            player_2.enabled = true;
            player_2.Shoot_Invoked(2);
            cam.Follow = player_2.transform.GetChild(1).GetChild(1).transform;
            phase = GamePhase.EnemyReadyPhase;
            player_2.ResetShipEnemy();
        }
    }


    public void Get_Ready_UI(float fade, float duration = 0.5f)
    {
        if (fade == 1)
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration);
            UI_Controller.instance.Getting_Ready_Object.interactable = true;
            UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = true;
            if (FlamesUI.activeInHierarchy)
            {
                UI_Animator infernoAnim = FlamesUI.GetComponent<UI_Animator>();
                infernoAnim.Func_PlayUIAnim();
            }            
        }
        else
        {
            UI_Controller.instance.Getting_Ready_Object.DOFade(fade, duration).OnComplete(() =>
            {
                UI_Controller.instance.Getting_Ready_Object.interactable = false;
                UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
            });
        }

    }

    void Check_Abilites()
    {
        if (Fire_Uses == 0)
            UI_Controller.instance.Fire_Object.SetActive(false);
        else
            UI_Controller.instance.Fire_Object.SetActive(true);

        if (Burst_Uses == 0)
            UI_Controller.instance.Burst_Object.SetActive(false);
        else
            UI_Controller.instance.Burst_Object.SetActive(true);
    }

    public void Check_PowerUps()
    {
        UI_Controller.instance.All_PowerUps.SetActive(true);
        if (Sheild_count > 0 && current_Usage_Sheild < Sheild_UsagePerGame)
        {
            UI_Controller.instance.Sheild_Object.SetActive(true);
            UI_Animator infernoAnim = ShieldUI.GetComponent<UI_Animator>();
            infernoAnim.Func_PlayUIAnim();
        }            
        else
            UI_Controller.instance.Sheild_Object.SetActive(false);

        if (Freez_count > 0 && current_Usage_freeze < Freez_UsagePerGame)
            UI_Controller.instance.Freez_Object.SetActive(true);
        else
            UI_Controller.instance.Freez_Object.SetActive(false);

        if (TinyShots_count > 0 && current_Usage_TinyShots < TinyShots_UsagePerGame)
            UI_Controller.instance.TinyShots_Object.SetActive(true);
        else
            UI_Controller.instance.TinyShots_Object.SetActive(false);

        if (TinyShip_count > 0 && current_Usage_TinyShip < TinyShip_UsagePerGame)
            UI_Controller.instance.TinyShip_Object.SetActive(true);
        else
            UI_Controller.instance.TinyShip_Object.SetActive(false);
    }

    void UncheckPowerUps()
    {
        UI_Controller.instance.All_PowerUps.SetActive(false);
        UI_Controller.instance.Sheild_Object.SetActive(false);
        UI_Controller.instance.Freez_Object.SetActive(false);
        UI_Controller.instance.TinyShots_Object.SetActive(false);
        UI_Controller.instance.TinyShip_Object.SetActive(false);
    }
    void ResetPowerUpsCurrent()
    {
        current_Usage_freeze = 0;
        current_Usage_Sheild = 0;
        current_Usage_TinyShip = 0;
        current_Usage_TinyShots = 0;
    }
    public void Get_Fire()
    {
        if (Coins >= Fire_Cost.Coins)
        {
            Fire_Uses++;
            Coins -= Fire_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
            SaveData("coins", Coins);

            //audio
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Buy);
        }
        else
        {
            UI_Controller.instance.FeedBackPopUp("Not enough currency", UI_Controller.FeedbackType.failed);
        }
    }
    public void Dump_Fire()
    {
        if (Fire_Uses != 0)
        {
            Fire_Uses--;
            Coins += Fire_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("fireUses", Fire_Uses);
            SaveData("coins", Coins);
        }
    }
    public void Get_Burst()
    {
        if (Coins >= Burst_Cost.Coins)
        {
            Burst_Uses++;
            Coins -= Burst_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
            SaveData("coins", Coins);

            //audio
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Buy);
        }
        else
        {
            UI_Controller.instance.FeedBackPopUp("Not enough currency", UI_Controller.FeedbackType.failed);
        }
    }
    public void Dump_Burst()
    {
        if (Burst_Uses != 0)
        {
            Burst_Uses--;
            Coins += Burst_Cost.Coins;
            UI_Controller.instance.SetAbilitesCount();
            UI_Controller.instance.SetCurrencyUI();
            SaveData("burstUses", Burst_Uses);
            SaveData("coins", Coins);
        }
    }

    public void Reset_play()
    {
        player_1.enabled = false;
        player_2.enabled = false;
        player_2.StopAllCoroutines();
    }

    private void Reset_Ships(bool restart)
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            Ships[i].gameObject.SetActive(true);
        }
        Ship[] ships = FindObjectsOfType<Ship>();
        for (int i = 0; i < ships.Length; i++)
        {
            if (!restart)
                ships[i].Get_Health();
            else
            {
                if (ships[i].player)
                    ships[i].Get_Player_Health();
            }

            if(ships[i].Start_Pos != null)
                ships[i].transform.localPosition = ships[i].Start_Pos;
        }

        Ships[0].GetComponentInChildren<Player>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        Ships[1].GetComponentInChildren<Enemy_AI>().Canon.localRotation = Quaternion.Euler(0, 0, 0);

        Transform fireTransform_0 = Ships[0].transform.Find("Fire(Clone)");
        if (fireTransform_0 != null)
        {
            GameObject fire_0 = fireTransform_0.gameObject;
            Destroy(fire_0);
        }

        Transform fireTransform_1 = Ships[1].transform.Find("Fire(Clone)");
        if (fireTransform_1 != null)
        {
            GameObject fire_1 = fireTransform_1.gameObject;
            Destroy(fire_1);
        }       
    }

    public void Set_Player()
    {
        UI_Controller.instance.ResetChecks();
        if (UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);
        else
            UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(true);

        if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == 0)
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
        else
            UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(true);

        if (Shop.Instance.Got_Extra_Slot == 1)
        {
            if (UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index == 0)
                UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(false);
            else
                UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(true);

            UI_Controller.instance.Select_Bullet_Extra.onClick.RemoveAllListeners();
            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().bullet;
            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index;

            UI_Controller.instance.Select_Bullet_Extra.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index));

            UI_Controller.instance.Select_Bullet_Extra.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().CheckForSelcted());

            UI_Controller.instance.bullet_slot_Extra.gameObject.SetActive(true);
            UI_Controller.instance.Extra_Bullet_Buy_Button.SetActive(false);

            UI_Controller.instance.Select_Bullet_Extra.GetComponent<Bullet_Select>().Get_Bullet_Stuff();
        }

        UI_Controller.instance.Select_Bullet_1.onClick.RemoveAllListeners();
        UI_Controller.instance.Select_Bullet_2.onClick.RemoveAllListeners();

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().bullet;
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().currentBullet = UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().bullet;

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index;
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().index = UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index;

        UI_Controller.instance.Select_Bullet_1.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index));
        UI_Controller.instance.Select_Bullet_2.onClick.AddListener(() => player_1.SelectBullet(UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index));

        UI_Controller.instance.Select_Bullet_1.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().CheckForSelcted());
        UI_Controller.instance.Select_Bullet_2.onClick.AddListener(() => UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().CheckForSelcted());

        UI_Controller.instance.Select_Bullet_1.GetComponent<Bullet_Select>().Get_Bullet_Stuff();
        UI_Controller.instance.Select_Bullet_2.GetComponent<Bullet_Select>().Get_Bullet_Stuff();

        UI_Controller.instance.unCheckAbilities();

        UI_Controller.instance.ResetChecks();
        UI_Controller.instance.Checks[0].SetActive(true);
        player_1.SelectBullet(0);
        player_1.ready = false;
        player_1._bulletsLimit.Clear();
    }


    public void SaveData(string key, int value)
    {        
        PlayerPrefs.SetInt(key, value);        
    }
    public void SaveData(string key, float value)
    {        
        PlayerPrefs.SetFloat(key, value);        
    }
    public void SaveData(string key, string value)
    {        
        PlayerPrefs.SetString(key, value);        
    }
    public void SaveData(string key, List<int> value)
    {        
        string list = string.Join(",", value.ConvertAll(i => i.ToString()).ToArray());
        PlayerPrefs.SetString(key, list);        
    }



    public void SaveLastLogin()
    {
        lastLogin = DateTime.Now;
        PlayerPrefs.SetString("LastLogin", lastLogin.ToString("o"));
    }
    public void SaveLogin()
    {        
        string lastLoginString = PlayerPrefs.GetString("LastLogin");
        DateTime lastLogin;
        
        if (DateTime.TryParse(lastLoginString, out lastLogin))
        {            
            lastLogin = lastLogin.AddDays(1);
        }
        else
        {            
            lastLogin = DateTime.Now;
        }
        
        PlayerPrefs.SetString("LastLogin", lastLogin.ToString("o"));
    }

    public bool Has24HoursPassed()
    {
        if (PlayerPrefs.HasKey("LastLogin"))
        {
            string lastLoginStr = PlayerPrefs.GetString("LastLogin");
            DateTime lastLogin = DateTime.Parse(lastLoginStr);

            TimeSpan timeSinceLastLogin = DateTime.Now - lastLogin;            
            if (timeSinceLastLogin.TotalHours >= 24)
            {
                return true;
            }
        }
        return false;
    }

    void GenerateQuests()
    {
        currentQuests.Clear();
        HashSet<int> uniqueQuests = new HashSet<int>();
        HashSet<Quest.Type> usedQuestTypes = new HashSet<Quest.Type>();

        for (int i = 0; i < 3; i++)
        {
            System.Random random = new System.Random();
            int value;
            Quest quest;

            do
            {
                value = random.Next(0, questsData.Get_Length);
                quest = questsData.Get_Quest(value);
            } while (uniqueQuests.Contains(value) || usedQuestTypes.Contains(quest.type));
            
            uniqueQuests.Add(value);
            usedQuestTypes.Add(quest.type);
            
            currentQuests.Add(value);
        }

        SetQuestsValues();
        QuestSpawner.instance.NoQuests.SetActive(false);
    }

    void GenerateGifts()
    {
        currentGifts.Clear();
        HashSet<int> uniqueGifts = new HashSet<int>();       

        for (int i = 0; i < 2; i++)
        {
            System.Random random = new System.Random();
            int value;            

            do
            {
                value = random.Next(0, questsData.Get_Length);                
            } while (uniqueGifts.Contains(value));

            uniqueGifts.Add(value);
            currentGifts.Add(value);
        }        
    }


    private void SetQuestsValues()
    {
        PlayerPrefs.SetInt("WinCount", 0);
        PlayerPrefs.SetInt("FireShots", 0);
        PlayerPrefs.SetInt("noMissShots", 0);
        PlayerPrefs.SetInt("readNotification", 0);
    }

    private void LoadQuestValue()
    {
        WinCount =  PlayerPrefs.GetInt("WinCount");
        FireShots = PlayerPrefs.GetInt("FireShots");
        noMissShots =  PlayerPrefs.GetInt("noMissShots");
        ReadNotification = PlayerPrefs.GetInt("readNotification");
    }

    public void CheckForQuestsAndGifts()
    {
        if (Has24HoursPassed())
        {
            GenerateQuests();
            GenerateGifts();
            SaveLogin();
            SetList("current_Quests", currentQuests);
            SetList("current_Gifts", currentGifts);
            SetQuestsValues();
        }
        else
        {
            LoadList("current_Quests", currentQuests);
            LoadList("current_Gifts", currentGifts);
            LoadQuestValue();
        }
    }

    public Quest.Type CheckQuestType(int index)
    {
        if (currentQuests.Contains(index))
        {
            Quest quest = questsData.Get_Quest(index);
            return quest.type;
        }
        return Quest.Type.none;
    }


   
    void setDefaultData()
    {
        
            PlayerPrefs.SetFloat("force", player_1.maxForce);
            PlayerPrefs.SetInt("health", Ships[0].GetComponent<Ship>().Health);

            PlayerPrefs.SetInt("extraSlot", shop.Got_Extra_Slot);

            PlayerPrefs.SetInt("slot1", uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index);
            PlayerPrefs.SetInt("slot2", uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index);
            PlayerPrefs.SetInt("slotExtra", uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index);

            PlayerPrefs.SetInt("level", Current_Level);

            PlayerPrefs.SetInt("coins", Coins);
            PlayerPrefs.SetInt("diamond", Diamond);

            PlayerPrefs.SetInt("fireUses", Fire_Uses);
            PlayerPrefs.SetInt("burstUses", Burst_Uses);

            PlayerPrefs.SetInt("levelHealth", upgrades.lvl_health);
            PlayerPrefs.SetInt("levelForce", upgrades.lvl_force);

            //acountStuff
            PlayerPrefs.SetString("username", "");
            PlayerPrefs.SetString("imgUrl", "");
            PlayerPrefs.SetString("token", "");

            PlayerPrefs.SetInt("totalWins", 0);
            PlayerPrefs.SetInt("totalLost", 0);
            PlayerPrefs.SetInt("totalShotsFired", 0);
            PlayerPrefs.SetInt("fireShotsHit", 0);
            PlayerPrefs.SetInt("totalShotsHit", 0);
            PlayerPrefs.SetInt("totalShotsMiss", 0);

            //player bullets
            SetList("bullets", shop.bullets.data);

            //skins
            SetList("ship_skins", shop.skins.Ships_Skins);
            SetList("sail_skins", shop.skins.Sail_Skins);
            SetList("flag_skins", shop.skins.Flag_Skins);
            SetList("cannon_skins", shop.skins.Cannon_Skins);
            SetList("anchor_skins", shop.skins.Anchors_Skins);
            SetList("helm_skins", shop.skins.Helm_Skins);

            //selected skin
            PlayerPrefs.SetInt("select_skin_ship", player_1._selectedShip);
            PlayerPrefs.SetInt("select_skin_sail", player_1._selectedSail);
            PlayerPrefs.SetInt("select_skin_flag", player_1._selectedFlag);
            PlayerPrefs.SetInt("select_skin_cannon", player_1._selectedCannon);
            PlayerPrefs.SetInt("select_skin_anchor", player_1._selectedAnchor);
            PlayerPrefs.SetInt("select_skin_helm", player_1._selectedHelm);

            SaveLastLogin();

            //current Quests
            GenerateQuests();
            SetList("current_Quests", currentQuests);

            //gifts
            GenerateGifts();
            SetList("current_Gifts", currentGifts);

            //power ups
            PlayerPrefs.SetInt("freeze", Freez_count);
            PlayerPrefs.SetInt("tinyShots", TinyShots_count);
            PlayerPrefs.SetInt("shield", Sheild_count);
            PlayerPrefs.SetInt("tinyShip", TinyShip_count);

            //tut
            PlayerPrefs.SetInt("tut", tut);

            //Audio
            PlayerPrefs.SetFloat("music", MusicVolume);
            PlayerPrefs.SetFloat("sound", SoundVolume);

            //end
            PlayerPrefs.SetInt("end", end);                
    }

    void LoadData()
    {
        
            player_1.maxForce = PlayerPrefs.GetFloat("force");
            Ships[0].GetComponent<Ship>().Health = PlayerPrefs.GetInt("health");

            shop.Got_Extra_Slot = PlayerPrefs.GetInt("extraSlot");

            uI_Controller.bullet_slot_1.GetComponent<Bullet_Slot>().index = PlayerPrefs.GetInt("slot1");
            uI_Controller.bullet_slot_2.GetComponent<Bullet_Slot>().index = PlayerPrefs.GetInt("slot2");
            uI_Controller.bullet_slot_Extra.GetComponent<Bullet_Slot>().index = PlayerPrefs.GetInt("slotExtra");

            Current_Level = PlayerPrefs.GetInt("level");

            Coins = PlayerPrefs.GetInt("coins");
            Diamond = PlayerPrefs.GetInt("diamond");

            Fire_Uses = PlayerPrefs.GetInt("fireUses");
            Burst_Uses = PlayerPrefs.GetInt("burstUses");

            upgrades.lvl_health = PlayerPrefs.GetInt("levelHealth");
            upgrades.lvl_force = PlayerPrefs.GetInt("levelForce");


            totalMatchWin = PlayerPrefs.GetInt("totalWins");
            totalMatchLost = PlayerPrefs.GetInt("totalLost");
            TotalShotsFired = PlayerPrefs.GetInt("totalShotsFired");
            FireShotsHit = PlayerPrefs.GetInt("fireShotsHit");
            TotalShotsHit = PlayerPrefs.GetInt("totalShotsHit");
            TotalShotsMiss = PlayerPrefs.GetInt("totalShotsMiss");

            //Player Bullets
            LoadList("bullets", shop.bullets.data);

            //skins
            LoadList("ship_skins", shop.skins.Ships_Skins);
            LoadList("sail_skins", shop.skins.Sail_Skins);
            LoadList("flag_skins", shop.skins.Flag_Skins);
            LoadList("cannon_skins", shop.skins.Cannon_Skins);
            LoadList("anchor_skins", shop.skins.Anchors_Skins);
            LoadList("helm_skins", shop.skins.Helm_Skins);


            //selected skins
            player_1._selectedShip = PlayerPrefs.GetInt("select_skin_ship");
            player_1._selectedSail = PlayerPrefs.GetInt("select_skin_sail");
            player_1._selectedFlag = PlayerPrefs.GetInt("select_skin_flag");
            player_1._selectedCannon = PlayerPrefs.GetInt("select_skin_cannon");
            player_1._selectedAnchor = PlayerPrefs.GetInt("select_skin_anchor");
            player_1._selectedHelm = PlayerPrefs.GetInt("select_skin_helm");

            //quests and gifts
            CheckForQuestsAndGifts();


            //power ups
            Freez_count = PlayerPrefs.GetInt("freeze");
            TinyShots_count = PlayerPrefs.GetInt("tinyShots");
            Sheild_count = PlayerPrefs.GetInt("shield");
            TinyShip_count = PlayerPrefs.GetInt("tinyShip");

            //tut
            tut = PlayerPrefs.GetInt("tut");

            //Audio
            MusicVolume = PlayerPrefs.GetFloat("music");
            SoundVolume = PlayerPrefs.GetFloat("sound");

            end = PlayerPrefs.GetInt("end");
       
        
    }

    public void SetList(string key, List<int> list)
    {
        if (key.Contains("skins"))
        {
            list.Add(0);
        }        
        string _list = string.Join(",", list.ConvertAll(i => i.ToString()).ToArray());
        PlayerPrefs.SetString(key, _list);
    }

    private void LoadList(string key, List<int> list)
    {
        string intListString = PlayerPrefs.GetString(key);

        if (!string.IsNullOrEmpty(intListString))
        {
            string[] stringArray = intListString.Split(',');
            list.Clear();
            foreach (string str in stringArray)
            {
                int value;
                if (int.TryParse(str, out value))
                {
                    list.Add(value);
                }
            }
        }
    }


    
    public void RevivePlayer()
    {
        Reset_Ships(true);
        MusicSource.Play();
        OceanBackGround.Play();
        UI_Controller.instance.Block.SetActive(false);
        UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(true);
        UI_Controller.instance.Win_Tigger(0);
    }

    public void OpenSheild()
    {
        SheildObj.SetActive(true);
        Animator anim = SheildObj.GetComponent<Animator>();
        anim.SetTrigger("open");
        UncheckPowerUps();
        current_Usage_Sheild++;
        Sheild_count -= 1;
        UI_Controller.instance.SetPowerUpsCount();
        SaveData("shield", Sheild_count);
        PlayAudio(Soundeffects.SheildSound);
    }

    public void Freez()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            GameObject iceCube = Instantiate(FreezObj, bullets[i].transform.position, bullets[i].transform.rotation);            
            bullets[i].transform.SetParent(iceCube.transform);

            SpriteRenderer sr = bullets[i].GetComponent<SpriteRenderer>();
            sr.sortingOrder = 0;


            Rigidbody2D bulletRb = bullets[i].GetComponent<Rigidbody2D>();
            bulletRb.velocity *= 0.2f;

            Collider2D col = bullets[i].GetComponent<Collider2D>();
            col.isTrigger = true;

            Rigidbody2D iceRb = iceCube.GetComponent<Rigidbody2D>();
            iceRb.velocity = bulletRb.velocity;            
            bulletRb.isKinematic = true;            
            bullets[i].tag = "Finish";
            
            ParticleSystem particleSystem = bullets[i].GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
        UncheckPowerUps();
        current_Usage_freeze++;
        Freez_count--;
        UI_Controller.instance.SetPowerUpsCount();
        SaveData("freeze", Freez_count);
        PlayAudio(Soundeffects.FreezeSound);        
        Invoke(nameof(check_After), 2);
    }

    void check_After()
    {
        if (!isChecking)
        {
            isChecking = true;
            Check_Turn();
        }
    }

    public void TinyShots()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            Vector3 scale = bullets[i].transform.localScale;
            bullets[i].transform.DOScale(new Vector3(scale.x / 2, scale.y / 2, scale.z), 0.2f);

            Bullet bullet = bullets[i].GetComponent<Bullet>();
            bullet.Damage /= 2;

            ParticleSystem particleSystem = bullets[i].GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                Vector3 scalePart = particleSystem.gameObject.transform.localScale;
                particleSystem.gameObject.transform.DOScale(new Vector3(scalePart.x / 2, scalePart.y / 2, scalePart.z), 0.2f);
            }
        }
        UncheckPowerUps();
        current_Usage_TinyShots++;
        TinyShots_count--;
        UI_Controller.instance.SetPowerUpsCount();
        SaveData("tinyShots", TinyShots_count);
        PlayAudio(Soundeffects.ShrinkSound);
    }

    public void TinyShip()
    {
        UncheckPowerUps();
        Ship ship = Ships[0].GetComponent<Ship>();
        ship.Floating = false;
        PlayerShip.transform.DOLocalMove(new Vector3(PlayerShip.transform.localPosition.x, -2.5f, PlayerShip.transform.localPosition.z), 0.1f);        
        current_Usage_TinyShip++;
        TinyShip_count--;
        PlayerShip.transform.DOScale(new Vector3(2, 2, 1), 0.3f);
        UI_Controller.instance.SetPowerUpsCount();
        SaveData("tinyShip", TinyShip_count);
        PlayAudio(Soundeffects.ShrinkSound);
    }

    public enum GamePhase
    {
        MainMenu,
        Start,
        ReadyPhase,
        PlayerShootPhase,
        EnemyReadyPhase,
        EnemyShootPhase,
        GameOver,
    }

    public void ShowTut()
    {
        if(tut == 0)
        {            
            Invoke(nameof(startTutInvoke), 0.5f);
        }
        else
        {
            UI_Controller.instance.Main_Menu.DOFade(1, 0.3f);
            UI_Controller.instance.Main_Menu.interactable = true;
            UI_Controller.instance.Main_Menu.blocksRaycasts = true;
            TutObj.SetActive(false);
        }
        SetVolume(1);
    }

    void startTutInvoke()
    {
        UI_Controller.instance.Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            cam_.GetComponent<CameraFollow>().SetTarget(null);
            cam_.transform.DOMove(End_Point.localPosition, 5).OnComplete(() =>
            {
                UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(true);                
                phase = GamePhase.ReadyPhase;
                player_1.enabled = true;
                player_1.GetComponent<Rotate_Object>().enabled = true;
                player_2.enabled = false;
                cam_.GetComponent<CinemachineBrain>().enabled = true;
                cam.gameObject.SetActive(true);
                cam.Follow = player_1.transform.parent.transform;
                captain.DOFade(1, 0.3f);
                SkipNextDia();
            });
        });
        UI_Controller.instance.Menu_BG.interactable = false;
        UI_Controller.instance.Menu_BG.blocksRaycasts = false;
        Set_Player();
        Reset_Ships(false);
        player_1.GetAllSkins();
        player_2.Get_Stats(Current_Level);
        turn = 2;
        MissShot = false;
        UI_Controller.instance.SetPlayerStats();
        Coins_Start = Coins;                
    }

    void AnimateTut(Transform thing)
    {
        thing.gameObject.SetActive(true);
        var Squence = DOTween.Sequence();
        CanvasGroup group = thing.gameObject.GetComponent<CanvasGroup>();
        Squence.Append(thing.DOLocalMove(new Vector2(-185, -30), 1));
        Squence.Append(group.DOFade(0, 0.3f));
        Squence.Append(thing.DOScale(0, 0.1f));
        Squence.Append(thing.DOLocalMove(new Vector2(0, 0), 0.1f));
        Squence.Append(group.DOFade(1, 0.3f));
        Squence.Append(thing.DOScale(2, 0.2f));
        Squence.SetLoops(-1, LoopType.Restart);
        Squence.Play();
    }
    int DiaSkipped;
    public void SkipNextDia()
    {
        if(DiaSkipped == 0)
        {
            StartCoroutine(TypeSentence("Ahoy matey, new recruit! Welcome to the crew of the fiercest pirate ship on the high seas!"));
            PlayAudio(Soundeffects.AhoyMatey);
        }

        else if(DiaSkipped == 1)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence("You�ve got the spirit of a true pirate, and today we�ll see if you have the skill!"));
            PlayAudio(Soundeffects.Ohoy);
        }            
        else if(DiaSkipped == 2)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence3("The enemy ship approaches! Ready the cannons for battle! (Press 'Ready' to prepare your aim)"));            
            captain.interactable = false;
            captain.blocksRaycasts = true;            
        }
        else if(DiaSkipped == 3)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence2("Now, hold and drag the screen to aim your cannon..."));
            MusicSource.clip = Soundeffects.BattleMusic;
            MusicSource.DOFade(MusicVolume, 2);
            MusicSource.Play();
        }
        else if (DiaSkipped == 4)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence2("Perfect! Now, release to shoot and send those scallywags to the depths!"));
            PlayAudio(Soundeffects.ScallyWags);
        }
        else if(DiaSkipped == 5)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence2("FIRE!!"));
            PlayAudio(Soundeffects.Fire[0]);
            
            Invoke(nameof(RemoveTut), 2);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence2($"Enjoy playing"));
        }
        DiaSkipped++;
    }

    public void ReadyTutFunc()
    {
        AnimateTut(Pointer);
        ReadyTut.DOFade(0, 0.3f);
        ReadyTut.interactable = false;
        ReadyTut.blocksRaycasts = false;
        MusicSource.DOFade(0, 1);
        SkipNextDia();
    }

    void RemoveTut()
    {
        captain.DOFade(0, 0.3f).OnComplete(() => {
            TutObj.SetActive(false);
        });
        captain.interactable = false;
    }

    public IEnumerator TypeSentence(string sentence)
    {
        tutText.text = "";
        captain.interactable = false;
        foreach (char letter in sentence.ToCharArray())
        {
            tutText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        captain.interactable = true;
    }

    public IEnumerator TypeSentence2(string sentence)
    {
        tutText.text = "";
        captain.interactable = false;
        foreach (char letter in sentence.ToCharArray())
        {
            tutText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }        
    }

    IEnumerator TypeSentence3(string sentence)
    {
        tutText.text = "";
        captain.interactable = false;
        foreach (char letter in sentence.ToCharArray())
        {
            tutText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        ReadyTut.DOFade(1, 0.3f);
        ReadyTut.interactable = true;
        ReadyTut.blocksRaycasts = true;
    }

    public IEnumerator TypeSentenceEnd(string sentence)
    {
        tutText.text = "";
        Next.GetComponentInChildren<TMPro.TMP_Text>().text = "End";
        captain.interactable = false;
        foreach (char letter in sentence.ToCharArray())
        {
            tutText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        captain.interactable = true;
        Next.onClick.RemoveAllListeners();
        Next.onClick.AddListener(() => RemoveTut());
    }

    
    public void Congrats()
    {
        Next.GetComponentInChildren<TMPro.TMP_Text>().text = "End";
        TutObj.SetActive(true);
        captain.DOFade(1, 0.3f);
        captain.interactable = true;
        captain.blocksRaycasts = true;
        StartCoroutine(TypeSentenceEnd("Congratulations, Captain! You've conquered the seas and completed your pirate adventure!"));
        Next.onClick.RemoveAllListeners();
        Next.onClick.AddListener(() => RemoveTut());
        SaveData("end", 1);
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://haddajidev.vercel.app/");
    }
    public void IconFinder()
    {
        Application.OpenURL("https://www.iconfinder.com/");
    }

    public void GitHub()
    {
        Application.OpenURL("https://github.com/HaddajiDev/Pirate-s-Dual");
    }

    public void SetVolume(int index)
    {
        if(index == 0)
        {
            MusicSlider.value = 0;
            SoundSlider.value = 0;
            OceanBackGround.volume = 0;
            MusicSource.volume = 0;
        }
        else
        {
            MusicSlider.value = MusicVolume;
            SoundSlider.value = SoundVolume;
            MusicSource.volume = 0;
            MusicSource.DOFade(MusicVolume, 1f);
            OceanBackGround.DOFade(SoundVolume, 1f);
        }
    }

    public void SetMusicVolume()
    {
        MusicSource.volume = MusicSlider.value;
        MusicVolume = MusicSlider.value;
        SaveData("music", MusicSlider.value);
    }
    public void SetSoundVolume()
    {
        OceanBackGround.volume = SoundSlider.value;
        SoundVolume = SoundSlider.value;
        SaveData("sound", SoundSlider.value);
    }

    public void PlayAudio(AudioClip effect)
    {        
        AudioSource source = Instantiate(AudioInstance).GetComponent<AudioSource>();
        source.clip = effect;
        source.volume = SoundVolume;
        source.Play();        
    }

    [Header("Skins Cover")]
    public UI_Animator[] SkinsCover;
    
    public void SkinCoverAnimation()
    {
        for (int i = 0; i < SkinsCover.Length; i++)
        {
            SkinsCover[i].Func_PlayUIAnim();
        }        
    }

    public GetSelectedSkin[] selectedSkin;
    public void GetCoverSkin()
    {         
        for (int i = 0; i < selectedSkin.Length; i++)
        {
            selectedSkin[i].Get_Skin();
        }
    }
}