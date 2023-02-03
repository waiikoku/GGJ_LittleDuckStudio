using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private string targetTag = "Untagged";

    [Header("Auto")]
    [SerializeField] private bool auto = false;
    [SerializeField] private Item item;

    private void Awake()
    {
        if (auto)
        {
            Set(item);
        }
    }

    public void Set(Item info)
    {
        sr.sprite = info.itemIcon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            GameManager.Instance.AddItem(item.itemID);
            Destroy(gameObject);
        }
    }
}
