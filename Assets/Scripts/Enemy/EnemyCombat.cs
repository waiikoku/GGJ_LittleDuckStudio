using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : CharacterCombat , IKnockbackable , IDamagable
{
    public float currentHealth;
    public int maxHealth = 100;
    public float damage = 1;
    [SerializeField] private SpriteRenderer sr;

    public GameObject[] dropItems;

    [Header("Knockback")]
    [SerializeField] private EnemyAI movement;
    [SerializeField] private float strength = 16f;
    [SerializeField] private float delay = 0.15f;

    [Header ("Atk")]
    private EnemyAnimator anima;
    public Transform attackPoint; 
    public Transform attackPoint2;
    public float attackRange;
    public LayerMask playerLayer;
    public float attackTime = 0;
    public float attackDelay = 3;

    private void Start()
    {
        HealthbarManager.Instance?.AddHealth(transform, this);
        anima = GetComponent<EnemyAnimator>();
    }
    void Update()
    {
        if (movement.distance < movement.stopDistance)
        { Attack(); }
    }
    void Attack()
    {
        anima.Attack();

        if (attackTime < Time.time)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
            Collider2D[] hit2 = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, playerLayer);

            foreach (Collider2D player in hit)
            {
                if (player.CompareTag("Player"))
                {
                    Debug.Log("Hit : " + player.gameObject.name);
                    IDamagable damagable = player.GetComponentInParent<IDamagable>();
                    damagable.Damage(damage);
                    attackTime = Time.time + attackDelay;
                }
                //player.gameObject.GetComponent<PlayerCombat>().Damage(damage);
            }
            foreach (Collider2D player in hit2)
            {
                if (player.CompareTag("Player"))
                {
                    Debug.Log("Hit : " + player.gameObject.name);
                    IDamagable damagable = player.GetComponentInParent<IDamagable>();
                    damagable.Damage(damage);
                    attackTime = Time.time + attackDelay;
                }
                //player.gameObject.GetComponent<PlayerCombat>().Damage(damage);
            }
        }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
    }
}
