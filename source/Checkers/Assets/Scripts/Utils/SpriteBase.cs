using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class SpriteBase : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeOnAwake();
    }

    protected virtual void InitializeOnAwake() { }
}
