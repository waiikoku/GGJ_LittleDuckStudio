using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiptor : MonoBehaviour
{
    public Action OnAttack;
    public void Attack()
    {
        OnAttack?.Invoke();
    }
}
