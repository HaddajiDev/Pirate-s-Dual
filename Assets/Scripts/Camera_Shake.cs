using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_Shake : MonoBehaviour
{
    public static Camera_Shake Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(CinemachineImpulseSource source, float force)
    {
        source.GenerateImpulseWithForce(force);
    }
}
