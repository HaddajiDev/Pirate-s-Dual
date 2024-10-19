using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Show_Description : MonoBehaviour
{
    private bool isFlipped = false;
    public GameObject Description;
    public Transform arrow;

    private void Flip()
    {
        float targetRotation = isFlipped ? 0f : 180f;
        arrow.DORotate(new Vector3(0, 0, targetRotation), 0.3f);
        isFlipped = !isFlipped;
    }

    public void Show()
    {
        if (Description.activeInHierarchy)
            Description.SetActive(false);
        else
            Description.SetActive(true);
        Flip();
    }
}
