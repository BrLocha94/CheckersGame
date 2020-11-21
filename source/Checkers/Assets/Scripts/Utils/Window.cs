using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Lean.Gui.LeanWindow))]
public abstract class Window : MonoBehaviour
{
    public float timerAnimation = 0f;

    protected bool finishedAnimation = true;

    protected Lean.Gui.LeanWindow window = null;

    void Awake()
    {
        window = GetComponent<Lean.Gui.LeanWindow>();

        InitializeOnAwake();
    }

    protected virtual void InitializeOnAwake() { }

    public virtual void TurnOn()
    {
        window.TurnOn();
    }

    public virtual void TurnOff()
    {
        window.TurnOff();
    }

    protected IEnumerator AnimateWindowRoutine(float timer)
    {
        finishedAnimation = false;

        yield return new WaitForSeconds(timer);

        AnimateWindowFinish();
    }

    protected virtual void AnimateWindowFinish()
    {
        finishedAnimation = true;
    }
}
