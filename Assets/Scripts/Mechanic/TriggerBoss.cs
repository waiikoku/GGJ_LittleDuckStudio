using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    public BoxCollider2D col;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if(collision.collider.GetComponent<NewBossController>())
            {

            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, col);
            }
        }
        else
        {
            Physics2D.IgnoreCollision(collision.collider, col);
        }

    }
}
