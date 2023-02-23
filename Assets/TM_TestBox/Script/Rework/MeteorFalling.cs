using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFalling : MonoBehaviour
{
    private Transform _transform;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private Animator anim;
    /// <summary>
    /// Visual warning danger destination for Player
    /// </summary>
    [SerializeField] private Transform dangerSign;
    [SerializeField] private AnimationEventReceiptor aer;

    public float height;
    /// <summary>
    /// Between StartPosition to Landing
    /// </summary>
    private float distance;
    [SerializeField] private float minimumDistance = 0.1f;
    [SerializeField] private bool hasImpacted = false;

    [Header("Animation IDs")]
    private int animID_Break;

    [Header("Attributes")]
    [SerializeField] private float fallingSpeed = 10f;
    [SerializeField] private float impactDamage = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag = "Untagged";
    [SerializeField] private Vector2 boxSize;

    [Header("Combat Caches")]
    private Collider2D[] cacheCol;
    private const int cacheSize = 10;

#if UNITY_EDITOR
    [Header("Debug Mode")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private Color boxColor = Color.green;
#endif
    private void Awake()
    {
        _transform = transform;
        AnimationID();
        startPosition = _transform.position;
        endPosition = startPosition - new Vector3(0, height, 0);
        dangerSign.gameObject.SetActive(false);
        dangerSign.parent = null;
        dangerSign.position = endPosition;
        dangerSign.gameObject.SetActive(true);
        cacheCol = new Collider2D[cacheSize];
        if (aer != null)
        {
            aer.OnDied += delegate { gameObject.SetActive(false); Destroy(gameObject, 1f); Destroy(dangerSign.gameObject, 1f); };
        }
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(_transform.position, endPosition);
        if (distance < minimumDistance)
        {
            if (hasImpacted)
            {

            }
            else
            {
                hasImpacted = true;
                int number = Physics2D.OverlapBoxNonAlloc(endPosition, boxSize, 0, cacheCol, targetLayer);
                for (int i = 0; i < number; i++)
                {
                    if (cacheCol[i].CompareTag(targetTag))
                    {
                        IDamagable damagable = cacheCol[i].GetComponentInParent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.TakeDamage(impactDamage);
                        }
                    }
                }
                anim.SetTrigger(animID_Break);
                dangerSign.gameObject.SetActive(false);
            }
        }
        else
        {
            _transform.position += fallingSpeed * Time.fixedDeltaTime * Vector3.down;
        }
    }

    private void AnimationID()
    {
        animID_Break = Animator.StringToHash("Break");
    }

    private IEnumerator Execution(Action callback, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode == false) return; 
        Gizmos.color = boxColor;
        Vector3 pos = transform.position;
        Gizmos.DrawWireCube(pos, boxSize);
        Gizmos.DrawLine(pos, pos - new Vector3(0, height, 0));
    }
#endif
}
