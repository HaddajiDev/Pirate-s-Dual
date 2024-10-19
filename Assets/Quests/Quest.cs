using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string Descrption;
    public int Goal_Value;
    public Cost cost;

    public Type type;
    public enum Type
    {
        wins,
        fireShots,
        noMissShots,
        none
    }
}
