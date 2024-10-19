using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    AudioSource source;
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        duration = source.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, duration + 0.1f);
    }
}
