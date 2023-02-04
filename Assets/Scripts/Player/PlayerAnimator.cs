using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : Character
{
    [SerializeField] private Animator anim;
    [SerializeField] private Animator wand;
    public AnimationEventReceiptor aer;
    public void Attack()
    {
        anim.SetTrigger("Attack");
        wand.SetTrigger("Attack");
    }
}
