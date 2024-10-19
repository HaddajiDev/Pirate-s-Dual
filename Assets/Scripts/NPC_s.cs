using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC_s : MonoBehaviour
{
    Animator anim;

    public int Health = 10;

    public Transform Point_1;
    public Transform Point_2;
    public float Speed;
    public float Idle_Time = 5;
    int cap;
    bool isPatroling = true;

    bool Dead = false;

    public RuntimeAnimatorController[] controllers;
    

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = controllers[Random.Range(0, controllers.Length)];
    }

    private void Update()
    {
        if (!Dead)
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
    }

    public int hit = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Respawn" || collision.gameObject.tag == "Bullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            Instantiate(Ship.Instance.Explode_Effect, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            if (collision.gameObject.GetComponent<Bullet>().transform.childCount != 0)
            {
                collision.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            }
            Take_Damage(bullet.Damage);
            if(GameManager.Instance.cam.Follow != GameManager.Instance.player_1.transform.parent.transform)
            {
                hit++;
                if (hit == 1)
                    GameManager.Instance.cam.Follow = transform;
            }           

            GameManager.Instance.Coins += bullet.Player_Bullet ? Random.Range(1, 6) : 0;
        }
    }

    void Take_Damage(int dmg)
    {
        Health -= dmg;        
        anim.SetTrigger("hit");
        if(Health <= 0)
        {
            anim.SetTrigger("dead");
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            Dead = true;
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
        transform.localPosition = Vector2.MoveTowards(transform.position, Point_1.position, Speed * Time.deltaTime);        
        anim.SetTrigger("run");
        LookAt(transform.parent.name.ToLower().Contains("player") ? GameManager.Instance.transform : Point_1);

        if (transform.localPosition == Point_1.localPosition)
        {
            isPatroling = false;
            StartCoroutine(Wait(1));
        }
    }
    private void Patrol_Point_2()
    {
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
}
