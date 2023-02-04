using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ZoneManager;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform player;
    private Vector3 direction;
    public float speed = 1;
    public float stopDistance = 1;
    public float distance;

    [SerializeField] private EnemyCombat ec;
    [SerializeField] private EnemyAnimator ea;
    [SerializeField] private AnimationEventReceiptor aer;
    public enum CombatStyle
    {
        Melee,Range
    }
    public CombatStyle combatStyle;


    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private LayerMask targetLayer;
    private List<IDamagable> enemies = new List<IDamagable>();

    [Header("Melee")]
    public float meleeDistance = 1f;
    public float meleeCooldown = 1f;
    public BoxCollider2D hitbox;
    public float meleeDmg;

    [Header("Range")]
    [SerializeField] private Transform weaponHolder;
    public float rangeDistance = 1;
    public float fireRate = 1; //Per second
    private float attackTimer;
    public Rigidbody2D projectile;
    public float pjSpeed;
    public float rangeDmg;
    private ZoneManager.LimitInfo limitInfo;

    [Header("SoundInfo")]
    [SerializeField] private string projectileSFX;
    [SerializeField] private string meleeSFX;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        limitInfo = ZoneManager.Instance.GetInfo();
        if (combatStyle == CombatStyle.Melee)
        {
            aer.OnAttack += MeleeHit;
        }
    }

    private void FixedUpdate()
    {
        ZoneManager.Instance.LimitYPosition(transform, limitInfo.MinY, limitInfo.MaxY);
        if (ea.isFreeze)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        direction = player.position - transform.position;
        distance = direction.magnitude;
        FlipHandler();
        if (distance < stopDistance)
        {
            rb.velocity = Vector2.zero;
            rb.drag = 1f;
            return;
        }
        rb.velocity = direction.normalized * speed; 
        /*
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        rb.drag = 0f;
        */
    }


    private void LateUpdate()
    {
        switch (combatStyle)
        {
            case CombatStyle.Melee:
                if (distance < meleeDistance)
                {
                    if (Time.time > attackTimer)
                    {
                        attackTimer = Time.time + meleeCooldown;
                        ea.Attack();
                    }
                }
                break;
            case CombatStyle.Range:
                if (distance < rangeDistance)
                {
                    if (Time.time > attackTimer)
                    {
                        attackTimer = Time.time + (1f / fireRate);
                        ShootProjectile();
                    }
                }
                break;
            default:
                break;
        }
    }

    private void MeleeHit()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)hitbox.transform.position + hitbox.offset, hitbox.size, targetLayer);
        foreach (var col in cols)
        {
            if (col.CompareTag(targetTag))
            {
                IDamagable damagable = col.GetComponentInParent<IDamagable>();
                if (damagable != null)
                {
                    damagable.Damage(meleeDmg);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlaySFX(meleeSFX);
                    }
                }
            }
        }
    }

    private void ShootProjectile()
    {
        Vector3 shootDirection;
        shootDirection = player.position - transform.position;
        Rigidbody2D bulletInstance = Instantiate(projectile, weaponHolder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        projectile.GetComponent<Projectile>().Set(rangeDmg);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * pjSpeed, shootDirection.y * pjSpeed);
    }

    private void FlipHandler()
    {
        ea.IsRight(direction.x > 0);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeDistance);
    }
#endif
}
