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

    [Header("Melee Combat")]
    public bool activateMelee = false;
    public float meleeDistance;
    public Transform meleePoint;
    public float meleeDmg;
    public float meleeCooldown = 1f;
    private float meleeTimestamp;

    [Header("Range Combat")]
    public bool activateRange = false;
    public float rangeDistance;
    public Transform shootPoint;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    public float fireRate = 1f; // bullet per second
    private float fireTimestamp;

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
        HealthbarManager.Instance.AddHealth(transform, OnHealthUpdate);
    }

    private void FixedUpdate()
    {
        direction = target.position - transform.position;
        var currentSpeed = direction.magnitude > stopDistance? moveSpeed: 0;
        rb.velocity = direction.normalized * currentSpeed;
    }

    private void LateUpdate()
    {
        switch (combatStyle)
        {
            case CombatStyle.Melee:
                if (activateMelee == false) return;
                if (Time.time > meleeTimestamp)
                {
                    meleeTimestamp = Time.time + meleeCooldown;
                    
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

    private void OnDrawGizmos()
    {
        
    }
}
