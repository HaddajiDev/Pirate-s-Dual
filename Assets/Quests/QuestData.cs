using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    public Quest[] Quests;

    public int Get_Length
    {
        get
        {
            return Quests.Length;
        }
    }

    public Quest Get_Quest(int index)
    {
        return Quests[index];
    }
}
