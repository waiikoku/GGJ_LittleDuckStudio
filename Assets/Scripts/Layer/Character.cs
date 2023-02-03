using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Vector2 moveDirect;

    private void FixedUpdate()
    {
        if (rb == null) return; 
        rb.velocity = moveDirect;
    }
}
