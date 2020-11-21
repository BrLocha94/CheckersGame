using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowIntro : Window
{
    public void ChooseGameMode(bool againstIA)
    {
        GameController.instance.againstAI = againstIA;

        TurnOff();
    }

    public override void TurnOff()
    {
        base.TurnOff();

        StartCoroutine(AnimateWindowRoutine(timerAnimation));
    }

    protected override void AnimateWindowFinish()
    {
        VisualController.instance.TurnIntroOff();

        base.AnimateWindowFinish();
    }
}
