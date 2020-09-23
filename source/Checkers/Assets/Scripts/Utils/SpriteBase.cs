using UnityEngine;

public abstract class SpriteBase : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeOnAwake();
    }

    protected virtual void InitializeOnAwake() { }
}
