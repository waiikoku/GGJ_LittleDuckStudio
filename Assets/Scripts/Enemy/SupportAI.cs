using UnityEngine;

public class SupportAI : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 1f;
    public float stopDistance = 1f;

    public float flySpeed = 10f;
    public float skillRadius = 5f;
    public LayerMask targetLayer;
    public string targetTag = "Untagged";
    public GameObject rootPrefab;

    [Header("Spiral Effect")]
    public float radius = 2f;
    public float speed = 1f;
    public float rotationSpeed = 1f;
    public float timeFlyingAroundPlayer;
    public float flyOutSpeed = 1f;
    private bool flyOut = false;
    private float angle = 0f;
    private Vector2 circlePosition;
    private Vector3 intend;
    private Vector3 flyOutDirection;
    private Vector3 minOffset = new Vector3(0.01f, 0.01f, 0.01f);

    [Header("Attributes")]
    [SerializeField] private PlayerCombat.FairyMode currentSkill;
    public float liveDuration = 3f;
    private PlayerCombat buffOwner;
    private bool skillUsed = false;

    [Header("Imprison Skill")]
    private float lifeTime = 0f;
    public float lockDuration = 5f;

    [Header("Heal Skill")]
    public float healPoint = 100f;
    private void Start()
    {
        timeFlyingAroundPlayer = Time.time + timeFlyingAroundPlayer;
    }

    public void Setup(Transform target,PlayerCombat combat,PlayerCombat.FairyMode skill)
    {
        this.target = target;
        currentSkill = skill;
        buffOwner = combat;
    }

    private void Update()
    {
        if (target == null) return;
        float x = (minOffset.x + target.position.x) + radius * Mathf.Cos(angle);
        float y = (minOffset.y + target.position.y) + radius * Mathf.Sin(angle);
        circlePosition = new Vector2(x, y);

        if (Time.time < timeFlyingAroundPlayer)
        {
            // The object is flying around the player in a circle
            angle += rotationSpeed * Time.deltaTime;
            transform.position = circlePosition;
            transform.LookAt(circlePosition);
        }
        else
        {
            // The object is spinning and flying out
            flyOutDirection = (transform.position - target.position).normalized;
            intend = flyOutDirection * flyOutSpeed * Time.deltaTime;
            transform.position += intend;
            if (skillUsed == false)
            {
                skillUsed = true;
                switch (currentSkill)
                {
                    case PlayerCombat.FairyMode.Imprision:
                        Skill_Roots();
                        break;
                    case PlayerCombat.FairyMode.Healing:
                        Skill_Heal();
                        break;
                    default:
                        break;
                }
            }
            if (flyOut == false)
            {
                flyOut = true;
            }
        }

    }

    private void OnBecameInvisible()
    {
        if (flyOut)
        {
            Destroy(gameObject);
        }
    }

    public void Skill_Roots()
    {
        Vector3 pos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, skillRadius, targetLayer);
        foreach (var col in cols)
        {
            if (col.CompareTag(targetTag))
            {
                GameObject go = Instantiate(rootPrefab, col.transform.position, Quaternion.identity);
                EnemyAnimator ea = col.GetComponent<EnemyAnimator>();
                if (ea != null)
                {
                    ea.Freeze(true, lockDuration);
                }
                Destroy(go, lockDuration);
            }
        }
    }

    private void Skill_Heal()
    {
        buffOwner.Heal(healPoint);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
#endif
}
