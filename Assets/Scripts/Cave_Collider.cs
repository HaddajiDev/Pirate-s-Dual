using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_Collider : MonoBehaviour
{
    public NPC_s npc;
    int hit = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.tag = "Respawn";
            hit++;
            if (hit == 1)
            {
                Invoke(nameof(Check_Turns), 1f);
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
        npc.hit = 2;
        if (!GameManager.Instance.isChecking)
        {
            GameManager.Instance.isChecking = true;
            GameManager.Instance.Check_Turn();
            npc.hit = 0;            
        }
    }
}
