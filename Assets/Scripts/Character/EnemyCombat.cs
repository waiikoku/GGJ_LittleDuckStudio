using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, IDamagable
{
    public Action<float> OnHealthUpdate;
    public float currentHealth;
    public int maxHealth = 100;
    [SerializeField] private SpriteRenderer sr;

    public GameObject[] dropItems;
    private void Start()
    {
        HealthbarManager.Instance.AddHealth(transform, this);
    }

    public void Damage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / (float)maxHealth);
        if(currentHealth == 0)
        {
            DropItem();
            gameObject.SetActive(false);
            HealthbarManager.Instance.Remove(transform);
            CharacterLayerManager.Instance.Remove(sr, delegate { Destroy(gameObject); });
        }
    }

    private void DropItem()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            Instantiate(dropItems[i], transform.position, Quaternion.identity);
        }
    }
}
