using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>();
                if (s_instance == null)
                {
                    var obj = new GameObject(typeof(T).Name);
                    s_instance = obj.AddComponent<T>();
                }
            }
            return s_instance;
        }
    }

    public virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
