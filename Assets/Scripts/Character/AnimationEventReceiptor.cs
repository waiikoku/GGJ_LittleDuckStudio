using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiptor : MonoBehaviour
{
    public Action OnAttack;
    public Action OnDied;
    public void Attack()
    {
        OnAttack?.Invoke();
    }

    public void Died()
    {
        OnDied?.Invoke();
    }
}
