using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Slot : MonoBehaviour
{
    public int index;
    public Bullets bullet;

    Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        if(index != 0)
            Get_Bullet();
    }

    public void Get_Bullet()
    {
        for (int i = 0; i < GameManager.Instance.player_1.bulletsData.Get_Lenght; i++)
        {
            if(index == i)
            {
                bullet = GameManager.Instance.player_1.bulletsData.Get_Bullet(i);
                img.sprite = bullet.sr;                
                return;
            }
        }        
    }
}
