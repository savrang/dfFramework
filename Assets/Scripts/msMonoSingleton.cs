using UnityEngine;
using System.Collections;

public abstract class msMonoSingleton<T> : MonoBehaviour where T : msMonoSingleton<T>
{

    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    _instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    _instance.Init();
                }

            }
            return _instance;
        }
    }

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this as T;
        }

        Init();
    }

    public virtual void Init() 
    { 
    }


    private void OnApplicationQuit()
    {
        _instance = null;
    }
}


