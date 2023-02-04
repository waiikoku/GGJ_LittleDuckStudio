using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
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
    private void Start()
    {
        InputManager.Instance.OnLMB += PrimaryAttack;
        OnHealthUpdate += UIManager.Instance.UpdateHealth;
        anim.aer.OnAttack += ShootProjectile;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnLMB -= PrimaryAttack;
    }

    public override void Damage(float dmg)
    {
        print($"{gameObject.name} take {dmg}");
        currentHealth = Mathf.Clamp(currentHealth - dmg, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
    }

    public void Heal(float hp)
    {
        currentHealth = Mathf.Clamp(currentHealth + hp, 0, maxHealth);
        OnHealthUpdate?.Invoke(currentHealth / maxHealth);
    }
    private void ShootProjectile()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = (shootDirection - transform.position).normalized;
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
    }

}
