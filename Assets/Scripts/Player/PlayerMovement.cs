using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 moveDirection;
    public float moveSpeed;
    [SerializeField] private PlayerAnimator anim;
    [SerializeField] private SpriteRenderer sr;
    public enum PhysicMode
    {
        Translate,
        Rigidbody,
        Position
    }
    public PhysicMode physicMode;
    private ZoneManager.LimitInfo limitInfo;

    private void Start()
    {
        InputManager.Instance.OnAxis += UpdateInput;
        limitInfo = ZoneManager.Instance.GetInfo();
    }
    private void FixedUpdate()
    {
        MoveHandler();
        ZoneManager.Instance.LimitYPosition(transform, limitInfo.MinY, limitInfo.MaxY);
    }

    private void LateUpdate()
    {
        AnimHandler();
    }

    public void UpdateInput(Vector2 input)
    {
        moveDirection = input;
        FlipHandler();
    }

    private void MoveHandler()
    {
        switch (physicMode)
        {
            case PhysicMode.Translate:
                transform.Translate(Time.deltaTime * moveSpeed * moveDirection);
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

    private void FlipHandler()
    {
        if(moveDirection.x > 0)
        {
            sr.flipX = false;
           //transform.localScale = Vector3.one;
        }
        else if(moveDirection.x < 0)
        {
            sr.flipX = true;
            //transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void AnimHandler()
    {
        anim.SetWalk(moveDirection != Vector2.zero);
    }


}
