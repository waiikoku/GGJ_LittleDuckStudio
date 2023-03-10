using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBossController : CharacterCombat
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
    public float normalAtk;
    public float chargeDamage;

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
    private bool throwRock = false;

    [Header ("DropRock")]
    public GameObject rock;
    public float dropRockHight = 5f;

    [Header("Timer")]
    public float chargeAttackNextTime;
    public float chargeAttackDelay;
    public float stunTime = 8f;
    public float elapsedTime = 0f;
    public bool startAnimation = false;
    public float fixedAET;
    public float animationEndTime = 0f;

    [Header ("Condition")]
    public bool moveCon1 = false;
    public bool moveCon2 = false;
    public bool moveCon3 = false;
    public bool faceRight = false;

    private ZoneManager.LimitInfo limitInfo;
    [SerializeField] private Transform head;

    private void Awake()
    {
        bossHP = bossMaxHp;
        bAnima = GetComponent<Animator>();
    }
    void Start()
    {
        enemyCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        limitInfo = ZoneManager.Instance.GetInfo();
    }
    private void Update()
    {
        if(player == null)
        {
            this.enabled = false;
            return;
        }
        if (bossHP <= 0)
        {
            bAnima.Play("Die");
        }
        else
        {
            SelectAttack();
            if (moveCon3)
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
                if (!throwRock)
                {
                    MoveToChargeLocation();
                }
                else
                { ThrowRock(); }
            }
        }
    }

    private void FixedUpdate()
    {
        //ZoneManager.Instance.LimitYPosition(transform, limitInfo.MinY, limitInfo.MaxY);
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
        FlipCheck(chargeLocation);
        if (transform.position == chargeLocation.position)
        {
            if(moveCon3)
            {
                StartCoroutine(Chargeing());
            }
            else if (moveCon2)
            {
                FlipCheck(player);
                throwRock = true;
            }
        }
    }
    IEnumerator Chargeing()
    {
        bAnima.Play("Chargeing");
        FlipCheck(player);
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
            if (!startAnimation)
            {
                bAnima.Play("PreCrush");
                animationEndTime = 0;
            }
            animationEndTime += Time.deltaTime;
            if (animationEndTime >= 0.1f)
            {
                bAnima.Play("Crush");
            }
                Debug.Log("Charge");
            rigidBody.velocity = direction * chargeSpeed;
        }
        if (stunCondition)
        {
            bAnima.Play("Stun");
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
        }
        if (con == 2)
        {
            moveCon2 = false;
            startAnimation = false;
        }
        startAnimation = false;
        bAnima.Play("Idel");
        yield   return new WaitForSeconds(0.3f); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (MoveConditionCheck(3) && moveToPlayer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Hit Player");
                IDamagable damagable = collision.collider.GetComponentInParent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(chargeDamage);
                }
                isHit = true;
                StartCoroutine(ResetCondition(3));
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Hit Wall");
                isHit = true;
                stunCondition = true;
                startAnimation = false;
            }

        }
    }
    void Move(float mSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * mSpeed);
        FlipCheck(player.transform);
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
                startAnimation = true;
            }
            animationEndTime += Time.deltaTime;
            if(animationEndTime >= 0.51f)
            {
                Debug.Log("Attack");
                Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeNormal, playerLayer);
                print($"Boss ({hit.Length})");
                foreach (Collider2D enemy in hit)
                {
                    if (enemy.CompareTag("Player"))
                    {
                        print("Boss Hit Player");
                        IDamagable damagable = enemy.GetComponentInParent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.TakeDamage(normalAtk);
                        }
                    }
                }
                StartCoroutine(ResetCondition(1));
            }
        }
        if (Distance() >= stopWalk) { Move(speed); }

    }
    void ThrowRock()
    {
        if (!startAnimation)
        {
            bAnima.Play("Throw");
            animationEndTime = 0;
            startAnimation = true;
        }
        animationEndTime += Time.deltaTime;
        if (animationEndTime >= 0.30f)
        {
            bAnima.Play("Idel");
            if (player == null) return;
            GameObject r = Instantiate(rock, player.position + Vector3.up * dropRockHight, Quaternion.identity);
            StartCoroutine(ResetCondition(2));
        }
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
        if (player == null) return 9999;
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
    void FlipCheck(Transform target)
    {
        float dis = transform.position.x - target.position.x;
        if (dis < 0 && !faceRight)
        { Flip(); faceRight = !faceRight; }
        if (dis > 0 && faceRight)
        { Flip(); faceRight = !faceRight; }

    }
    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRangeNormal);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
    }

    public override void TakeDamage(float dmg)
    {
        bossHP -= (int)dmg;
        if (bossHP <= 0)
        {
            //GameManager.Instance.Victory();
            UIManager.Instance.SetGUI(false);
            SceneLoader.Instance.LoadScene("Cutscene");
            SoundManager.Instance.StopBGM();
        }
        OnHealthUpdate?.Invoke((float)bossHP / (float)bossMaxHp);
    }
}
