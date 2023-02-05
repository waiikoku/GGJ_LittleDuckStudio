using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NewBossController : MonoBehaviour
{
    public Transform player;
    private Animator bAnima;
    private Rigidbody2D rigidBody;
    public float speed = 5f;

    [Header("Stats")]
    public int bossMaxHp;
    public int bossHP;
    public int bossAtk;
    public int bossDef;

    [Header("Atk")]
    public Transform attackPoint;
    public float stopWalk;
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
    public bool startAnimation = false;
    public float animationEndTime = 0f;

    [Header ("Condition")]
    public bool moveCon0 = false;
    public bool moveCon1 = false;
    public bool moveCon2 = false;
    public bool moveCon3 = false;

    private void Awake()
    {
        bossHP = bossMaxHp;
        bAnima = GetComponent<Animator>();
    }
    void Start()
    {
        enemyCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        SelectAttack();
        if(moveCon3)
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
        if (moveCon1)
        {
            NormalAttack();
        }
        if (moveCon2)
        {
            ThrowRock();
        }

    }
    void SelectAttack()
    {
        if (Time.time >= chargeAttackNextTime /* && Distance() > chargeRange*/)
        {
            if (MoveConditionCheck(3))
            {
                moveCon3 = true;
            }
        }
        else if (Distance() < chargeRange)
        {
            if (MoveConditionCheck(1))
            {
                moveCon1 = true;    
            }
        }
        else if (Time.time <= chargeAttackNextTime && Distance() > chargeRange)
        {
            if (MoveConditionCheck(2))
            {
                moveCon2 = true;
            }
        }
    }
    void MoveToChargeLocation()
    {
        Debug.Log("MoveToPrepareCharege");
        transform.position = Vector2.MoveTowards(transform.position, chargeLocation.position, Time.deltaTime * speed);
        if (transform.position == chargeLocation.position)
        {
            if(moveCon3)
            {
                StartCoroutine(Chargeing());
            }
            else if (moveCon2)
            {

            }
        }
    }
    IEnumerator Chargeing()
    {
        yield return new WaitForSeconds(2);
        moveToPlayer = true;
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
            Debug.Log("Charge");
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
                StartCoroutine(ResetCondition(3));
            }
        }
    }
    IEnumerator ResetCondition(int con)
    {
        if (con == 3)
        {
            chargeAttackNextTime = Time.time + chargeAttackDelay;
            moveCon3 = false;
            isHit = false;
            getChargeDirection = false;
            stunCondition = false;
            moveToPlayer = false;
            elapsedTime = 0;
        }
        if(con == 1)
        {
            moveCon1 = false;
            startAnimation = true;
            bAnima.Play("Idel");
        }
        yield   return new WaitForSeconds(0.3f); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (MoveConditionCheck(3) && moveToPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Hit Player");
                // dodamage
                isHit = true;
            }
            else if (collision.gameObject.tag == "Wall")
            {
                Debug.Log("Hit Wall");
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
        Debug.Log("NormalAttack");
        if (Distance() <= stopWalk + 5)
        {
            if (!startAnimation)
            {
                bAnima.Play("NormalAttack");
                animationEndTime = 0;
            }
            animationEndTime += Time.deltaTime;
            if(animationEndTime >= 0.51f)
            {
                Debug.Log("Attack");
                Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeNormal, playerLayer);

                foreach (Collider2D enemy in hit)
                {
                    //damageCode
                }
                StartCoroutine(ResetCondition(1));
            }
        }
        if (Distance() >= stopWalk) { Move(speed); }

    }
    void ThrowRock()
    {

    }
    bool MoveConditionCheck(int moveNum)
    {
        bool returner;
        switch (moveNum)
        {
            case 1: if (!moveCon2 && !moveCon3)
                    { returner = true; } 
                    else { returner = false; }
                break;
            case 2: if (!moveCon1 && !moveCon3)
                    { returner = true; } 
                    else { returner = false; }
                break;
            case 3: if (!moveCon1 && !moveCon2)
                    { returner = true; }
                    else { returner = false; }
                break;
                default: returner = false; break;
        }
        return returner;
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
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRangeNormal);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
    }
}
