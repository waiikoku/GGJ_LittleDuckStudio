using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private enum AttackMode
    {
        Normal,
        Charge,
        Special
    }
    [SerializeField] private AttackMode mode;

    private enum TriggerPhase
    {
        Enter,
        Stay,
        Exit
    }
    [SerializeField] private TriggerPhase trigger;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (trigger == TriggerPhase.Enter)
        {
            var aer = animator.GetComponent<AnimationEventReceiptor>();
            if (aer != null)
            {
                aer.Attack();
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (trigger == TriggerPhase.Exit)
        {
            var aer = animator.GetComponent<AnimationEventReceiptor>();
            if (aer != null)
            {
                aer.Attack();
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
