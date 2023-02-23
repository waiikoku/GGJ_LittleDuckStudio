using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    public string targetTag = "Untagged";
    public float dmg;
    private bool isSet = false;

    public void Set(float damage)
    {
        dmg = damage;
        isSet = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            IDamagable damagable = collision.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(dmg);
            }
            IKnockbackable knockbackable = collision.GetComponentInParent<IKnockbackable>();
            if(knockbackable != null)
            {
                knockbackable.Knockback(transform.position);
            }
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        if (isSet == false) return;
        Destroy(gameObject);
    }
}
