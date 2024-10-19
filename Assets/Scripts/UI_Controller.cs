using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    [Header("UI Routes")]
    public CanvasGroup Getting_Ready_Object;
    public CanvasGroup Main_Menu;
    public CanvasGroup Extra_Slot_Buy_UI;
    public CanvasGroup Menu_BG; //Start Game
    public CanvasGroup Ready_Button;
    public CanvasGroup Bullets_Shop;
    public CanvasGroup Select_Bullets;
    public CanvasGroup Win_Obj;
    public CanvasGroup Buy_Upgrades;
    public CanvasGroup Buy_Skins;
    public CanvasGroup Show_Skin;
    public CanvasGroup Quests;
    public CanvasGroup Ad_FeedBack;
    public CanvasGroup Gifts;
    public CanvasGroup SelectBattleStrategies;
    public CanvasGroup Settings;
    public CanvasGroup Credits;
    public CanvasGroup Help;

    [Header("Select Bullet UI")]
    public Button Select_Bullet_1;
    public Button Select_Bullet_2;
    public Button Select_Bullet_Extra;
    public GameObject[] Checks;

    [Header("Abilites")]
    public Sprite Check;
    public GameObject Fire_Object;
    public GameObject Burst_Object;

    [Header("Power Ups")]
    public GameObject Sheild_Object;
    public GameObject Freez_Object;
    public GameObject TinyShots_Object;
    public GameObject TinyShip_Object;
    public GameObject All_PowerUps;
    public GameObject infernoAnimUI;
    public GameObject sheildAnimUI;     

    [Header("Texts")]
    public TMP_Text Coins_Main_Text;
    public TMP_Text Diamond_Text;
    public TMP_Text Coins_text;
    public TMP_Text State_Win;
    public TMP_Text Fire_Shot_Price;
    public TMP_Text Busrt_Shot_Price;
    public TMP_Text Level_Counter;
    public TMP_Text Bullet_Limit_1;
    public TMP_Text Bullet_Limit_2;
    public TMP_Text Bullet_Limit_Extra;
    public TMP_Text Total_Fire;
    public TMP_Text Total_Busrt;
    public TMP_Text Health_Cost_Upgrade_Coins;
    public TMP_Text Health_Cost_Upgrade_Diamond;
    public TMP_Text Force_Cost_Upgrade_Coins;    
    public TMP_Text Force_Cost_Upgrade__Diamond;
    public TMP_Text Diammind_Win;
    public TMP_Text Freeze_Count;
    public TMP_Text Freeze_Cost_Coins;
    public TMP_Text Freeze_Cost_gems;
    public TMP_Text TinyShots_Count;
    public TMP_Text TinyShots_Cost_Coins;
    public TMP_Text TinyShots_Cost_gems;
    public TMP_Text Sheild_Count;
    public TMP_Text Sheild_Cost_Coins;
    public TMP_Text Sheild_Cost_gems;
    public TMP_Text TinyShip_Count;
    public TMP_Text TinyShip_Cost_Coins;
    public TMP_Text TinyShip_Cost_gems;    

    [Header("Bullets Slots")]
    public Image bullet_slot_1;
    public Image bullet_slot_2;
    public Image bullet_slot_Extra;
    public GameObject Extra_Bullet_Buy_Button;
    public Button Shop_Buy_Extra_Bullet;
    public Button Clear_Button_Bullet;

    [Header("Account Stuff")]
    public CanvasGroup account;
    public Image userImg;
    public TMP_Text username;
    public GameObject SignInButton;
    public GameObject AccountButton;
    public TMP_Text Stats;

    

    [Header("Skins Stuff")]
    public Transform skins_Container;



    [Header("Others")]
    public Transform playButton;
    public GameObject Block;
    public Sprite succes;
    public Sprite failure;
    public Image feed_sprite;
    public TMP_Text feed_text;
    public GameObject ReviveButton;
    public GameObject QuestNotification;
    public GameObject GiftsNotification;
    public GameObject DiammondWinObj;
    
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetPowerUpsCost();

        SetAbilitesCount();
        SetPowerUpsCount();
        SetCurrencyUI();
        SetPlayerStats();
        
        playButton.DOScale(new Vector3(1.05f, 1.05f, 1), 1)
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void Check_Fire()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        Image my_img = null;

        if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
        {
            Button clickedButton = selectedObject.GetComponent<Button>();
            my_img = clickedButton.transform.GetChild(0).GetComponentInChildren<Image>();

            if(GameManager.Instance.Fire_Uses == 0)
            {
                clickedButton.interactable = false;
                return;
            }
        }

        //Fire
        if (my_img.sprite == Check)
        {
            my_img.sprite = null;
            my_img.enabled = false;
            GameManager.Instance.player_1.inFire = false;
        }            
        else if (my_img.sprite == null)
        {
            my_img.sprite = Check;
            my_img.enabled = true;            
            GameManager.Instance.player_1.inFire = true;
        }
    }
    public void Check_Busrt()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        Image my_img = null;

        if (selectedObject != null && selectedObject.GetComponent<Button>() != null)
        {
            Button clickedButton = selectedObject.GetComponent<Button>();
            my_img = clickedButton.transform.GetChild(0).GetComponentInChildren<Image>();

            if (GameManager.Instance.Burst_Uses == 0)
            {
                clickedButton.interactable = false;
                return;
            }
        }

        //burst
        if (my_img.sprite == Check)
        {
            my_img.sprite = null;
            my_img.enabled = false;
            GameManager.Instance.player_1.burstCount = 1;
        }
        else if (my_img.sprite == null)
        {
            my_img.sprite = Check;
            my_img.enabled = true;
            GameManager.Instance.player_1.burstCount = 3;
        }
    }
    public void unCheckAbilities()
    {
        Image fire_img = Fire_Object.transform.GetChild(0).GetComponentInChildren<Image>();
        fire_img.sprite = null;
        fire_img.enabled = false;
        GameManager.Instance.player_1.inFire = false;

        Image burst_img = Burst_Object.transform.GetChild(0).GetComponentInChildren<Image>();
        burst_img.sprite = null;
        burst_img.enabled = false;
        GameManager.Instance.player_1.burstCount = 1;
    }
    public void Win_Tigger(int value, string state = "")
    {
        if(value == 1)
        {
            Win_Obj.interactable = true;
            Win_Obj.blocksRaycasts = true;
            Win_Obj.DOFade(1, 0.3f);
            State_Win.text = state;
            Getting_Ready_Object.DOFade(0, 0.1f);
            Getting_Ready_Object.interactable = false;
            Getting_Ready_Object.blocksRaycasts = false;
            if (state.ToLower().Contains("win"))
                State_Win.color = Color.green;
            else
            {
                State_Win.color = Color.red;
                //float randomValue = Random.Range(0.0f, 1.0f);

                //if (randomValue <= 0.4f)
                //{
                //    ReviveButton.SetActive(true);
                //}
            }
                
        }
        else
        {            
            Win_Obj.DOFade(0, 0.3f).OnComplete(() => {
                Win_Obj.interactable = false;
                Win_Obj.blocksRaycasts = false;
                ReviveButton.SetActive(false);
            });
        }        
    }

    public void Back_Main()
    {
        Win_Tigger(0);
        GameManager.Instance.cam.Follow = null;
        GameManager.Instance.cam.gameObject.SetActive(false);
        GameManager.Instance.cam_.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        GameManager.Instance.cam_.GetComponent<CameraFollow>().SetTarget(null);
        GameManager.Instance.cam_.transform.DOMove(GameManager.Instance.Main_Point.localPosition, 3).OnComplete(() => {
            Main_Menu.DOFade(1, 0.3f);
            Main_Menu.interactable = true;
            Main_Menu.blocksRaycasts = true;
            GameManager.Instance.phase = GameManager.GamePhase.MainMenu;            
            GameManager.Instance.MusicSource.clip = GameManager.Instance.Soundeffects.MainMenu;
            GameManager.Instance.MusicSource.DOFade(GameManager.Instance.MusicVolume, 1);
            GameManager.Instance.MusicSource.Play();
            Crab.instance.Escaped = false;
            if(PlayerPrefs.GetInt("tut") == 1)
            {
                GameManager.Instance.player_1.SelectBullet(0);
                GameManager.Instance.Play();
            }
            else
            {
                GameManager.Instance.SaveData("tut", 1);
                GameManager.Instance.TutObj.SetActive(true);
                GameManager.Instance.captain.DOFade(1, 0.3f);
                GameManager.Instance.captain.interactable = true;
                GameManager.Instance.captain.blocksRaycasts = true;
                StartCoroutine(GameManager.Instance.TypeSentenceEnd("This is just the start! There are 100 battles ahead. Prove yourself, and you'll rule the seas"));
            }
            if(GameManager.Instance.GetLevel == 100 && PlayerPrefs.GetInt("end") == 0)
            {
                GameManager.Instance.Congrats();
            }
        });
    }

    public void Bullets_Shop_Controller(int index)
    {
        if(index == 1)
        {
            Bullets_Shop.gameObject.SetActive(true);
            Bullets_Shop.interactable = true;
            Bullets_Shop.blocksRaycasts = true;
            Bullets_Shop.DOFade(1, 0.3f);
        }
        else
        {
            Bullets_Shop.DOFade(0, 0.3f).OnComplete(() => {                
                Bullets_Shop.gameObject.SetActive(false);
            });
            Bullets_Shop.interactable = false;
            Bullets_Shop.blocksRaycasts = false;
        }
    }

    public void Select_Bullet_Controller(int index)
    {
        if (index == 1)
        {
            Select_Bullets.gameObject.SetActive(true);
            Select_Bullets.interactable = true;
            Select_Bullets.blocksRaycasts = true;
            Select_Bullets.DOFade(1, 0.3f);
        }
        else
        {
            Select_Bullets.DOFade(0, 0.3f).OnComplete(() => {                
                Select_Bullets.gameObject.SetActive(false);
            });
            Select_Bullets.interactable = false;
            Select_Bullets.blocksRaycasts = false;
        }
    }

    public void Buy_Extra_Bullet_Slot(int index)
    {
        if (index == 1)
        {
            Extra_Slot_Buy_UI.gameObject.SetActive(true);
            Extra_Slot_Buy_UI.interactable = true;
            Extra_Slot_Buy_UI.blocksRaycasts = true;
            Extra_Slot_Buy_UI.DOFade(1, 0.3f);
        }
        else
        {
            Extra_Slot_Buy_UI.DOFade(0, 0.3f).OnComplete(() => {                
                Extra_Slot_Buy_UI.gameObject.SetActive(false);
            });
            Extra_Slot_Buy_UI.interactable = false;
            Extra_Slot_Buy_UI.blocksRaycasts = false;
        }        
    }

    public void Buy_Upgrades_Controller(int index)
    {
        if (index == 1)
        {
            Buy_Upgrades.gameObject.SetActive(true);
            Buy_Upgrades.interactable = true;
            Buy_Upgrades.blocksRaycasts = true;
            Buy_Upgrades.DOFade(1, 0.3f);
        }
        else
        {
            Buy_Upgrades.DOFade(0, 0.3f).OnComplete(() => {                
                Buy_Upgrades.gameObject.SetActive(false);
            });
            Buy_Upgrades.interactable = false;
            Buy_Upgrades.blocksRaycasts = false;
        }
    }
    public void Open_Account(int index)
    {
        if (index == 1)
        {
            account.gameObject.SetActive(true);
            account.interactable = true;
            account.blocksRaycasts = true;
            account.DOFade(1, 0.3f);
        }
        else
        {
            account.DOFade(0, 0.3f).OnComplete(() => {                
                account.gameObject.SetActive(false);
            });
            account.interactable = false;
            account.blocksRaycasts = false;
        }
    }

    public void Buy_Skins_Controller(int index)
    {
        if (index == 1)
        {
            Buy_Skins.gameObject.SetActive(true);
            GameManager.Instance.SkinCoverAnimation();
            Buy_Skins.interactable = true;
            Buy_Skins.blocksRaycasts = true;
            Buy_Skins.DOFade(1, 0.3f);
        }
        else
        {
            Buy_Skins.DOFade(0, 0.3f).OnComplete(() => {                
                Buy_Skins.gameObject.SetActive(false);
            });
            Buy_Skins.interactable = false;
            Buy_Skins.blocksRaycasts = false;
        }
    }

    public void Show_Skin_Controller(int index)
    {
        if (index == 1)
        {
            Show_Skin.gameObject.SetActive(true);
            Show_Skin.interactable = true;
            Show_Skin.blocksRaycasts = true;
            Show_Skin.DOFade(1, 0.3f);
        }
        else
        {
            Show_Skin.DOFade(0, 0.3f).OnComplete(() => {                
                Show_Skin.gameObject.SetActive(false);
            });
            Show_Skin.interactable = false;
            Show_Skin.blocksRaycasts = false;
        }
    }

    public void Show_Quests_Controller(int index)
    {
        if (index == 1)
        {
            GameManager.Instance.ReadNotification = 1;
            GameManager.Instance.SaveData("readNotification", GameManager.Instance.ReadNotification);            
            QuestNotification.SetActive(false);
            Quests.interactable = true;
            Quests.blocksRaycasts = true;
            Quests.DOFade(1, 0.3f);
        }
        else
        {
            Quests.DOFade(0, 0.3f).OnComplete(() => {
                
            });
            Quests.interactable = false;
            Quests.blocksRaycasts = false;
        }
    }

    public void Show_AdFeedback_Controller(int index)
    {
        if (index == 1)
        {
            Ad_FeedBack.gameObject.SetActive(true);
            Ad_FeedBack.interactable = true;
            Ad_FeedBack.blocksRaycasts = true;
            Ad_FeedBack.DOFade(1, 0.3f);
        }
        else
        {
            Ad_FeedBack.DOFade(0, 0.3f).OnComplete(() => {                
                Ad_FeedBack.gameObject.SetActive(false);
            });
            Ad_FeedBack.interactable = false;
            Ad_FeedBack.blocksRaycasts = false;
        }
    }

    public void Show_SelectPowerUps_Controller(int index)
    {
        if (index == 1)
        {            
            SelectBattleStrategies.gameObject.SetActive(true);
            UI_Animator infernoAnim = infernoAnimUI.GetComponentInChildren<UI_Animator>();
            infernoAnim.Func_PlayUIAnim();
            UI_Animator SheildAnim = sheildAnimUI.GetComponentInChildren<UI_Animator>();
            SheildAnim.Func_PlayUIAnim();
            SelectBattleStrategies.interactable = true;
            SelectBattleStrategies.blocksRaycasts = true;
            SelectBattleStrategies.DOFade(1, 0.3f);            
        }
        else
        {
            SelectBattleStrategies.DOFade(0, 0.3f).OnComplete(() => {                
                SelectBattleStrategies.gameObject.SetActive(false);
            });
            SelectBattleStrategies.interactable = false;
            SelectBattleStrategies.blocksRaycasts = false;
        }
    }

    public void FeedBackPopUp(string text, FeedbackType type)
    {
        Block.SetActive(false);
        Show_AdFeedback_Controller(1);
        feed_text.text = text;
        if(type == FeedbackType.failed)
        {
            feed_sprite.sprite = failure;
            feed_sprite.color = Color.red;
        }
        else if(type == FeedbackType.succes)
        {
            feed_sprite.sprite = succes;
            feed_sprite.color = Color.green;            
        }        
    }

    public void Show_Gifts_Controller(int index)
    {
        if (index == 1)
        {            
            Gifts.interactable = true;
            Gifts.blocksRaycasts = true;
            Gifts.DOFade(1, 0.3f);
        }
        else
        {
            Gifts.DOFade(0, 0.3f).OnComplete(() => {
                
            });
            Gifts.interactable = false;
            Gifts.blocksRaycasts = false;
        }
    }

    public void Show_Settings_Controller(int index)
    {
        if (index == 1)
        {
            Settings.gameObject.SetActive(true);
            Settings.interactable = true;
            Settings.blocksRaycasts = true;
            Settings.DOFade(1, 0.3f);
        }
        else
        {
            Settings.DOFade(0, 0.3f).OnComplete(() => {                
                Settings.gameObject.SetActive(false);
            });
            Settings.interactable = false;
            Settings.blocksRaycasts = false;
        }
    }

    public void Show_Credits_Controller(int index)
    {
        if (index == 1)
        {
            Credits.gameObject.SetActive(true);
            Credits.interactable = true;
            Credits.blocksRaycasts = true;
            Credits.DOFade(1, 0.3f);
        }
        else
        {
            Credits.DOFade(0, 0.3f).OnComplete(() => {                
                Credits.gameObject.SetActive(false);
            });
            Credits.interactable = false;
            Credits.blocksRaycasts = false;
        }
    }

    public void Show_Help_Controller(int index)
    {
        if (index == 1)
        {
            Help.gameObject.SetActive(true);
            Help.interactable = true;
            Help.blocksRaycasts = true;
            Help.DOFade(1, 0.3f);
        }
        else
        {
            Help.DOFade(0, 0.3f).OnComplete(() => {                
                Help.gameObject.SetActive(false);
            });
            Help.interactable = false;
            Help.blocksRaycasts = false;
        }
    }


    public void Close_Start_Menu()
    {
        Menu_BG.DOFade(0, 0.3f).OnComplete(() =>
        {
            Menu_BG.interactable = false;
            Menu_BG.blocksRaycasts = false;
        });
    }

    public void ResetChecks()
    {
        for (int i = 0; i < Checks.Length; i++)
        {
            Checks[i].SetActive(false);
        }
    }

    public void SetCurrencyUI()
    {
        Coins_Main_Text.text = GameManager.Instance.Coins.ToString();
        Diamond_Text.text = GameManager.Instance.Diamond.ToString();
    }
    public void SetAbilitesCount()
    {
        Total_Fire.text = $"You have : " + GameManager.Instance.Fire_Uses.ToString();
        Total_Busrt.text = $"You have : " + GameManager.Instance.Burst_Uses.ToString();
    }

    public void SetPowerUpsCount()
    {
        Freeze_Count.text = $"You have : {GameManager.Instance.Freez_count}";
        TinyShots_Count.text = $"You have : {GameManager.Instance.TinyShots_count}";
        Sheild_Count.text = $"You have : {GameManager.Instance.Sheild_count}";
        TinyShip_Count.text = $"You have : {GameManager.Instance.TinyShip_count}";
    }

    private void SetPowerUpsCost()
    {
        Fire_Shot_Price.text = GameManager.Instance.Fire_Cost.Coins.ToString();
        Busrt_Shot_Price.text = GameManager.Instance.Burst_Cost.Coins.ToString();

        Freeze_Cost_Coins.text = $"{GameManager.Instance.Freez_cost.Coins}";
        Freeze_Cost_gems.text = $"{GameManager.Instance.Freez_cost.Diamond}";

        TinyShots_Cost_Coins.text = $"{GameManager.Instance.TinyShots_cost.Coins}";
        TinyShots_Cost_gems.text = $"{GameManager.Instance.TinyShots_cost.Diamond}";

        Sheild_Cost_Coins.text = $"{GameManager.Instance.Sheild_cost.Coins}";
        Sheild_Cost_gems.text = $"{GameManager.Instance.Sheild_cost.Diamond}";

        TinyShip_Cost_Coins.text = $"{GameManager.Instance.TinyShip_cost.Coins}";
        TinyShip_Cost_gems.text = $"{GameManager.Instance.TinyShip_cost.Diamond}";
    }

    public void SetPlayerStats()
    {
        Stats.text =
            $"Stats \n" +
            $"Total Games Win : {GameManager.Instance.totalMatchWin} \n" +
            $"Total Games Lost : {GameManager.Instance.totalMatchLost} \n" +
            $"Total Shots Fired : {GameManager.Instance.TotalShotsFired} \n" +
            $"Total Shots Missed : {GameManager.Instance.TotalShotsMiss} \n" +
            $"Total Shots Hit : {GameManager.Instance.TotalShotsHit} \n" +
            $"Total Fire Shots Hit : {GameManager.Instance.FireShotsHit}";
    }

    public void PlaySkinAnimation(GameObject parent)
    {
        UI_Animator[] _animator = parent.GetComponentsInChildren<UI_Animator>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (_animator[i].gameObject.activeInHierarchy)
                _animator[i].Func_PlayUIAnim();
        }        
    }
    public void PlaySkinAnimation2(GameObject parent)
    {
        UI_Animator[] _animator = parent.GetComponentsInChildren<UI_Animator>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (_animator[i].gameObject.activeInHierarchy) 
                _animator[i].Func_PlayUIAnim();
        }
    }

    public void ExitSkin()
    {
        foreach(Transform child in skins_Container)
        {
            if (child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(false);
        }        
    }


    public enum FeedbackType
    {
        succes,
        failed
    }
}
