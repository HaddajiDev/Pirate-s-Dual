using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnchorCosmaticData : ScriptableObject
{
    public AnchorCosmatic[] anchor_skins;

    public int Get_Lenght
    {
        get
        {
            return anchor_skins.Length;
        }
    }

    public AnchorCosmatic Get_Skin(int index)
    {
        return anchor_skins[index];
    }
}
