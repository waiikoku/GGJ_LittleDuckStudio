using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject manager;

    private void Awake()
    {
        GameObject go = Instantiate(manager, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(go);
        go.name = "Managers";
    }
}
