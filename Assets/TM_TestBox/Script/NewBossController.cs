using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NewBossController : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rigidBody;
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

    [Header ("ChargeAtk")]
    public float chargeRange;
    public Transform chargeLocation;
    private bool moveToPlayer = false;
    public bool stunCondition = false;
    public bool isHit = false;
    private bool getChargeDirection = false;
    private Vector2 direction;
    public float chargeSpeed = 5f;
    private BoxCollider2D enemyCollider;

    [Header("Timer")]
    public float chargeAttackNextTime;
    public float chargeAttackDelay;
    public float stunTime = 8f;
    public float elapsedTime = 0f;

    [Header ("Condition")]
    public bool moveCon0 = false;
    public bool moveCon1 = false;
    public bool moveCon2 = false;
    public bool moveCon3 = false;

    private void Awake()
    {
        bossHP = bossMaxHp;
    }
    void Start()
    {
        enemyCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Distance() < chargeRange)
        {
            if (MoveConditionCheck(1))
            {
                NormalAttack();
            }
        }
        else if (Time.time <= chargeAttackNextTime && Distance() > chargeRange)
        {
            if (MoveConditionCheck(2))
            {
                ThrowRock();
            }
        }
        else if (Time.time >= chargeAttackNextTime && Distance() > chargeRange)
        {
            if(MoveConditionCheck(3))
            {
                if (!moveToPlayer)
                {
                    MoveToChargeLocation();
                }
                else
                {
                    MoveToPlayer();
                }
            }
        }
    }
    
    void MoveToChargeLocation()
    {
        transform.position = Vector2.MoveTowards(transform.position, chargeLocation.position, Time.deltaTime);
        if (transform.position == chargeLocation.position)
        {
            moveToPlayer = true;
        }
    }
    void MoveToPlayer()
    {
        if (!getChargeDirection)
        {
            direction = (player.position - transform.position).normalized;
            getChargeDirection = true;
        }
        if (!isHit)
        {
            rigidBody.velocity = direction * chargeSpeed;
        }
        if (stunCondition)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 5f)
            {
                rigidBody.velocity = Vector2.zero;
            }
            if (elapsedTime >= stunTime)
            {
                elapsedTime= 0;
                stunCondition = false;
                moveCon3 = false;
                moveToPlayer = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (MoveConditionCheck(3) && moveToPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                // dodamage
                isHit = true;
            }
            else if (collision.gameObject.tag == "Wall")
            {
                isHit = true;
                stunCondition = true;
            }

        }
    }
    void Move(float mSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * mSpeed);
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
    bool MoveConditionCheck(int moveNum)
    {
        if (!moveCon0 && !moveCon1 && !moveCon2 && !moveCon3)
        {
            return true;
        }
        else if (moveCon0 && moveNum == 0)
        {
            return true;
        }
        else if(moveCon1 && moveNum == 1)
        {
            return true;
        }
        else if (moveCon2 && moveNum == 2)
        {
            return true;
        }
        else if (moveCon3 && moveNum == 3)
        {
            return true;
        }
        return false;
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
