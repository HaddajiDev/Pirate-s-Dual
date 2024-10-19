using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyCollider : MonoBehaviour
{
    int hit = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.tag = "Respawn";
            hit++;
            if (hit == 1)
            {
                Invoke(nameof(Check_Turns), 0.5f);
            }

            if (collision.gameObject.GetComponent<Bullet>().Player_Bullet)
            {
                if (!GameManager.Instance.MissShot)
                    GameManager.Instance.MissShot = true;
                GameManager.Instance.TotalShotsMiss++;
                GameManager.Instance.SaveData("totalShotsMiss", GameManager.Instance.TotalShotsMiss);
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
        }
    }
}
