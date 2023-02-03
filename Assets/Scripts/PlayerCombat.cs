using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform wandHolder;
    public Rigidbody2D projectile;
    public float speed;
    public float dmg;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 shootDirection;
            shootDirection = Input.mousePosition;
            shootDirection.z = 0.0f;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            shootDirection = shootDirection - transform.position;
            //...instantiating the rocket
            Rigidbody2D bulletInstance = Instantiate(projectile, wandHolder.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            bulletInstance.gameObject.SetActive(false);
            projectile.GetComponent<Projectile>().Set(dmg);
            bulletInstance.gameObject.SetActive(true);
            bulletInstance.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);
        }
    }
}
