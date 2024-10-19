using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    public int Damage;

    public BulletType type = new BulletType();
    Animator anim;
    public GameObject Fire_Partical;

    public bool Player_Bullet = false;

    public bool inFire = false;
    
    
    void Start()
    {
        cam = GameObject.FindWithTag("GameController").GetComponent<CinemachineVirtualCamera>();        
        cam.Follow = this.transform;
        
        Invoke(nameof(Destroy_bullet), 10);

        if (type == BulletType.Animated)
        {
            anim = GetComponent<Animator>();
        }

        if (inFire)
        {
            Instantiate(Fire_Partical, gameObject.transform);
            Damage += 2;
        }
    }

    void Destroy_bullet()
    {
        if (this != null)
        {
            transform.DOScale(0, 1.5f).OnComplete(() =>
            {                
                Destroy(gameObject);
            });
        }
    }

    public enum BulletType
    {
        Normal_Bullet,
        Animated,
        Chicken
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            if (type == BulletType.Animated)
            {
                anim.SetTrigger("off");
            }
        }       
    }
}
