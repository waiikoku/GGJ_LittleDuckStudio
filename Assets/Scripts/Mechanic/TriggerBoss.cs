using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private string playerTag = "Untagged";
    public BoxCollider2D col;
    private bool playerPassed = false;
    [SerializeField] private float minimumExit = 1f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D cacheCol = collision.collider;
        if (cacheCol.CompareTag(targetTag))
        {
            BossAI cache = cacheCol.GetComponentInParent<BossAI>();
            if(cache == null)
            {
                //Exclude Normal Enemy
                Physics2D.IgnoreCollision(cacheCol, col);
            }
            else
            {
                //Block Boss
            }
        }
        if (playerPassed)
        {
            //Block player
        }
        else
        {
            //Exclude for first-pass
            if (cacheCol.CompareTag(playerTag))
            {
                Physics2D.IgnoreCollision(cacheCol, col);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D cacheCol = collision.collider;
        if (playerPassed)
        {

        }
        else
        {
            if (cacheCol.CompareTag(playerTag))
            {
                if(cacheCol.transform.position.x > transform.position.x + minimumExit)
                {
                    playerPassed = true;
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(minimumExit, 0, 0));
    }
#endif
}
