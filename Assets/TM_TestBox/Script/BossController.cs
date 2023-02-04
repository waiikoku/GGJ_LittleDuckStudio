using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;

    [Header("Stats")]
    public int bossMaxHp;
    public int bossHP;
    public int bossAtk;
    public int bossDef;

    [Header("Atk")]
    public Transform attackPoint;
    public Vector2 attackRangeNormal;
    public LayerMask playerLayer;
    public float chargeRange;
    public float normalRange;
    private bool bossIsStop = false;
    private bool endProcess = false;

    [Header("Timer")]
    public float chargeAttackNextTime;
    public float chargeAttackDelay;
    public float rockAttackNextTime;
    public float rockAttackDelay;
    public float bossStunTime;

    private void Awake()
    {
        bossHP = bossMaxHp;
    }
    private void Update()
    {
        if (!bossIsStop)
        {
            SelectAttack();
        }
    }
    void SelectAttack()
    {
        if (Distance() > normalRange && Distance() < chargeRange)
        {
            Move();
        }
        else if (Distance() < normalRange)
        {
            NormalAttack();
        }
        else if (Time.time >= rockAttackNextTime && Distance() > chargeRange)
        {
            ThrowRock();
        }
    }
    void Move()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    void NormalAttack()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeNormal, playerLayer);

        foreach (Collider2D enemy in hit)
        {
            //damageCode
        }
    }
    void ThrowRock()
    {
        
    }
    bool ChargeAttackCheck()
    {
        if(Time.time >= chargeAttackNextTime && Distance() > chargeRange)
        {
            bossIsStop = true;
            return true;
        }
        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ChargeAttackCheck())
        {
            StartCoroutine(ChargeAttackAnima());
            if (collision.gameObject.tag == "Player")
            {
                //DoDamage
                //make KnockBack
                chargeAttackNextTime = Time.time + chargeAttackDelay;
            }
            else if (collision.gameObject.tag == "Wall")
            {
                StartCoroutine(Stun());
                chargeAttackNextTime = Time.time + chargeAttackDelay;
            }
        }
    }
    IEnumerator ChargeAttackAnima()
    {
        //moveBack method
        yield return new WaitForSeconds(1);// method calcu time to target
        //animetion move
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator Stun()
    {
        //animetion Stun
        yield return new WaitForSeconds(bossStunTime);
        bossIsStop= false;
    }
    float Distance()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        return distance;
    }
    public void GetDamageBack(int damage)
    {
        bossHP -= damage;
    }
    public void GetDamageFront(int damage)
    {
        bossHP -= damage - bossDef;
    }
    private void OnDrawGizmos()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeNormal, playerLayer);
        Gizmos.color = Color.red;
        foreach (Collider2D collider in hit)
        {
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
