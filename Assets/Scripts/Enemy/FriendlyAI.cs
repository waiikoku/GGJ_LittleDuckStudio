using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : Character
{
    public AssistMode mode;
    public CombatStyle combatStyle;

    private List<EnemyCombat> enemies = new List<EnemyCombat>();
    public string targetTag = "Untagged";
    [SerializeField] private Animator anim;

    [Header("Physic-Based AI")]
    [SerializeField] private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stopDistance;
    public float distance;

    public bool autoCombat = false;
    public LayerMask targetLayer;

    [Header("Melee Combat")]
    public bool activateMelee = false;
    public float meleeDistance;
    public BoxCollider2D hitbox;
    public float meleeDmg;
    public float meleeCooldown = 1f;
    private float meleeTimestamp;
    [SerializeField] private AnimationEventReceiptor aer;

    [Header("Range Combat")]
    public bool activateRange = false;
    public float rangeDistance;
    public Transform shootPoint;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    public float fireRate = 1f; // bullet per second
    private float fireTimestamp;

    [Header("VFX")]
    [SerializeField] private GameObject OnCommand2Attack;

    [Header("SoundInfo")]
    [SerializeField] private string projectileSFX;
    [SerializeField] private string meleeSFX;
    private Action<float> OnHealthUpdate;
    public enum AssistMode
    {
        Stand,
        Follow
    }

    public enum CombatStyle
    {
        Melee,
        Range
    }

    protected override void Start()
    {
        base.Start();
        //HealthbarManager.Instance.AddHealth(transform, OnHealthUpdate);
        if(aer != null)
        {
            aer.OnAttack += MeleeHit;
            print("Add melee");
        }

    }

    private void FixedUpdate()
    {
        if (mode == AssistMode.Stand) return;

        if(enemies.Count != 0)
        {
            distance = Vector3.Distance(enemies[0].transform.position, transform.position);
            direction = enemies[0].transform.position - transform.position;
        }
        else
        {
            distance = 99;
            direction = target.position - transform.position;
        }
        var currentSpeed = direction.magnitude > stopDistance? moveSpeed: 0;
        rb.velocity = direction.normalized * currentSpeed;
    }

    private void LateUpdate()
    {
        if(mode == AssistMode.Follow)
        {
            anim.SetBool("Move", direction.magnitude > stopDistance);
            IsRight(direction.x > 0);
        }

        if (autoCombat == false) return;
        switch (combatStyle)
        {
            case CombatStyle.Melee:
                if (activateMelee == false) return;
                if (distance < meleeDistance)
                {
                    if (Time.time > meleeTimestamp)
                    {
                        meleeTimestamp = Time.time + meleeCooldown;
                        anim.SetTrigger("Attack");
                    }
                }
                break;
            case CombatStyle.Range:
                if (activateRange == false) return;
                if (Time.time > fireTimestamp)
                {
                    fireTimestamp = Time.time + (1f / fireRate);
                    ShootProjectile();
                }
                break;
            default:
                break;
        }
        if (enemies.Count == 0) return;
        if (enemies[0].currentHealth == 0)
        {
            enemies.RemoveAt(0);
        }
    }

    public void IsRight(bool value)
    {
        if (value)
        {
            m_sprite.flipX = false;
        }
        else
        {
            m_sprite.flipX = true;

        }
    }

    public void CommandToAttack(bool value)
    {
        OnCommand2Attack.SetActive(true);
        autoCombat = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            EnemyCombat ec = collision.GetComponentInParent<EnemyCombat>();
            if (enemies.Contains(ec)) return;
            enemies.Add(ec);
        }
    }

    private void ShootProjectile()
    {
        if (enemies.Count == 0) return;
        anim.SetTrigger("Attack");
        Vector3 shootDirection;
        shootDirection = enemies[0].transform.position - shootPoint.position;
        Rigidbody2D bulletInstance = Instantiate(projectile, shootPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        projectile.GetComponent<Projectile>().Set(dmg);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
        SoundManager.Instance.PlaySFX(projectileSFX);
    }

    private void MeleeHit()
    {
        print("Start to detect");
        Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)hitbox.transform.position + hitbox.offset,hitbox.size, targetLayer);
        foreach (var col in cols)
        {
            if (col.CompareTag(targetTag))
            {
                print($"Hit {col.gameObject.name}");
                IDamagable damagable = col.GetComponentInParent<IDamagable>();
                if(damagable != null)
                {
                    print("ApplyDmg");
                    damagable.Damage(meleeDmg);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlaySFX(meleeSFX);
                    }
                }
            }
        }
    }
}
