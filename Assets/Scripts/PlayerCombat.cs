using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    public Transform wandHolder;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    public float currentHealth;
    public float maxHealth;

    public int hitCount;
    [SerializeField] private PlayerAnimator anim;

    [Header("SoundInfo")]
    [SerializeField] private string projectileSFX;
    [SerializeField] private string wiggleSFX;

    [Header("Skills")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag;
    [SerializeField] private GameObject fairySupport;
    public float fairyConditionHp = 0.5f;
    public float fairyCooldown = 60f;
    public bool fairyUsed = false;
    [SerializeField] private bool fairyRootAvailable;
    [SerializeField] private bool fairyHealAvailable;
    private float fairyTimestamp;
    public float wiggleRadius = 5f;

    [SerializeField] private GameObject smileySupport;
    public int smileyCondition = 5; //Root
    public float smileyDuration = 10f;
    private float smileyTimestamp;
    public bool smileyActive = false;
    public float spawnDistance = 5f;

    private Queue<Vector2> attackQueue;

    [Header("Keybinds")]
    [SerializeField] private KeyCode fairyRoot = KeyCode.Q;
    [SerializeField] private KeyCode fairyHeal = KeyCode.E;
    private void Start()
    {
        InputManager.Instance.OnLMB += PrimaryAttack;
        if (UIManager.Instance != null)
        {
            OnHealthUpdate += UIManager.Instance.UpdateHealth;
        }
        anim.aer.OnAttack += ShootProjectile;
        InputManager.Instance.OnSprinkle += WiggleSkill;
        GameManager.Instance.OnRootChange += GM_SpawnSmiley;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnLMB -= PrimaryAttack;
        OnHealthUpdate -= UIManager.Instance.UpdateHealth;
        anim.aer.OnAttack -= ShootProjectile;
        InputManager.Instance.OnSprinkle -= WiggleSkill;
        GameManager.Instance.OnRootChange -= GM_SpawnSmiley;
    }


    private void Update()
    {
        if (Input.GetKeyDown(fairyRoot))
        {
            SpawnFairy(FairyMode.Imprision);
        }
        if (Input.GetKeyDown(fairyHeal))
        {
            SpawnFairy(FairyMode.Healing);
        }
    }

    private void LateUpdate()
    {
        
    }

    private void GM_SpawnSmiley(int amount)
    {
        SpawnSmiley(amount);
    }

    public override void TakeDamage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
        //ConditionSkill();
        if(currentHealth == 0)
        {
            GameManager.Instance.Gameover();
            Destroy(gameObject);
        }
    }

    public void Heal(float hp)
    {
        currentHealth = Mathf.Clamp(currentHealth + hp, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
    }
    private void ShootProjectile()
    {
        Vector3 shootDirection;
        shootDirection = InputManager.Instance.mousePos;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

        shootDirection = (shootDirection - transform.position);

        Rigidbody2D bulletInstance = Instantiate(projectile, wandHolder.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bulletInstance.gameObject.SetActive(false);
        Projectile pfb = projectile.GetComponent<Projectile>();
        pfb.Set(dmg);
        bulletInstance.gameObject.SetActive(true);
        bulletInstance.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
        SoundManager.Instance.PlaySFX(projectileSFX);
    }

    private void PrimaryAttack(bool value)
    {
        if (value == false) return;
        anim.Attack();
        //attackQueue.Enqueue(InputManager.Instance.mousePos);
    }

    private void WiggleSkill(bool value)
    {
        if (value == false) return;
        anim.SetWand(false);
        anim.TriggerWaggle();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(wiggleSFX);
        }
        Vector3 pos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, wiggleRadius, targetLayer);
        foreach (var col in cols)
        {
            if (col.CompareTag(targetTag))
            {
                FriendlyAI buddy = col.GetComponentInParent<FriendlyAI>();
                if (buddy != null)
                {
                    buddy.CommandToAttack(true);

                }
            }
        }
    }

    public enum FairyMode
    {
        None,
        Imprision,
        Healing
    }
    private void SpawnFairy(FairyMode mode)
    {
        switch (mode)
        {
            case FairyMode.Imprision:
                if (fairyRootAvailable == false) return;
                fairyRootAvailable = false;
                break;
            case FairyMode.Healing:
                if (fairyHealAvailable == false) return;
                fairyHealAvailable = false;
                break;
            default:
                break;
        }
        GameObject go = Instantiate(fairySupport, transform.position, Quaternion.identity);
        go.SetActive(false);
        go.GetComponent<SupportAI>().Setup(transform, this, mode);
        go.SetActive(true);
        StartCoroutine(FairyThread(mode));
    }

    private void SpawnSmiley(int amount)
    {
        if (amount < smileyCondition) return;
        if (smileyActive) return;
        GameManager.Instance.UseRoot(5);
        Vector2 pos = (Vector2)transform.position + (Random.insideUnitCircle * spawnDistance);
        GameObject go = Instantiate(smileySupport, pos, Quaternion.identity);
        smileyActive = true;
        StartCoroutine(SmileyThread(go));

    }

    private IEnumerator SmileyThread(GameObject go)
    {
        yield return new WaitForSeconds(smileyDuration);
        Destroy(go);
        smileyActive = false;
        if(GameManager.Instance.GetRoot() == 5)
        {
            GM_SpawnSmiley(5);
        }
    }
    private float[] pfairyRoot = new float[2] { 0, 0 };
    private float[] pfairyHeal = new float[2] { 0, 0 };
    private IEnumerator FairyThread(FairyMode mode)
    {
        float timer = 0;
        switch (mode)
        {
            case FairyMode.Imprision:
                pfairyRoot[1] = fairyCooldown;
                while (true)
                {
                    timer += Time.deltaTime;
                    pfairyRoot[0] = timer;
                    GameManager.Instance.OnRootSkillUpdate(pfairyRoot);
                    if (timer > fairyCooldown)
                    {
                        break;
                    }
                    yield return null;
                }
                fairyRootAvailable = true;
                break;
            case FairyMode.Healing:
                pfairyHeal[1] = fairyCooldown;
                while (true)
                {
                    timer += Time.deltaTime;
                    pfairyHeal[0] = timer;
                    GameManager.Instance.OnHealSkillUpdate(pfairyHeal);
                    if (timer > fairyCooldown)
                    {
                        break;
                    }
                    yield return null;
                }
                fairyHealAvailable = true;
                break;
            default:
                break;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wiggleRadius);
    }
#endif
}
