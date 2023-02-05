using UnityEngine;

public class SupportAI : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity;
    private Vector3 direction;
    private Vector3 destination;
    public float smoothTime = 1f;
    public float stopDistance = 1f;

    public float flySpeed = 10f;
    public float skillRadius = 5f;
    public LayerMask targetLayer;
    public string targetTag = "Untagged";
    public GameObject rootPrefab;

    public float liveDuration = 3f;
    private float lifeTime = 0f;
    public float lockDuration = 5f;
    private bool skillUsed = false;
    public enum Phase
    {
        None,
        Follow,
        FlyIn,
        FlyOut
    }
    public Phase assistPhase = Phase.None;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            target = go.transform;
            assistPhase = Phase.Follow;
        }
    }

    private void FixedUpdate()
    {
        switch (assistPhase)
        {
            case Phase.Follow:
                FollowTarget();
                break;
            case Phase.FlyIn:
            case Phase.FlyOut:
                MovePosition();
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (assistPhase)
        {
            case Phase.Follow:
                if(direction.magnitude < 5f)
                {
                    if (skillUsed == false)
                    {
                        skillUsed = true;
                        Skill_Roots();
                        FlyAway();
                        return;
                    }
                }
                lifeTime += Time.deltaTime;
                if (lifeTime > liveDuration)
                {
                    FlyAway();
                }
                break;
            default:
                break;
        }
        /*

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Skill_Roots();
        }
        */
    }

    private void FollowTarget()
    {
        direction = target.position - transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, target.position - (direction.normalized * stopDistance), ref velocity, smoothTime);
    }

    private void MovePosition()
    {
        transform.position = Vector3.Lerp(transform.position, destination, flySpeed * Time.deltaTime);
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

    private void FlyAway()
    {
        destination = transform.position + (new Vector3(1, 1, 0) * 100);
        assistPhase = Phase.FlyOut;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
#endif
}
