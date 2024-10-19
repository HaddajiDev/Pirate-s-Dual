using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SailCosmaticData : ScriptableObject
{
    public Cosmatic[] sail_skins;

    public int Get_Lenght
    {
        get
        {
            return sail_skins.Length;
        }
    }

    public Cosmatic Get_Skin(int index)
    {
        return sail_skins[index];
    }
}
