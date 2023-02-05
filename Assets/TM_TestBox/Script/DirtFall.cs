using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DirtFall : MonoBehaviour
{
    public Transform spawnPosition;
    public Animator anim;
    public GameObject danger;
    public float hight = 40;
    public float delay = 0.5f;
    private bool doDamage = true;
    private Vector3 pos;

    [Header("Atk")]
    public Transform attackPoint;
    public Vector2 attackRangeNormal;
    public LayerMask playerLayer;
    public float fallDamage;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        Debug.Log(spawnPosition.position);
        anim.Play("DirtIdel");
        danger.GetComponent<Danger>().pos = spawnPosition.position - new Vector3(0, 42.5f, 0);
        pos = spawnPosition.position - new Vector3(0, 41.5f, 0);
        attackPoint = danger.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pos);
        if (transform.position.y < pos.y)
        {
            if (doDamage)
            {
                Collider2D[] hit = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeNormal, playerLayer);
                print($"Boss ({hit.Length})");
                foreach (Collider2D enemy in hit)
                {
                    if (enemy.CompareTag("Player"))
                    {
                        print("Dirt Hit Player");
                        IDamagable damagable = enemy.GetComponentInParent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(fallDamage);
                        }
                    }
                }
            }
            anim.Play("DirtBreak 0");
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                Destroy(gameObject);
            }
        }
        else { transform.position += Vector3.down * Time.deltaTime * 10; }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackRangeNormal);
    }
}
