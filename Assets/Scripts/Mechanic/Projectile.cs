using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    public string targetTag = "Untagged";
    public float dmg;

    public void Set(float damage)
    {
        dmg = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            IDamagable damagable = collision.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(dmg);
            }
        }
    }

}
