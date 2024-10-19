using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bullets_Data : ScriptableObject
{
    public Bullets[] bullets;

    public int Get_Lenght
    {
        get
        {
            return bullets.Length;
        }
    }

    public Bullets Get_Bullet(int index)
    {
        return bullets[index];
    }
}
