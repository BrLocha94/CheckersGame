using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Warning: Instance not initialized");

            return _instance;
        }

        private set { }
    }

    void Awake()
    {
        _instance = this as T;

        InitializeOnAwake();
    }

    protected virtual void InitializeOnAwake() { }
}
