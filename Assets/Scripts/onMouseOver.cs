using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class onMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 scale;
        
    void Start()
    {
        scale = gameObject.transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(scale.x + 0.05f, scale.y + 0.05f, 1);        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = new Vector3(scale.x, scale.y, 1);       
    }
}
