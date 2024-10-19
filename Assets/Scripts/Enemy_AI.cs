using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Enemy_AI : MonoBehaviour
{
    public Level_Data levels;
    private GameObject BulletPrefab;
    
    public Transform shootPoint;
    private float MaxshootForce = 30f;
    private float MinshootForce = 50f;

    
    private float Min_Angle = 10;
    private float Max_Angle = 60;
    
    private bool Can_Fire = false;
    [HideInInspector] public bool Fire = false;

    private bool canUsePowerUps = false;

    private int Burst_Count;

    private int DamageMin;
    private int DamageMax;

    public Transform Canon;
    Animator anim;

    CinemachineImpulseSource source;
    [Header("Cosmatic")]
    public ShipCosmaticData shipCosmatic;    
    public Animator Ship;

    [Space]
    public CanonSkins CannonCosmatic;    
    public Animator Cannon;
    public SpriteRenderer Stand;

    [Space]
    public SailCosmaticData sailCosmatic;    
    public Animator sail;

    [Space]
    public HelmCosmaticData helmCosmatic;    
    public Animator helm;

    [Space]
    public FlagCosmaticData flagCosmatic;    
    public Animator Flag;

    [Space]
    public AnchorCosmaticData anchorCosmatic;   
    public SpriteRenderer[] anchors;

    public GameObject SheildObj;
    int current_Usage_Sheild;

    int current_Usage_freeze;

    int current_Usage_TinyShots;

    int current_Usage_TinyShip;

    public GameObject[] EnemyShots;
    private void Start()
    {
        anim = Canon.gameObject.GetComponent<Animator>();
        source = GetComponent<CinemachineImpulseSource>();        
    }

    void Shoot()
    {
        GameManager.Instance.phase = GameManager.GamePhase.EnemyShootPhase;
        GameManager.Instance.Check_PowerUps();
        getRandomBullet();
        if (Can_Fire)
        {            
            Fire = Random.Range(0, 2) == 0 ? true : false;
        }
        float angle = Random.Range(Min_Angle, Max_Angle);
        shootPoint.localRotation = Quaternion.Euler(shootPoint.rotation.x, shootPoint.rotation.y, angle);
        Canon.localRotation = Quaternion.Euler(shootPoint.rotation.x, shootPoint.rotation.y, angle);

        int Current_Burst = Random.Range(0, 3) == 0 ? Burst_Count : 1;
        for (int i = 0; i < Current_Burst; i++)
        {            
            GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float random_Force = Random.Range(MinshootForce, MaxshootForce + 1);
            rb.AddForce(Vector2.right * -random_Force, ForceMode2D.Impulse);
            rb.AddForce(Vector2.down * -random_Force / 3, ForceMode2D.Impulse);
            rb.AddTorque(2, ForceMode2D.Impulse);
            Bullet bull = bullet.GetComponent<Bullet>();
            bull.Damage = Random.Range(DamageMin, DamageMax + 1);
            if (Fire)
                bull.inFire = true;
            if (bull.GetComponent<Bullet>().type == Bullet.BulletType.Chicken && Fire)
            {
                bull.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.player_1.FriedChicken;
            }
        }        
        
        GameManager.Instance.isChecking = false;
        anim.SetTrigger("shoot");
        Camera_Shake.Instance.Shake(source, 2);
        GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Shoot);
    }

    public void EnemyPowerUps()
    {
        Invoke(nameof(EnemyPowerUpsInvoke), 0.5f);
    }
    private void EnemyPowerUpsInvoke()
    {
        if (canUsePowerUps)
        {
            float randomValue = Random.Range(0.0f, 1.0f);

            if (randomValue <= 0.5f)
            {
                int random = Random.Range(0, 4);
                if (random == 0)
                    OpenSheild();
                else if (random == 1)
                    Freeze();
                else if (random == 2)
                    TinyShots();
                else
                    TinyShip();
            }
        }
    }

    public void Shoot_Invoked(float value)
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(Start_Shooting(value));
    }

    IEnumerator Start_Shooting(float value)
    {
        yield return new WaitForSeconds(value);
        Shoot();
    }

    public void Get_Stats(int index)
    {
        Level lvl = levels.Get_Level(index);
        MaxshootForce = lvl.MaxshootForce;
        MinshootForce = lvl.MinshootForce;

        Max_Angle = lvl.Max_Angle;
        Min_Angle = lvl.Min_Angle;

        Can_Fire = lvl.Fire;

        Burst_Count = lvl.Burst_Shoots;

        DamageMax = lvl.DamageMax;
        DamageMin = lvl.DamageMin;

        canUsePowerUps = lvl.usePowerUps;

        GetShip_Skin(lvl.ship);
        GetSail_skin(lvl.sail);
        GetFlag_skin(lvl.flag);
        GetHelm_skin(lvl.helm);
        GetCannon_skin(lvl.cannon);
        GetAnchor_skin(lvl.anchor);
    }

    private void OpenSheild()
    {
        if(current_Usage_Sheild <= GameManager.Instance.Sheild_UsagePerGame)
        {
            SheildObj.SetActive(true);
            Animator anim = SheildObj.GetComponent<Animator>();
            anim.SetTrigger("open");
            current_Usage_Sheild++;
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.SheildSound);
        }        
    }

    private void Freeze()
    {
        if(current_Usage_freeze <= GameManager.Instance.Freez_UsagePerGame)
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                GameObject iceCube = Instantiate(GameManager.Instance.FreezObj, bullets[i].transform.position, bullets[i].transform.rotation);
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
            current_Usage_freeze++;
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.FreezeSound);
            Invoke(nameof(check_After), 2);
        }        
    }

    void check_After()
    {
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();
        }
    }

    private void TinyShots()
    {
        if(current_Usage_TinyShots <= GameManager.Instance.TinyShots_UsagePerGame)
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
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.ShrinkSound);
            current_Usage_TinyShots++;
        }        
    }

    private void TinyShip()
    {   
        if(current_Usage_TinyShip <= GameManager.Instance.TinyShip_UsagePerGame)
        {
            Ship ship = GetComponent<Ship>();
            ship.Floating = false;
            transform.DOLocalMove(new Vector3(transform.localPosition.x, -2.5f, transform.localPosition.z), 0.1f);
            current_Usage_TinyShip++;
            GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.ShrinkSound);
            transform.DOScale(new Vector3(-2, 2, 1), 0.3f);
        }
    }

    public void ResetPowerUpsEnemy()
    {
        current_Usage_Sheild = 0;
        current_Usage_freeze = 0;
        current_Usage_TinyShots = 0;
        current_Usage_TinyShip = 0;
        ResetShipEnemy();
    }

    public void ResetShipEnemy()
    {
        Ship ship = GetComponent<Ship>();
        ship.Floating = true;

        transform.DOScale(new Vector3(-4.3f, 4.3f, 1), 0.3f);
        if (SheildObj.activeInHierarchy)
            SheildObj.SetActive(false);
    }

    void getRandomBullet()
    {
        BulletPrefab = GameManager.Instance.GetLevel == 1 ? EnemyShots[0] :  EnemyShots[Random.Range(0, EnemyShots.Length)];
    }

    private void GetShip_Skin(int index)
    {
        ShipCosmatic cos = shipCosmatic.Get_Skin(index);
        Ship.runtimeAnimatorController = cos.anim;
    }

    private void GetSail_skin(int index)
    {
        Cosmatic cos = sailCosmatic.Get_Skin(index);
        sail.runtimeAnimatorController = cos.anim;
    }

    private void GetFlag_skin(int index)
    {
        Cosmatic cos = flagCosmatic.Get_Skin(index);
        Flag.runtimeAnimatorController = cos.anim;
    }

    private void GetHelm_skin(int index)
    {
        Cosmatic cos = helmCosmatic.Get_Skin(index);
        helm.runtimeAnimatorController = cos.anim;
    }

    private void GetCannon_skin(int index)
    {
        CanonCosmaticData data = CannonCosmatic.Get_Skin(index);
        Cannon.runtimeAnimatorController = data.anim;
        Stand.sprite = data.Stand;
    }

    private void GetAnchor_skin(int index)
    {
        AnchorCosmatic cos = anchorCosmatic.Get_Skin(index);
        anchors[0].sprite = cos.Top;
        anchors[1].sprite = cos.Bottom;
    }
}
