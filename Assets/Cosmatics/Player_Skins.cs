using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Skins
{
    public List<int> Ships_Skins;
    public List<int> Cannon_Skins;
    public List<int> Sail_Skins;
    public List<int> Flag_Skins;
    public List<int> Anchors_Skins;
    public List<int> Helm_Skins;

    public void Add_Skin(List<int> list, int value)
    {
        list.Add(value);
    }

    public bool Check_Skin(List<int> list, int value)
    {
        if (list.Contains(value))
        {
            return true;
        }
        return false;
    }
}
