using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CanonSkins : ScriptableObject
{
    public CanonCosmaticData[] canonCosmatics;

    public int GetLength
    {
        get
        {
            return canonCosmatics.Length;
        }
    }

    public CanonCosmaticData Get_Skin(int index)
    {
        return canonCosmatics[index];
    }
}
