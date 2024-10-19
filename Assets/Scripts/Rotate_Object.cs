using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Object : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float maxRadius = 3f;
    private bool isDragging = false;
    private float lastAngle = 0f;

    private float wid_height;

    private void Start()
    {
        wid_height = transform.localScale.x < 0 ? -transform.localScale.x : transform.localScale.x;
    }

    void Update()
    {
        if (GetComponent<Player>().ready)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                isDragging = false;
                lastAngle = GetCurrentAngle();
            }

            if (isDragging)
            {
                float angle = GetCurrentAngle();
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, angle), rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, lastAngle);
            }
        }       
    }

    float GetCurrentAngle()
    {
        Vector3 mousePosition;
        if (Input.touchCount > 0)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        mousePosition.z = 0f;
        Vector3 direction = mousePosition - transform.position;
        float distanceToMouse = direction.magnitude;
        if (distanceToMouse > maxRadius)
        {
            direction = direction.normalized * maxRadius;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;

        if (mousePosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-wid_height, wid_height, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(wid_height, wid_height, transform.localScale.z);
        }
        return angle;
    }

}
