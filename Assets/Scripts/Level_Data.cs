using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level_Data : ScriptableObject
{    
    public Level[] levels;

    public int Get_Lenght
    {
        get
        {
            return levels.Length;
        }
    }

    public Level Get_Level(int index)
    {
        return levels[index];
    }
}
