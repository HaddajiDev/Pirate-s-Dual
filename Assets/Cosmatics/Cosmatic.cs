using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cosmatic
{
    public Sprite Cover;
    public Sprite[] spriteSheet;
    public RuntimeAnimatorController anim;

    [SerializeField]
    public Cost cost;
    public bool Ad_Skin = false;
}
