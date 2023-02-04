using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAER : MonoBehaviour
{
    public AnimationEventReceiptor aer;

    public void Attack()
    {
        aer.Attack();
    }
}
