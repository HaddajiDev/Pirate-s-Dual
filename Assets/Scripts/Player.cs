using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    public CinemachineVirtualCamera cam;
    private Vector2 startDragPos;
    private LineRenderer trajectoryLineRenderer;
    private float maxPullDistance;
    public float maxForce = 50f;
    public GameObject BulletPrefab;

    [Header("Line Renderer")]
    public GameObject trajectoryLinePrefab;
    public int linePoints = 20;

    [Header("Others")]
    public Transform shootPoint;
    public GameObject cannonFireEffect;
    public Bullets_Data bulletsData;
    private int damage;
    public float scaleFactor = 0.5f;
    public float minSize = 17.0f;
    public float maxSize = 19.0f;
    public float resizeSpeed = 1.0f;
    public Sprite FriedChicken;

    public bool ready = true;
    public int burstCount;
    public bool inFire = false;

    private Animator anim;
    private CinemachineImpulseSource impulseSource;
    [HideInInspector] public int _selectedBullet;
    public List<(int, int)> _bulletsLimit = new List<(int, int)>();

    [Header("Cosmatic")]
    public ShipCosmaticData shipCosmatic;
    [HideInInspector] public int _selectedShip;
    public Animator Ship;

    [Space]
    public CanonSkins CannonCosmatic;
    [HideInInspector] public int _selectedCannon;
    public Animator Cannon;
    public SpriteRenderer Stand;

    [Space]
    public SailCosmaticData sailCosmatic;
    [HideInInspector] public int _selectedSail;
    public Animator sail;

    [Space]
    public HelmCosmaticData helmCosmatic;
    [HideInInspector] public int _selectedHelm;
    public Animator helm;
    public SpriteRenderer helmImage; 

    [Space]
    public FlagCosmaticData flagCosmatic;
    [HideInInspector] public int _selectedFlag;
    public Animator Flag;

    [Space]
    public AnchorCosmaticData anchorCosmatic;
    [HideInInspector] public int _selectedAnchor;
    public SpriteRenderer[] anchors;


    void Start()
    {
        ready = false;
        maxPullDistance = maxForce;
        mainCamera = Camera.main;
        InitializeTrajectoryLineRenderer();
        anim = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        SelectBullet(0);

        GetAllSkins();
    }

    void Update()
    {
        if (ready)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                if (Input.touchCount > 0)
                {
                    startDragPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    startDragPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                trajectoryLineRenderer.enabled = true;
                if(PlayerPrefs.GetInt("tut") == 0)
                {
                    GameManager.Instance.Pointer.gameObject.SetActive(false);
                    GameManager.Instance.Hand.gameObject.SetActive(false);
                    GameManager.Instance.SkipNextDia();
                }
            }

            if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                Vector2 currentMousePos;
                if (Input.touchCount > 0)
                {
                    currentMousePos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    currentMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                DrawTrajectory(startDragPos, currentMousePos);
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                mainCamera.orthographicSize = 17;
                cam.m_Lens.OrthographicSize = 17;

                trajectoryLineRenderer.enabled = false;
                Vector2 endDragPos;
                if (Input.touchCount > 0)
                {
                    endDragPos = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else
                {
                    endDragPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

                Vector2 swipeDirection = endDragPos - startDragPos;
                float swipeMagnitude = swipeDirection.magnitude;

                swipeMagnitude = Mathf.Min(swipeMagnitude, maxPullDistance);
                float forceStrength = swipeMagnitude / maxPullDistance * maxForce;                

                if (GameManager.Instance.Fire_Uses == 0)
                    inFire = false;

                if (getLimit() == 0)
                    SelectBullet(0);

                if (burstCount > 1 && GameManager.Instance.Burst_Uses != 0)
                {
                    for (int i = 0; i < burstCount; i++)
                    {
                        shootBurst(swipeDirection, forceStrength);
                    }
                    GameManager.Instance.Burst_Uses--;
                    GameManager.Instance.SaveData("burstUses", GameManager.Instance.Burst_Uses);
                }
                else
                {
                    shootOne(swipeDirection, forceStrength);
                }

                setLimit();
                afterShoot();

                //tut
                if (PlayerPrefs.GetInt("tut") == 0)
                {
                    GameManager.Instance.SkipNextDia();
                }
            }
        }
    }

    void shootOne(Vector2 swipeDirection, float forceStrength)
    {
        GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Player_Bullet = true;
        bullet.GetComponent<Bullet>().Damage = damage;
        if (bullet.GetComponent<Bullet>().type == Bullet.BulletType.Chicken && inFire)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = FriedChicken;
        }
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-swipeDirection.normalized * forceStrength, ForceMode2D.Impulse);
        rb.AddTorque(-2, ForceMode2D.Impulse);
        if (inFire)
        {
            bullet.GetComponent<Bullet>().inFire = true;
        }
        GameManager.Instance.TotalShotsFired++;
        GameManager.Instance.SaveData("totalShotsFired", GameManager.Instance.TotalShotsFired);
    }

    void shootBurst(Vector2 swipeDirection, float forceStrength)
    {
        Vector3 spreadVector = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
        GameObject bullet = Instantiate(BulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Player_Bullet = true;
        bullet.GetComponent<Bullet>().Damage = damage;
        if(bullet.GetComponent<Bullet>().type == Bullet.BulletType.Chicken && inFire)
        {
            bullet.GetComponent<SpriteRenderer>().sprite = FriedChicken;
        }
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-swipeDirection.normalized * forceStrength, ForceMode2D.Impulse);
        rb.AddTorque(-2, ForceMode2D.Impulse);
        rb.velocity = (shootPoint.right * forceStrength) + spreadVector;
        if (inFire)
        {
            bullet.GetComponent<Bullet>().inFire = true;
        }
        GameManager.Instance.TotalShotsFired += 3;
        GameManager.Instance.SaveData("totalShotsFired", GameManager.Instance.TotalShotsFired);
    }

    void afterShoot()
    {
        if (inFire && GameManager.Instance.Fire_Uses != 0)
        {
            GameManager.Instance.Fire_Uses--;
            GameManager.Instance.SaveData("fireUses", GameManager.Instance.Fire_Uses);
        }

        GameManager.Instance.phase = GameManager.GamePhase.PlayerShootPhase;
        GameManager.Instance.player_2.EnemyPowerUps();

        if (cannonFireEffect != null)
            Instantiate(cannonFireEffect, shootPoint.position, transform.rotation);

        this.enabled = false;
        ready = false;
        GetComponent<Rotate_Object>().enabled = false;
        GameManager.Instance.isChecking = false;
        anim.SetTrigger("shoot");
        Camera_Shake.Instance.Shake(impulseSource, 2);

        if (PlayerPrefs.GetInt("tut") != 0)
        {
            int value = Random.Range(0, 3);
            if (value == 2)
            {
                GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Fire[Random.Range(0, GameManager.Instance.Soundeffects.Fire.Length)]);
            }
        }
        GameManager.Instance.PlayAudio(GameManager.Instance.Soundeffects.Shoot);
    }

    private void InitializeTrajectoryLineRenderer()
    {
        GameObject trajectoryLineObject = Instantiate(trajectoryLinePrefab, shootPoint.position, Quaternion.identity);
        trajectoryLineRenderer = trajectoryLineObject.GetComponent<LineRenderer>();
        trajectoryLineRenderer.enabled = false;
        trajectoryLineRenderer.startColor = Color.black;
        trajectoryLineRenderer.endColor = Color.black;
    }


    private Vector3[] CalculateTrajectoryPoints(Vector2 start, Vector2 startVelocity, Vector2 gravity)
    {
        float timestep = 0.05f;
        Vector3[] points = new Vector3[linePoints];
        for (int i = 0; i < linePoints; i++)
        {
            float t = i * timestep;
            points[i] = start + startVelocity * t + 0.5f * gravity * t * t;
        }
        return points;
    }    

    private void DrawTrajectory(Vector2 startPos, Vector2 endPos)
    {
        Vector2 forceDirection = (endPos - startPos).normalized;
        float forceMagnitude = (endPos - startPos).magnitude;

        Vector3[] trajectoryPoints = CalculateTrajectoryPoints(startPos, -forceDirection * forceMagnitude, Physics2D.gravity);
        Vector2 offset = (Vector2)transform.position - startPos;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i] += (Vector3)offset;
        }
        trajectoryLineRenderer.positionCount = trajectoryPoints.Length;
        trajectoryLineRenderer.SetPositions(trajectoryPoints);

        float targetSize = Mathf.Clamp(forceMagnitude * scaleFactor, minSize, maxSize);        
        cam.m_Lens.OrthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * resizeSpeed);
    }

    private void ReadyDelay()
    {
        ready = true;
    }
    public void ReadyUp()
    {
        Invoke(nameof(ReadyDelay), 0.2f);
        GameManager.Instance.Get_Ready_UI(0, 0.3f);
    }

    private void GetBullet(int index)
    {
        Bullets bullet = bulletsData.Get_Bullet(index);
        BulletPrefab = bullet.Bullet_Prefab;
        damage = bullet.Damage;
        if(index != 0)
        {
            if (!_bulletsLimit.Any(limit => limit.Item1 == index))
            {
                _bulletsLimit.Add((index, bullet.Limit));
            }
        }        
    }

    public void SelectBullet(int index)
    {
        GetBullet(index);
        _selectedBullet = index;        
    }

    private void setLimit()
    {
        if(_selectedBullet != 0)
        {
            for (int i = 0; i < _bulletsLimit.Count; i++)
            {
                var current = _bulletsLimit[i];                
                if (_selectedBullet == current.Item1)
                {
                    var newValue = (current.Item1, current.Item2 - 1);
                    if (newValue.Item2 <= 0)
                        newValue.Item2 = 0;
                    _bulletsLimit[i] = newValue;
                }
            }
            Set_Limit_UI();
        }              
    }

    private int getLimit()
    {
        if(_selectedBullet != 0)
        {
            for (int i = 0; i < _bulletsLimit.Count; i++)
            {
                var current = _bulletsLimit[i];
                if (_selectedBullet == current.Item1)
                {
                    return current.Item2;
                }
            }
        }
        return 0;
    }

    public void Set_Limit_UI()
    {        
        List<int> itemsToRemove = new List<int>();

        foreach (var el in _bulletsLimit)
        {
            if (UI_Controller.instance.bullet_slot_1.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_1.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_1.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
            if (UI_Controller.instance.bullet_slot_2.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_2.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_2.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
            if (UI_Controller.instance.bullet_slot_Extra.GetComponent<Bullet_Slot>().index == el.Item1)
            {
                UI_Controller.instance.Bullet_Limit_Extra.text = el.Item2.ToString();
                if (el.Item2 == 0)
                {
                    UI_Controller.instance.Select_Bullet_Extra.gameObject.SetActive(false);
                    UI_Controller.instance.ResetChecks();
                    UI_Controller.instance.Checks[0].SetActive(true);
                    itemsToRemove.Add(el.Item1);
                }
            }
        }

        foreach (int itemToRemove in itemsToRemove)
        {
            _bulletsLimit.RemoveAll(tuple => tuple.Item1 == itemToRemove);
        }
    }

    public void GetAllSkins()
    {
        GetShip_Skin(_selectedShip);
        GetSail_skin(_selectedSail);
        GetFlag_skin(_selectedFlag);
        GetHelm_skin(_selectedHelm);
        GetCannon_skin(_selectedCannon);
        GetAnchor_skin(_selectedAnchor);
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
        Cosmatic cos = helmCosmatic.Get_Skin(_selectedHelm);        
        helm.runtimeAnimatorController = cos.anim;
        helmImage.sprite = cos.Cover;
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