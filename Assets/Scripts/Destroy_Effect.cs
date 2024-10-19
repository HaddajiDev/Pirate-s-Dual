using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Destroy_Effect : MonoBehaviour
{
    public float Wait;

    public bool Ship;

    private void Start()
    {
        Destroy(gameObject, Wait);
    }
}
