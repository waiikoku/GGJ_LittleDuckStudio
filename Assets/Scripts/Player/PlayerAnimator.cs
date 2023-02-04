using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : Character
{
    [SerializeField] private Animator anim;
    [SerializeField] private Animator m_wand;
    public Animator Wand => m_wand;
    public AnimationEventReceiptor aer;
    private int animID_Died;
    private int animID_Waggle;
    private int animID_Summon;
    private int animID_Walk;
    private int animID_Attack;
    private void Awake()
    {
        animID_Died = Animator.StringToHash("Died");
        animID_Waggle = Animator.StringToHash("Waggle");
        animID_Summon = Animator.StringToHash("Summon");
        animID_Walk = Animator.StringToHash("Walk");
        animID_Attack = Animator.StringToHash("Attack");
    }

    public void Attack()
    {
        print("Wand attack!");
        m_wand.SetTrigger(animID_Attack);
    }

    public void SetDied(bool died)
    {
        anim.SetBool(animID_Died,died);
    }

    public void SetWalk(bool walk)
    {
        anim.SetBool(animID_Walk, walk);
    }

    public void TriggerWaggle()
    {
        anim.SetTrigger(animID_Waggle);
    }

    public void SetWand(bool value)
    {
        m_wand.gameObject.SetActive(value);
    }
}
