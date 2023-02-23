using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private BoxCollider2D col;
    private bool lockdown = false;
    private bool pass = false;
    [SerializeField] private float pushSpeed = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (!pass)
            {
                pass = true;
                SpawnManager.Instance.TriggerZone();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            collision.transform.position += Vector3.right * Time.deltaTime * pushSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (collision.transform.position.x > transform.position.x + transform.localScale.x)
            {
                col.isTrigger = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
   
        }
        else
        {
            Physics2D.IgnoreCollision(collision.collider, col);
        }
    }
}
