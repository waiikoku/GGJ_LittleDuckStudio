using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 moveDirection;
    public float moveSpeed;
    private void Update()
    {
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        MoveHandler();
    }

    public void UpdateInput(Vector2 input)
    {
        moveDirection = input;
    }

    private void MoveHandler()
    {
        rb.velocity = moveDirection * moveSpeed;
    }
}
