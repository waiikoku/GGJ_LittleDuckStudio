using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat , IKnockbackable
{
    public float currentHealth;
    public int maxHealth = 100;
    [SerializeField] private SpriteRenderer sr;

    public GameObject[] dropItems;

    [Header("Knockback")]
    [SerializeField] private EnemyAI movement;
    [SerializeField] private float strength = 16f;
    [SerializeField] private float delay = 0.15f;
    private void Start()
    {
        HealthbarManager.Instance?.AddHealth(transform, this);
    }

    public override void Damage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / (float)maxHealth);
        if(currentHealth == 0)
        {
            SpawnManager.Instance.ConfirmKill();
            DropItem();
            gameObject.SetActive(false);
            HealthbarManager.Instance.Remove(transform);
            CharacterLayerManager.Instance.Remove(sr, delegate { Destroy(gameObject,8f); });
        }
    }

    private void DropItem()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand > 50)
            {
                Instantiate(dropItems[i], transform.position, Quaternion.identity);
            }
        }
    }

    public void Knockback(Vector3 position)
    {
        Vector2 dir = transform.position - position;
        movement.rb.AddForce(dir * strength, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(delay);
        movement.rb.velocity = Vector2.zero;
    }
}
