using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GiftsData : ScriptableObject
{
    public Gifts[] gifts;    

    public int Get_Length
    {
        get
        {
            return gifts.Length;
        }
    }

    public Gifts GetGift(int index)
    {
        return gifts[index];
    }
}
