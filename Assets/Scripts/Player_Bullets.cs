using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Bullets
{
    public List<int> data = new List<int>();

    public void Add_Bullet(int index)
    {
        data.Add(index);
    }
}
