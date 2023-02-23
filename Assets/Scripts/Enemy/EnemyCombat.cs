using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat, IKnockbackable
{
    public float currentHealth;
    public int maxHealth = 100;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform head;
    public GameObject[] dropItems;

    [Header("Knockback")]
    [SerializeField] private EnemyAI movement;
    [SerializeField] private float strength = 16f;
    [SerializeField] private float delay = 0.15f;

    [SerializeField] private string diedSFX;

    private bool deadCondition1;
    private bool deadCondition2;

    /*
    private void Start()
    {
        HealthbarManager.Instance.AddHealth(head, this);
    }

    private void OnDestroy()
    {
        HealthbarManager.Instance.Remove(head);
    }
    */

    public override void TakeDamage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / (float)maxHealth);
        if (currentHealth == 0)
        {
            SoundManager.Instance.PlaySFX(diedSFX);
            SpawnManager.Instance.ConfirmKill();
            DropItem();
            gameObject.SetActive(false);
            //HealthbarManager.Instance.Remove(head, delegate { deadCondition1 = true; ConfirmDead(); });
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
