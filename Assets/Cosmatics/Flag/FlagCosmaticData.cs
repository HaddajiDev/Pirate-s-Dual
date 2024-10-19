using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FlagCosmaticData : ScriptableObject
{
    public Cosmatic[] flag_skins;

    public int Get_Lenght
    {
        get
        {
            return flag_skins.Length;
        }
    }

    public Cosmatic Get_Skin(int index)
    {
        return flag_skins[index];
    }
}
