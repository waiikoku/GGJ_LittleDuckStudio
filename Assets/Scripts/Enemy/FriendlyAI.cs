using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAI : Character
{
    public List<EnemyCombat> enemies;
    public string targetTag = "Untagged";
    public Transform shootPoint;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    public float fireRate = 1f; // bullet per second
    private float fireTimestamp;
    [SerializeField] private Animator anim;

    [SerializeField] private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    [Header("SoundInfo")]
    [SerializeField] private string projectileSFX;

    private void FixedUpdate()
    {
        direction = target.position - transform.position;
        rb.velocity = direction.normalized * moveSpeed;
    }

    private void LateUpdate()
    {
        if(Time.time > fireTimestamp)
        {
            fireTimestamp = Time.time + (1f / fireRate);
            ShootProjectile();
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
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            EnemyCombat ec = collision.GetComponentInParent<EnemyCombat>();
            if (enemies.Contains(ec) == false) return;
            enemies.Remove(ec);
        }
    }
    */
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
}
