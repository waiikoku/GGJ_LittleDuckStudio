using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform player;
    private Vector3 direction;
    public float speed = 1;
    public float stopDistance = 1;
    public float distance;

    [SerializeField] private Transform weaponHolder;
    public float attackDistance = 1;
    public float attackRate = 1; //Per second
    private float attackTimer;
    public Rigidbody2D projectile;
    public float pjSpeed;
    public float damage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        direction = player.position - transform.position;
        distance = direction.magnitude;
        if (distance < stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = direction.normalized * speed;
    }

    private void LateUpdate()
    {
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
        //...instantiating the rocket
        Rigidbody2D bulletInstance = Instantiate(projectile, weaponHolder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        projectile.GetComponent<Projectile>().Set(damage);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * pjSpeed, shootDirection.y * pjSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
