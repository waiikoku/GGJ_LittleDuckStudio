using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class DirtFall : MonoBehaviour
{
    public Transform spawnPosition;
    public Animator anim;
    public GameObject danger;
    public float hight = 40;
    private float delay = 0.24f;
    private bool doDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        anim.Play("DirtIdel");
    }

    // Update is called once per frame
    void Update()
    {
        danger.transform.position = spawnPosition.position - new Vector3(0, -2.5f, 0) + Vector3.down * hight;
        transform.position += Vector3.down* Time.deltaTime;
        if (transform.position == spawnPosition.position + Vector3.down * hight)
        {
            if ( doDamage)
            {

            }
            anim.Play("DirtBreak");
            delay -= Time.deltaTime;
            if(delay < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
