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

    public bool isMelee = false;

    [Header("Range")]
    public bool isRange = false;
    [SerializeField] private Transform weaponHolder;
    public float attackDistance = 1;
    public float attackRate = 1; //Per second
    private float attackTimer;
    public Rigidbody2D projectile;
    public float pjSpeed;
    public float damage;
    private ZoneManager.LimitInfo limitInfo;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        ZoneManager.Instance.LimitYPosition(transform, limitInfo.MinY, limitInfo.MaxY);
        direction = player.position - transform.position;
        distance = direction.magnitude;
        FlipHandler();
        if (distance < stopDistance)
        {
            rb.velocity = Vector2.zero;
            rb.drag = 1f;
            return;
        }
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        rb.drag = 0f;
    }


    private void LateUpdate()
    {
        if (isRange == false) return;
        if (distance < attackDistance)
        {
            if(Time.time > attackTimer)
            {
                attackTimer = Time.time + (1f / attackRate);
                ShootProjectile();
            }
        }
    }

    private void ShootProjectile()
    {
        Vector3 shootDirection;
        shootDirection = player.position - transform.position;
        Rigidbody2D bulletInstance = Instantiate(projectile, weaponHolder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        projectile.GetComponent<Projectile>().Set(damage);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * pjSpeed, shootDirection.y * pjSpeed);
    }

    private void FlipHandler()
    {
        ea.IsRight(direction.x > 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
