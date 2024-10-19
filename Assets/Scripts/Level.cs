using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int level;


    [Header("Health")]
    public int Health = 20;

    [Header("Force")]
    public float MaxshootForce = 30;
    public float MinshootForce = 50;
    
    [Header("Angle")]
    public float Min_Angle = 10;
    public float Max_Angle = 60;
    
    [Header("Fire Shots")]
    public bool Fire = false;
    
    [Header("Burst Shots")]    
    public int Burst_Shoots = 1;

    [Header("Power Ups")]
    public bool usePowerUps = false;
    
    [Header("Damage")]
    public int DamageMin = 1;
    public int DamageMax = 2;

    [Header("Skins")]
    public int ship;
    public int anchor;
    public int flag;
    public int sail;
    public int helm;
    public int cannon;

    [Header("Distance")]
    public float distance = 0;
}
