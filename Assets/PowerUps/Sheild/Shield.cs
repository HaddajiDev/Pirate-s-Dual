using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    int hit = 0;
    public GameObject Explode_Effect;
    Animator anim;

    

    private void Start()
    {
        anim = GetComponent<Animator>();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.tag = "Respawn";
            Instantiate(Explode_Effect, collision.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            hit++;
            if (hit == 1)
            {
                Invoke("Check_Turns", 0.5f);                
            }

            if (collision.gameObject.GetComponent<Bullet>().Player_Bullet)
            {
                if (!GameManager.Instance.MissShot)
                    GameManager.Instance.MissShot = true;
                GameManager.Instance.TotalShotsMiss++;
                GameManager.Instance.SaveData("totalShotsMiss", GameManager.Instance.TotalShotsMiss);
            }
            if (collision.gameObject.GetComponent<Bullet>().transform.childCount != 0)
            {
                collision.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }
    }

    void Check_Turns()
    {
        hit = 0;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();
            anim.SetTrigger("collapse");
            Invoke("Disabel_Object", 0.7f);
        }
    }

    void Disabel_Object()
    {
        gameObject.SetActive(false);
    }
}
