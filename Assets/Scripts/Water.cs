using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Water : MonoBehaviour
{
    int hit = 0;
    public GameObject[] WaterSplash;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Respawn"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.mass = 2;
            if(collision.gameObject.CompareTag("Bullet")) rb.velocity = new Vector2(rb.velocity.x + ((rb.velocity.x * 80) / 100), rb.velocity.y);
            {
                rb.velocity = new Vector2(rb.velocity.x - ((rb.velocity.x * 50) / 100), rb.velocity.y + ((rb.velocity.y * 10) / 100));
            }
            rb.gravityScale *= -1;
            rb.gravityScale += (rb.gravityScale * 50) / 100;
            if(collision.gameObject.CompareTag("Bullet"))
            {
                hit++;
                if(hit == 1)
                {
                    Invoke(nameof(Check_Turns), 1f);
                    if (collision.gameObject.GetComponent<Bullet>().Player_Bullet)
                    {                        
                        GameManager.Instance.TotalShotsMiss++;
                        GameManager.Instance.SaveData("totalShotsMiss", GameManager.Instance.TotalShotsMiss);
                    }                    
                }
                    
                collision.gameObject.tag = "Respawn";
            }            
            if (rb.gravityScale >= 2.5f || rb.gravityScale <= -2.5f)
            {
                rb.gravityScale = 0;
                rb.transform.DOScale(0, 1.5f).OnComplete(() =>
                {
                    Destroy(collision.gameObject);                    
                });
            }

            Instantiate(WaterSplash[Random.Range(0, WaterSplash.Length)], collision.transform.position, Quaternion.identity);
            
            if(collision.gameObject.GetComponent<Bullet>().transform.childCount != 0)
            {
                collision.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            }
            if (collision.gameObject.GetComponent<Bullet>().Player_Bullet)
            {
                if (!GameManager.Instance.MissShot)
                    GameManager.Instance.MissShot = true;                
            }
        }

        if (collision.gameObject.CompareTag("ice"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            
            rb.mass = 2;
            if (collision.gameObject.CompareTag("ice")) rb.velocity = new Vector2(rb.velocity.x + ((rb.velocity.x * 80) / 100), rb.velocity.y);
            {
                rb.velocity = new Vector2(rb.velocity.x - ((rb.velocity.x * 50) / 100), rb.velocity.y + ((rb.velocity.y * 10) / 100));
            }
            rb.gravityScale *= -1;
            rb.gravityScale += (rb.gravityScale * 50) / 100;
            
            if (rb.gravityScale >= 2.5f || rb.gravityScale <= -2.5f)
            {
                rb.gravityScale = 0;
                IceBlock ice = collision.gameObject.GetComponent<IceBlock>();
                ice.Shrink();
                collision.gameObject.transform.DOScale(0, 1.5f).OnComplete(() =>
                {
                    Destroy(collision.gameObject);
                });
            }
            Instantiate(WaterSplash[Random.Range(0, WaterSplash.Length)], collision.transform.position, Quaternion.identity);
        }
    }
   
    void Check_Turns()
    {
        hit = 0;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();
        }
    }

}
