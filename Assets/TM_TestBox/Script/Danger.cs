using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Danger : MonoBehaviour
{
    public Vector3 pos;

    private void FixedUpdate()
    {
        transform.position = pos;
    }
}
