using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject manager;
    [SerializeField] private string menuName;
    private void Awake()
    {
        GameObject go = Instantiate(manager, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(go);
        go.name = "Managers";
        SceneManager.LoadScene(menuName);
    }
}
