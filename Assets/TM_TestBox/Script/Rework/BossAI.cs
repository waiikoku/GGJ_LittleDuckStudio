using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : CharacterCombat
{
    private Transform _transform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D hitInflict;
    [SerializeField] private AnimationEventReceiptor aer;

    [Header("Movement")]
    public float currentSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float stunSpeed;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private float distanceFromHoe;

    private Transform target;
    /// <summary>
    /// Distance from target
    /// </summary>
    private float distance;
    private Vector3 targetDirection;
    private bool lastFacingRight;

    [Header("Custom Move")]
    private Vector3 destination;
    private Vector3 destDirection;
    private float destDistance;
    [SerializeField] private float topY = 0;
    [SerializeField] private float bottomY = -20;

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    public int maxHealth;
    public float nAtkDamage; //Normal Attack
    public float cAtkDamage; //Charge Attack

    [Header("Combat")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private string obstacleTag = "Untagged";
    [SerializeField] private float normalAttackDistance = 5f;
    /// <summary>
    /// Distance to check if player in this range will not be charge
    /// </summary>
    [SerializeField] private float chargeAttackDistance = 10f;
    [SerializeField] private float meteorAttackDistance = 20f;
    [SerializeField] private float stunDuration = 5f;

    [Header("Charge Parameters")]
    /// <summary>
    /// Distance to leave distance before begin to charge
    /// </summary>
    [SerializeField] private float chargeDistance = 10f;
    /// <summary>
    /// Start Position Before Charging
    /// </summary>
    [SerializeField] private Vector3 chargePosition;
    private bool hasLastPosition = false;
    /// <summary>
    /// Capture Target Position Then Charging to that position
    /// </summary>
    [SerializeField] private Vector3 lastPosition;
    /// <summary>
    /// Check if reached chargePosition
    /// </summary>
    [SerializeField] private bool readyToCharge = false;

    [Header("Meteor")]
    [SerializeField] private bool canMeteor = false;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float dropHeight = 5f;
    [SerializeField] private float dropingDuration = 15f;
    [SerializeField] private int dropAmount = 1;
    private int dropCount = 0;

    [Header("Timer")]
    public float normalAttackCooldown = 3f;
    public float chargeAttackCooldown = 15f;
    public float dropMeteorCooldown = 60f;
    private float nAtkTimer; //Normal Attack Timer
    private float cAtkTimer; //Charge Attack Timer
    private float meteorTimer;
    private float dropTimer;
    private enum State
    {
        None,
        Moving,
        NormalAttack,
        Charging,
        ChargePerformed,
        DropingMeteor,
        Stunned,
        Died
    }
    [SerializeField] private State currentPhase;
    private State lastPhase;

    private ZoneManager.LimitInfo limitSpace;


#if UNITY_EDITOR
    [Header("Debug Mode")]
    [SerializeField] private bool debugMode = false;
#endif

    private void Awake()
    {
        _transform = transform;
        aer.OnAttack += FrameNormalAttack;
    }

    private void Start()
    {
        cAtkTimer = Time.time + chargeAttackCooldown;
        meteorTimer = Time.time + dropMeteorCooldown;
        StartCoroutine(DetectTarget());
        CharacterLayerManager.Instance.Add(sr);
    }

    private IEnumerator DetectTarget()
    {
        while (target == null)
        {
            target = GameObject.FindObjectOfType<PlayerMovement>().transform;
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (CheckTarget())
        {
            CheckDistance();
            SetMove(true);
            if (transform.position.y > topY)
            {
                rb.velocity = Vector2.zero;
            }
            if (transform.position.y < bottomY)
            {
                rb.velocity = Vector2.zero;
            }
            switch (currentPhase)
            {
                case State.Moving:
                    MoveHandle();
                    FlipHandle(targetDirection.x > 0.1f);
                    if (distance < normalAttackDistance)
                    {
                        ChangePhase(State.NormalAttack);
                        return;
                    }
                    break;
                case State.NormalAttack:
                    NormalAttack();
                    break;
                case State.Charging:
                    Charging();
                    break;
                case State.DropingMeteor:
                    DropMeteor();
                    break;
                case State.Stunned:
                    Stunned();
                    break;
                case State.Died:
                    break;
                default:
                    break;
            }
        }
        else
        {
            SetMove(false);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {

        }
        else
        {
            CheckSkillParameter();
        }
    }

    public override void TakeDamage(float dmg)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / (float)maxHealth);
        if (currentHealth == 0)
        {
            ChangePhase(State.Died);
            CharacterLayerManager.Instance.Remove(sr);
        }
    }

    private void NormalAttack()
    {
        if (distance > normalAttackDistance)
        {
            ChangePhase(State.Moving);
            return;
        }
        nAtkTimer -= Time.deltaTime;
        if (Time.time > nAtkTimer)
        {
            nAtkTimer = normalAttackCooldown;
            anim.SetTrigger("nAttack");
        }
    }

    private void FrameNormalAttack()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitInflict.transform.position, (hitInflict.size * hitInflict.transform.localScale) / 2f, 0, targetLayer);
        print($"Detect Hitbox ({cols.Length})");
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag(targetTag))
            {
                print($"Found Target!");
                IDamagable damagable = target.GetComponentInParent<IDamagable>();
                if (damagable != null)
                {
                    print("Hit Player!");
                    ApplyDamage(damagable, nAtkDamage);
                }
                break;
            }
        }

    }
    private void Charging()
    {
        if (readyToCharge)
        {
            if (hasLastPosition)
            {
                anim.SetBool("Charging", true);
                MoveHandle(lastPosition);
                if (destDistance < 1f)
                {
                    anim.SetBool("Charging", false);
                    readyToCharge = false;
                    lastPosition = Vector2.zero;
                    hasLastPosition = false;
                    cAtkTimer = Time.time + chargeAttackCooldown;
                    ChangePhase(State.ChargePerformed); //Nothing to Impact
                    return;
                }
            }
            else
            {
                hasLastPosition = true;
                lastPosition = target.position;
            }

        }
        else
        {
            MoveHandle(chargePosition);
            float dist = Vector3.Distance(chargePosition, _transform.position);
            if (dist < 1f)
            {
                readyToCharge = true;
                lastPosition = target.position;
                hasLastPosition = true;
                FlipHandle(targetDirection.x > 0.1f); //Facing Target
                return;
            }
        }
    }

    private void ApplyDamage(IDamagable target, float damage)
    {
        target.TakeDamage(damage);
    }

    private void Stunned()
    {
        anim.SetBool("Stun", true);
    }

    private void DropMeteor()
    {
        if (dropCount < dropAmount)
        {
            if (Time.time > dropTimer)
            {
                anim.SetTrigger("Throw");
                dropTimer = Time.time + (dropingDuration / (float)dropAmount);
                Instantiate(meteorPrefab, target.position + new Vector3(0, dropHeight, 0), Quaternion.identity);
                dropCount++;
            }
        }
        else
        {
            dropCount = 0;
            meteorTimer = Time.time + dropMeteorCooldown;
            ChangePhase(State.Moving);
        }
    }

    private bool CheckTarget()
    {
        if (target == null)
        {
            return false;
        }
        else
        {
            targetDirection = target.position - _transform.position;
            return true;
        }
    }

    private void CheckDistance()
    {
        distance = Vector3.Distance(_transform.position, target.position);
    }

    private void CheckSkillParameter()
    {
        float currentTime = Time.time;
        float tempDistance = distance;
        if (currentPhase == State.DropingMeteor)
        {

        }
        else
        {
            if(currentPhase.HasFlag(State.Charging) == false && currentPhase.HasFlag(State.ChargePerformed) == false && currentPhase.HasFlag(State.Stunned) == false)
            {
                if (currentTime > meteorTimer)
                {
                    if (tempDistance > meteorAttackDistance)
                    {
                        meteorTimer = Mathf.Infinity; //Not Cooldown Until Meteor Stop
                        ChangePhase(State.DropingMeteor);
                        return;
                    }
                }
            }
            if (currentPhase == State.Moving)
            {
                if (currentTime > cAtkTimer)
                {
                    if (tempDistance > chargeAttackDistance)
                    {
                        cAtkTimer = Mathf.Infinity; //Not Cooldown Until ChargeAttacked OR StunByWall
                        ChangePhase(State.Charging);
                        return;
                    }
                }
            }
        }
    }

    private void MoveHandle()
    {
        rb.velocity = targetDirection.normalized * currentSpeed;
    }

    private void MoveHandle(Vector3 desirePosition)
    {
        destination = desirePosition;
        destDirection = destination - _transform.position;
        destDistance = destDirection.magnitude;
        rb.velocity = destDirection.normalized * currentSpeed;
    }

    private void SetMove(bool canMove)
    {
        if (canMove)
        {
            switch (currentPhase)
            {
                case State.Moving:
                    currentSpeed = distance < stopDistance ? 0 : normalSpeed;
                    if (currentSpeed == 0)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    break;
                case State.NormalAttack:
                    currentSpeed = normalSpeed;
                    break;
                case State.Charging:
                    if (readyToCharge)
                    {
                        float diff = Mathf.Abs(target.position.y - _transform.position.y);
                        if (diff < 1f)
                        {
                            currentSpeed = distance < distanceFromHoe ? 0 : chargeSpeed;
                        }
                        else
                        {
                            currentSpeed = chargeSpeed;
                        }
                    }
                    else
                    {
                        currentSpeed = chargeSpeed;
                    }
                    break;
                case State.ChargePerformed:
                    currentSpeed = normalSpeed;
                    break;
                case State.DropingMeteor:
                    currentSpeed = 0;
                    break;
                case State.Stunned:
                    currentSpeed = 0;
                    break;
                case State.Died:
                    currentSpeed = 0;
                    target = null;
                    break;
                default:
                    break;
            }
        }
        else
        {
            currentSpeed = 0;
            rb.velocity = Vector2.zero;
        }
    }

    private void ChangePhase(State nextPhase)
    {
        lastPhase = currentPhase;
        switch (nextPhase)
        {
            case State.NormalAttack:
                break;
            case State.Charging:
                hasLastPosition = false;
                float tempDistance = distance;
                if (tempDistance < chargeDistance)
                {
                    readyToCharge = false;
                    print("Less than distance. Leave distance");
                    var dstMinusCharge = chargeDistance - tempDistance;
                    Vector2 temp = -targetDirection.normalized * Mathf.Clamp(dstMinusCharge, 0, Mathf.Infinity);
                    Vector2 temp2 = (Vector2)_transform.position;
                    chargePosition = FindIntersection(temp2, temp2 + temp); ;
                }
                else
                {
                    print("More than distance. begin to charge");
                    chargePosition = _transform.position;
                    readyToCharge = true;
                }
                break;
            case State.ChargePerformed:
                lastPosition = _transform.position;
                StartCoroutine(ChargedAttack());
                break;
            case State.DropingMeteor:
                dropCount = 0;
                break;
            case State.Stunned:
                break;
            case State.Died:
                anim.SetBool("Died", true);
                break;
            default:
                break;
        }
        currentPhase = nextPhase;
    }
    Vector2 FindIntersection(Vector2 startPoint, Vector2 endPoint)
    {
        // หาค่า delta_y จากตำแหน่ง y ที่กำหนดและ y ของจุดเริ่มต้นของเส้น
        float delta_y = topY - startPoint.y;
        float delta_y2 = bottomY - startPoint.y;

        // หาค่า t โดยหาร delta_y ด้วยค่า y ของเส้น
        float t = delta_y / (endPoint.y - startPoint.y);
        float t2 = delta_y2 / (endPoint.y - startPoint.y);

        // ตรวจสอบว่าจุดตัดอยู่ในช่วงเส้นหรือไม่
        if (t >= 0f && t <= 1f)
        {
            // นำค่า t ไปคูณกับ vector ของเส้น แล้วบวกกับ vector ของจุดเริ่มต้นของเส้น
            Vector2 intersectionPoint = startPoint + t * (endPoint - startPoint);
            return intersectionPoint;
        }
        else if (t2 >= 0f && t2 <= 1f)
        {
            Vector2 intersectionPoint = startPoint + t2 * (endPoint - startPoint);
            return intersectionPoint;
        }
        else
        {
            // ไม่อยู่ในช่วงเส้น คืนจุดเริ่มต้นของเส้นแทน
            return endPoint;
        }
    }
    private void FlipHandle(bool isRight)
    {
        if (lastFacingRight == isRight) return;
        if (isRight)
        {
            _transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        lastFacingRight = isRight;
    }

    private IEnumerator WallHit()
    {
        Stunned();
        yield return new WaitForSeconds(stunDuration);
        cAtkTimer = Time.time + chargeAttackCooldown;
        ChangePhase(State.Moving);
        anim.SetBool("Stun", false);
    }

    private IEnumerator ChargedAttack()
    {
        cAtkTimer = Time.time + chargeAttackCooldown;
        yield return new WaitForSeconds(3f);
        ChangePhase(State.Moving);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentPhase == State.Charging)
        {
            if (readyToCharge)
            {
                if (collision.collider.CompareTag(obstacleTag))
                {
                    if (currentPhase == State.Stunned)
                    {

                    }
                    else
                    {
                        anim.SetBool("Charging", false);
                        ChangePhase(State.Stunned);
                        StartCoroutine(WallHit());
                    }
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (currentPhase == State.Charging)
        {
            if (readyToCharge == false) return;
            if (collision.CompareTag(targetTag))
            {
                print("Charge hit Player!");
                IDamagable damagable = collision.GetComponentInParent<IDamagable>();
                if (damagable != null)
                {
                    ApplyDamage(damagable, cAtkDamage);
                }
                anim.SetBool("Charging", false);
                anim.Play("Pre-Crush");
                //anim.SetTrigger("cAttack");
                rb.velocity = Vector2.zero;
                ChangePhase(State.ChargePerformed);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chargeAttackDistance);
            if (target == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawLine(transform.position, lastPosition);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, chargePosition);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(chargePosition, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastPosition, 0.5f);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(hitInflict.transform.position, hitInflict.size * hitInflict.transform.localScale);
        }
    }
#endif
}
