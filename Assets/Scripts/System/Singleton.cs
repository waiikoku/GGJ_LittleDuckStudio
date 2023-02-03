using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{
    private static T m_instance;
    public static T Instance => m_instance;

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
