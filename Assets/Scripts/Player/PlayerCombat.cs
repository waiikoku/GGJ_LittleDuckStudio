using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    public Transform wandHolder;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    public float currentHealth;
    public float maxHealth;

    public int hitCount;
    [SerializeField] private PlayerAnimator anim;

    [Header("SoundInfo")]
    [SerializeField] private string projectileSFX;
    [SerializeField] private string wiggleSFX;

    [Header("Skills")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag;
    [SerializeField] private GameObject fairySupport;
    public float fairyConditionHp = 0.5f;
    public float fairyCooldown = 60f;
    public bool fairyUsed = false;
    private float fairyTimestamp;
    public float wiggleRadius = 5f;

    private Queue<Vector2> attackQueue;
    private void Start()
    {
        InputManager.Instance.OnLMB += PrimaryAttack;
        if (UIManager.Instance != null)
        {
            OnHealthUpdate += UIManager.Instance.UpdateHealth;
        }
        anim.aer.OnAttack += ShootProjectile;
        InputManager.Instance.OnSprinkle += WiggleSkill;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnLMB -= PrimaryAttack;
    }

    public override void Damage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
        ConditionSkill();
    }

    public void Heal(float hp)
    {
        currentHealth = Mathf.Clamp(currentHealth + hp, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
    }
    private void ShootProjectile()
    {
        Vector3 shootDirection;
        shootDirection = InputManager.Instance.mousePos;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

        shootDirection = (shootDirection - transform.position);

        Rigidbody2D bulletInstance = Instantiate(projectile, wandHolder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        Projectile pfb = projectile.GetComponent<Projectile>();
        pfb.Set(dmg);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
        //SoundManager.Instance.PlaySFX(projectileSFX);
    }

    private void PrimaryAttack(bool value)
    {
        if (value == false) return;
        anim.Attack();
        //attackQueue.Enqueue(InputManager.Instance.mousePos);
    }

    private void WiggleSkill(bool value)
    {
        if (value == false) return;
        anim.SetWand(false);
        anim.TriggerWaggle();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(wiggleSFX);
        }
        Vector3 pos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, wiggleRadius, targetLayer);
        foreach (var col in cols)
        {
            if (col.CompareTag(targetTag))
            {
                FriendlyAI buddy = col.GetComponentInParent<FriendlyAI>();
                if (buddy != null)
                {
                    buddy.CommandToAttack(true);

                }
            }
        }
    }

    private void ConditionSkill()
    {
        if (Time.time < fairyTimestamp) return;
        if ((currentHealth / maxHealth) < fairyConditionHp)
        {
            SpawnFairy();
            fairyTimestamp = Time.time + fairyCooldown;
        }
    }

    private void SpawnFairy()
    {
        Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(-0.1f, 0));
        GameObject go = Instantiate(fairySupport, pos, Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wiggleRadius);
    }
#endif
}
