using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Effect : MonoBehaviour
{
    
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Invoke("End_Effect", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void End_Effect()
    {
        anim.SetTrigger("End");
        Destroy(gameObject, 0.6f);
    }
}
