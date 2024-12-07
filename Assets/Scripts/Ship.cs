using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour
{
    public static Ship Instance;
    float yPos, pos;
    bool cap;
    public GameObject Explode_Effect;
    public GameObject Fire_Effect;
    public int Health = 10;
    int Current_Health;

    public Vector3 Start_Pos;

    public bool player;
    public Animator Sail_Anim;
    public Animator Ship_helm;
    int hit = 0;

    Animator anim;

    public CanvasGroup HealthBar_Player;
    public CanvasGroup HealthBar_Enemy;
    public GameObject Destroyed_Ship;
    public Transform Destroy_Point;

    public bool Floating = true;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        cap = true;
        pos = transform.localPosition.y;
        yPos = transform.localPosition.y;       

        Start_Pos = transform.localPosition;
        anim = GetComponent<Animator>();                
    }

    public void Get_Health()
    {
        if (player)
            Get_Player_Health();
        else
            Get_Enemy_Health();

        if (player)
        {
            Slider slider = HealthBar_Player.GetComponentInChildren<Slider>();
            slider.maxValue = Current_Health;
            slider.value = Current_Health;
        }
        else
        {
            Slider slider = HealthBar_Enemy.GetComponentInChildren<Slider>();
            slider.maxValue = Current_Health;
            slider.value = Current_Health;
        }
    }
    
    void Update()
    {
        if (Floating)
        {
            if (cap)
            {
                yPos -= Time.deltaTime;
                if (yPos <= pos - 0.2f)
                {
                    cap = false;
                }
            }
            if (!cap)
            {
                yPos += Time.deltaTime;
                if (yPos >= pos + 0.2f)
                {
                    cap = true;
                }
            }
            transform.localPosition = new Vector3(transform.localPosition.x, yPos, 0);
        }               
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.tag = "Untagged";
            Instantiate(Explode_Effect, collision.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            anim.SetTrigger("Hit");

            hit++;
            if(hit == 1)
            {
                Invoke(nameof(Check_Turns), 1);
            }

            Take_Damage(collision.gameObject.GetComponent<Bullet>().Damage);

            if (collision.gameObject.GetComponent<Bullet>().transform.childCount != 0)
            {
                Instantiate(Fire_Effect, collision.transform.position, Quaternion.identity, gameObject.transform);
                collision.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                if (!player)
                {
                    GameManager.Instance.Coins += player ? Random.Range(5, 11) : 0;

                    //Quests
                    GameManager.Instance.FireShots++;
                    GameManager.Instance.SaveData("FireShots", GameManager.Instance.FireShots);
                    for (int i = 0; i < QuestSpawner.instance.questsBanners.Count; i++)
                    {
                        QuestSpawner.instance.questsBanners[i].UpdateStats();
                        if(GameManager.Instance.CheckQuestType(QuestSpawner.instance.questsBanners[i].index) == Quest.Type.fireShots)
                        {
                            QuestSpawner.instance.SpawnQuestPopUp(QuestPopUp.Type.fireShots, QuestSpawner.instance.questsBanners[i].index);
                        }
                    }                   

                    GameManager.Instance.FireShotsHit++;
                    GameManager.Instance.SaveData("fireShotsHit", GameManager.Instance.FireShotsHit);
                }                
            }
            
            
            //Coins (Damage)
            if (!player)
            {
                GameManager.Instance.Coins += Random.value < 0.85f ? Random.Range(50, 81) * (GameManager.Instance.Current_Level / 2)
                    : Random.Range(40, 61) * (GameManager.Instance.Current_Level / 2);
                GameManager.Instance.TotalShotsHit++;
                GameManager.Instance.SaveData("totalShotsHit", GameManager.Instance.TotalShotsHit);
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.hit[Random.Range(0, GameManager.Instance.Soundeffects.hit.Length)]);                
            }
            else
            {
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.getHit[Random.Range(0, GameManager.Instance.Soundeffects.getHit.Length)]);
            }
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Explode);
        }
    }

    void Take_Damage(int Damage)
    {        
        if (player)
        {
            Current_Health -= Damage;
            HealthBar_Player.DOFade(1, 0.5f);
            Slider slider = HealthBar_Player.GetComponentInChildren<Slider>();
            slider.value = Current_Health;
            
            if (Current_Health <= 0)
            {
                gameObject.SetActive(false);
                if (player)
                {
                    UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(false);
                    Invoke(nameof(Win_Obj_Lose), 3);
                    GameObject DestroyedShip = Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity);
                    Destroy_Effect fo = DestroyedShip.GetComponent<Destroy_Effect>();
                    fo.Ship = true;
                    UI_Controller.instance.Getting_Ready_Object.interactable = false;
                    UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
                    int value = Random.Range(0, 3);
                    if (value == 2)
                    {
                        GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.AbondShip);
                    }
                }
                else
                {
                    Invoke(nameof(Win_Obj_Win), 3);
                    GameObject DestroyedShip = Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity);
                    DestroyedShip.transform.localScale = new Vector3(-1, 1, 0);
                    Destroy_Effect fo = DestroyedShip.GetComponent<Destroy_Effect>();
                    fo.Ship = false;
                }
                GameManager.Instance.Reset_play();
                //audio
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.ShipCrash);
            }
        }
        else
        {
            Current_Health -= Damage;
            HealthBar_Enemy.DOFade(1, 0.5f);
            Slider slider = HealthBar_Enemy.GetComponentInChildren<Slider>();
            slider.value = Current_Health;
            if (Current_Health <= 0)
            {
                gameObject.SetActive(false);
                if (player)
                {
                    UI_Controller.instance.Getting_Ready_Object.gameObject.SetActive(false);
                    Invoke(nameof(Win_Obj_Lose), 3);
                    GameObject DestroyedShip = Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity);
                    Destroy_Effect fo = DestroyedShip.GetComponent<Destroy_Effect>();
                    fo.Ship = true;
                    UI_Controller.instance.Getting_Ready_Object.interactable = false;
                    UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
                }
                else
                {
                    Invoke(nameof(Win_Obj_Win), 3);
                    GameObject DestroyedShip = Instantiate(Destroyed_Ship, Destroy_Point.position, Quaternion.identity);
                    DestroyedShip.transform.localScale = new Vector3(-1, 1, 0);
                    Destroy_Effect fo = DestroyedShip.GetComponent<Destroy_Effect>();
                    fo.Ship = false;
                }
                GameManager.Instance.Reset_play();
                //audio
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.ShipCrash);
            }
        }
        
        
    }

    void Win_Obj_Win()
    {
        UI_Controller.instance.Win_Tigger(1, "You Win");
        GameManager.Instance.phase = GameManager.GamePhase.GameOver;

        //some gems
        int value = Random.Range(0, 3) == 2 ? Random.Range(3, 21) : 0;
        
        //more coins
        GameManager.Instance.Coins += Random.Range(50, 81) * (GameManager.Instance.Current_Level / 2);
        UI_Controller.instance.Coins_text.text = (GameManager.Instance.Coins - GameManager.Instance.Coins_Start).ToString();
        
        
        if(GameManager.Instance.Current_Level < GameManager.Instance.player_2.levels.Get_Lenght - 1)
        {
            GameManager.Instance.Current_Level++;
            GameManager.Instance.SaveData("level", GameManager.Instance.Current_Level);
        }
        //Quests
        GameManager.Instance.WinCount++;
        GameManager.Instance.SaveData("WinCount", GameManager.Instance.WinCount);
        for (int i = 0; i < QuestSpawner.instance.questsBanners.Count; i++)
        {
            QuestSpawner.instance.questsBanners[i].UpdateStats();
            if (GameManager.Instance.CheckQuestType(QuestSpawner.instance.questsBanners[i].index) == Quest.Type.wins)
            {
                QuestSpawner.instance.SpawnQuestPopUp(QuestPopUp.Type.wins, QuestSpawner.instance.questsBanners[i].index);
            }
        }

        if (!GameManager.Instance.MissShot)
        {
            GameManager.Instance.noMissShots++;
            GameManager.Instance.SaveData("noMissShots", GameManager.Instance.noMissShots);
            for (int i = 0; i < QuestSpawner.instance.questsBanners.Count; i++)
            {
                QuestSpawner.instance.questsBanners[i].UpdateStats();
                if (GameManager.Instance.CheckQuestType(QuestSpawner.instance.questsBanners[i].index) == Quest.Type.noMissShots)
                {
                    QuestSpawner.instance.SpawnQuestPopUp(QuestPopUp.Type.noMissShots, QuestSpawner.instance.questsBanners[i].index);
                }
            }
        }
        
        if(value > 0)
        {
            UI_Controller.instance.DiammondWinObj.SetActive(true);
            UI_Controller.instance.Diammind_Win.text = $"{value}";
            GameManager.Instance.Diamond += value;
            GameManager.Instance.SaveData("diamond", GameManager.Instance.Diamond);
        }
        else
        {
            UI_Controller.instance.DiammondWinObj.SetActive(false);
        }

        //coins
        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);

        //stats
        GameManager.Instance.totalMatchWin++;
        GameManager.Instance.SaveData("totalWins", GameManager.Instance.totalMatchLost);        
        UI_Controller.instance.SetPlayerStats();

        //audio
        GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Laugh);
        GameManager.Instance.MusicSource.DOFade(0, 0.5f);
    }


    void Win_Obj_Lose()
    {
        UI_Controller.instance.Win_Tigger(1, "You Lost");
        UI_Controller.instance.Coins_text.text = (GameManager.Instance.Coins - GameManager.Instance.Coins_Start).ToString();
        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);
        GameManager.Instance.phase = GameManager.GamePhase.GameOver;

        //stats
        GameManager.Instance.totalMatchLost++;
        GameManager.Instance.SaveData("totalLost", GameManager.Instance.totalMatchLost);
        UI_Controller.instance.SetPlayerStats();

        UI_Controller.instance.SetCurrencyUI();
        GameManager.Instance.SaveData("coins", GameManager.Instance.Coins);

        UI_Controller.instance.DiammondWinObj.SetActive(false);

        //audio
        GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Loose);        
        GameManager.Instance.MusicSource.DOFade(0, 0.5f);
    }

    void Fade_Out_Ready_Obj()
    {
        UI_Controller.instance.Getting_Ready_Object.DOFade(0, 0.2f);
        UI_Controller.instance.Getting_Ready_Object.interactable = false;
        UI_Controller.instance.Getting_Ready_Object.blocksRaycasts = false;
    }

    void Check_Turns()
    {
        Manager_Turn();
        if (player)
        {
            if (-transform.position.x >= -Start_Pos.x + 7 || -transform.position.x <= -Start_Pos.x + 7)
                Invoke(nameof(Move), 0.5f);            
        }
        else
        {
            if (transform.position.x >= Start_Pos.x + 7)
                Invoke(nameof(Move), 0.5f);
        }
        
    }

    void Manager_Turn()
    {
        hit = 0;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();            
        }
        Invoke(nameof(Health_Bar_Out), 3);
    }

    void Health_Bar_Out()
    {
        if (player)
        {
            HealthBar_Player.DOFade(0, 0.5f);
        }
        else
        {
            HealthBar_Enemy.DOFade(0, 0.5f);
        }
    }

    public void Move()
    {
        Sail_Anim.SetTrigger("toWind");
        Ship_helm.SetTrigger("move");
        transform.DOLocalMove(new Vector3(Start_Pos.x, Start_Pos.y, 0), 2).OnComplete(() =>
        {
            Sail_Anim.SetTrigger("toNoWind");
        });
    }


    public void Get_Player_Health()
    {
        Current_Health = Health;
    }

    void Get_Enemy_Health()
    {
        Enemy_AI enemy = GetComponent<Enemy_AI>();
        Level lvl = enemy.levels.Get_Level(GameManager.Instance.Current_Level);
        Health = lvl.Health;
        Current_Health = Health;
    }
}