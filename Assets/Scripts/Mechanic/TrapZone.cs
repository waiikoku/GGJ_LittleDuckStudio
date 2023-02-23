using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    public string targetTag = "Untagged";
    public float dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            IDamagable damagable = collision.GetComponentInParent<IDamagable>();
            if(damagable != null)
            {
                damagable.TakeDamage(dmg);
            }
        }
    }
}
