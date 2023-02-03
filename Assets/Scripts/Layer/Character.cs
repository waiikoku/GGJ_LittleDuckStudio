using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Vector2 velocity;
    public Vector2 moveDirect;
    public Vector2 exceedVelocity;
    public bool receiver = false;

    private void FixedUpdate()
    {
        if (rb == null) return;
        if (receiver == false) return;
        velocity = rb.velocity;
        exceedVelocity = velocity - moveDirect;
        rb.velocity = moveDirect - exceedVelocity;
    }
}
