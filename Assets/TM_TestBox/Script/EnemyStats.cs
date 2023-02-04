using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Stats")]
    public int enemyMaxHp;
    public int enemyHP;
    public int enemyAtk;
    public int enemyDef;

    [Header("Atk")]
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayer;

    private void Awake()
    {
        enemyHP = enemyMaxHp;
    }
    void Attack()
    {
        //animetion code

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hit) 
        { 
            //damageCode
        }
    }
    public void GetDamage(int damage)
    {
        enemyHP -= damage - enemyDef;
    }
}
