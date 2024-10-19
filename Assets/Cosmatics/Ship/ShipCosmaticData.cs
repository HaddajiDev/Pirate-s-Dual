using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipCosmaticData : ScriptableObject
{
    public ShipCosmatic[] ship_skins;

    public int Get_Lenght
    {
        get
        {
            return ship_skins.Length;
        }
    }

    public ShipCosmatic Get_Skin(int index)
    {
        return ship_skins[index];
    }
}
