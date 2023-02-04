using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAI : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity;
    public float smoothTime = 1f;
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
