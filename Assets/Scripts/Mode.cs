using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    public enum ModeType
    {
        SinglePlayer,
        Classic,
        Competitive,
        Tournament,
    }
    
    public ModeType type;

    public ModeType getType()
    {
        return type;
    }
}
