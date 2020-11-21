using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTutorial : Window
{
    public override void TurnOff()
    {
        base.TurnOff();

        StartCoroutine(AnimateWindowRoutine(timerAnimation));
    }

    protected override void AnimateWindowFinish()
    {
        VisualController.instance.ResumeGame();

        base.AnimateWindowFinish();
    }
}
