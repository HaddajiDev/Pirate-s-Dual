using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public static Crab instance;    
    Animator anim;
    public Transform Point_1;
    public Transform Point_2;
    public Transform EscapePoint;
    public float Speed;
    public float Idle_Time = 5;
    int cap;
    bool isPatroling = true;

    public bool Escaped = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        if(PlayerPrefs.GetInt("tut") == 0)
        {
            Escaped = true;
        }
    }

    private void Update()
    {
        if (!Escaped)
        {
            if (isPatroling && cap == 1)
            {
                Patrol_Point_2();
            }
            if (isPatroling && cap == 0)
            {
                Patrol_Point_1();
            }
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.position, EscapePoint.position, (Speed + 5) * Time.deltaTime);
            anim.SetTrigger("run");
            LookAt(EscapePoint);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    bool flip;
    private void LookAt(Transform target)
    {
        Vector3 Scale = transform.localScale;
        if (target.position.x > transform.position.x)
        {
            Scale.x = Mathf.Abs(Scale.x) * (flip ? 1 : -1);
        }
        else
        {
            Scale.x = Mathf.Abs(Scale.x) * -1 * (flip ? 1 : -1);
        }
        transform.localScale = Scale;
    }


    private void Patrol_Point_1()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        transform.localPosition = Vector2.MoveTowards(transform.position, Point_1.position, Speed * Time.deltaTime);
        anim.SetTrigger("run");
        LookAt(Point_1);

        if (transform.localPosition == Point_1.localPosition)
        {
            isPatroling = false;
            StartCoroutine(Wait(1));
        }
    }
    private void Patrol_Point_2()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        transform.localPosition = Vector2.MoveTowards(transform.position, Point_2.position, Speed * Time.deltaTime);
        anim.SetTrigger("run");
        LookAt(Point_2);
        if (transform.localPosition == Point_2.localPosition)
        {
            isPatroling = false;
            StartCoroutine(Wait(0));
        }
    }



    IEnumerator Wait(int index)
    {
        anim.ResetTrigger("run");
        anim.SetTrigger("idle");
        yield return new WaitForSeconds(Idle_Time);
        isPatroling = true;
        cap = index;
    }

    private void OnMouseDown()
    {
        anim.SetTrigger("hit");
    }
}
