using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombat : MonoBehaviour , IDamagable
{
    public Action<float> OnHealthUpdate;
    public virtual void Damage(float dmg)
    {
    }
}
