using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public Transform Start_Point;

    void Update()
    {        
        BoxCollider2D parentCollider = GetComponent<BoxCollider2D>();
        Bounds parentBounds = parentCollider.bounds;
        
        foreach (Transform child in transform)
        {
            BoxCollider2D childCollider = child.GetComponent<BoxCollider2D>();
            if (childCollider != null)
            {
                Bounds childBounds = childCollider.bounds;
                
                if (!parentBounds.Intersects(childBounds))
                {
                    child.gameObject.transform.localPosition = Start_Point.localPosition;
                }
            }
        }
    }
}
