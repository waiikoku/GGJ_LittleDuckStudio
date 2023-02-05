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
    private float delay = 0.24f;
    private bool doDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        Debug.Log(spawnPosition.position);
        anim.Play("DirtIdel");
        danger.GetComponent<Danger>().pos = spawnPosition.position - new Vector3(0, 42.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = spawnPosition.position - new Vector3(0, 40f, 0);
        Debug.Log(pos);
        if (transform.position == pos)
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
        else { transform.position += Vector3.down * Time.deltaTime * 10; }
    }
}
