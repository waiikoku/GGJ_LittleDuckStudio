using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MniAssistant : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 cache;
    public float smoothTime = 1f;
    [SerializeField] private PlayerCombat player;
    [SerializeField] private ParticleSystem healPS;
    [SerializeField] private float healCooldown = 1f;
    private float healTimer;
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref cache, smoothTime);
    }

    private void LateUpdate()
    {
        if(Time.time > healTimer)
        {
            healTimer = Time.time + healCooldown;
            healPS.Play();
            player.Heal(100);
        }
    }
}
