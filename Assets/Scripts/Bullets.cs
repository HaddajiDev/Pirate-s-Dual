using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bullets
{
    public string Name;
    public Sprite sr;

    public GameObject Bullet_Prefab;
    public int Damage;

    public int Limit;

    [SerializeField]
    public Cost cost = new Cost(0, 0);

    public bool ad = false;
}
