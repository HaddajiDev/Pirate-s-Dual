using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HelmCosmaticData : ScriptableObject
{
    public Cosmatic[] Helm_skins;

    public int Get_Lenght
    {
        get
        {
            return Helm_skins.Length;
        }
    }

    public Cosmatic Get_Skin(int index)
    {
        return Helm_skins[index];
    }
}
