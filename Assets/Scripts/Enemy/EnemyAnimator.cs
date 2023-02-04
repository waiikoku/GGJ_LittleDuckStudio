using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : Character
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;
    private int animID_Attack;

    protected override void Start()
    {
        base.Start();
        animID_Attack = Animator.StringToHash("Attack");
    }
    public void Attack()
    {
        anim.SetTrigger(animID_Attack);
    }

    public void IsRight(bool value)
    {
        if (value)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;

        }
    }
}
