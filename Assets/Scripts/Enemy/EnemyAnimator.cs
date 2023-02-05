using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : Character
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;
    public bool isFreeze = false;
    private int animID_Attack;

    private void Awake()
    {
        animID_Attack = Animator.StringToHash("Attack");
    }

    protected override void Start()
    {
        base.Start();

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

    public void Freeze(bool freeze,float duration = 1)
    {
        anim.enabled = !freeze;
        isFreeze = freeze;
        if(duration != 0)
        {
            StartCoroutine(Unfreeze(duration));
        }
    }

    private IEnumerator Unfreeze(float duration)
    {
        yield return new WaitForSeconds(duration);
        Freeze(false,0);
    }
}
