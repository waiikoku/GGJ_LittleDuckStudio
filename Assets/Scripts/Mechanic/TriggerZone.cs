using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private string targetTag = "Untagged";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            SpawnManager.Instance.TriggerZone();
        }
    }
}
