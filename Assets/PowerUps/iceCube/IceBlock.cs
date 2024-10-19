using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceBlock : MonoBehaviour
{
    private void Start()
    {
        Invoke("Shrink", 3);
    }
    public void Shrink()
    {
        transform.DOScale(0, 1.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
