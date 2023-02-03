using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 moveDirection;
    public float moveSpeed;

    public enum PhysicMode
    {
        Translate,
        Rigidbody,
        Position
    }
    public PhysicMode physicMode;

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
        switch (physicMode)
        {
            case PhysicMode.Translate:
                transform.Translate(moveDirection * moveSpeed);
                break;
            case PhysicMode.Rigidbody:
                rb.velocity = moveDirection * moveSpeed;
                break;
            case PhysicMode.Position:
                transform.position += (Vector3)moveDirection * moveSpeed;
                break;
            default:
                break;
        }

    }
}
