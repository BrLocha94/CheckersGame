using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualController : MonoSingleton<VisualController>
{
    [Header("General Buttons references")]
    [SerializeField]
    private Button buttonPauseOver;

    [Header("General Ui texts")]
    [SerializeField]
    private Text textTime;
    [SerializeField]
    private Text textCurrentPlayer;

    [Space]

    [Header("General windows used on this script")]
    [SerializeField]
    private Window windowFume;
    [SerializeField]
    private Window windowIntro;
    [SerializeField]
    private Window windowTutorial;
    [SerializeField]
    private Window windowConfirm;
    [SerializeField]
    private Window windowGameOver;

    public GameStates currentGameState { get; private set; }

    void OnGameStateChange(GameStates newGameState)
    {
        currentGameState = newGameState;

        if (currentGameState == GameStates.Intro)
            TurnIntroOn();
    }

    private void OnEnable()
    {
        GameController.onGameStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameController.onGameStateChange -= OnGameStateChange;
    }

    public void TurnPauseOn()
    {
        if (currentGameState == GameStates.Running)
        {
            GameController.instance.ChangeGameState(GameStates.Paused);

            buttonPauseOver.transform.localScale = Vector3.one;

            windowFume.TurnOn();
        }
    }

    public void TurnPauseOff()
    {
        if (currentGameState == GameStates.Paused)
        {
            buttonPauseOver.transform.localScale = Vector3.zero;

            ResumeGame();
        }
    }

    public void TurnIntroOn()
    {
        if (currentGameState == GameStates.Intro)
        {
            windowFume.TurnOn();
            windowIntro.TurnOn();
        }
    }

    public void TurnIntroOff()
    {
        if (currentGameState == GameStates.Intro)
        {
            windowFume.TurnOff();
            GameController.instance.ChangeGameState(GameStates.SpawningPieces);
        }
    }

    public void TurnConfirmOn()
    {
        if (currentGameState == GameStates.Running)
        {
            GameController.instance.ChangeGameState(GameStates.Confirm);

            windowFume.TurnOn();
            windowConfirm.TurnOn();
        }
    }

    public void TurnTutorialOn()
    {
        if (currentGameState == GameStates.Running)
        {
            GameController.instance.ChangeGameState(GameStates.Tutorial);

            windowFume.TurnOn();
            windowTutorial.TurnOn();
        }
    }

    public void TurnGameOverOn()
    {
        if (currentGameState == GameStates.GameClear ||
            currentGameState == GameStates.GameOver ||
            currentGameState == GameStates.Draw)
        {
            windowFume.TurnOn();
            windowGameOver.TurnOn();
        }
    }

    public void ResumeGame()
    {
        GameController.instance.ChangeGameState(GameStates.Running);

        windowFume.TurnOff();
    }
}
