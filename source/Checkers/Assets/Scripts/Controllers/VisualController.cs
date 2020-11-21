using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualController : MonoSingleton<VisualController>
{
    [Header("General Buttons references")]
    [SerializeField]
    private Button buttonPauseOver = null;

    [Space]

    [Header("General Visual references")]
    [SerializeField]
    private Text textTime = null;
    [SerializeField]
    private Image imageCurrentPiece = null;
    [SerializeField]
    private Color colorPieceWhite = Color.white;
    [SerializeField]
    private Color colorPieceBlack = Color.black;

    [Space]

    [Header("General windows used on this script")]
    [SerializeField]
    private Window windowFume = null;
    [SerializeField]
    private Window windowIntro = null;
    [SerializeField]
    private Window windowTutorial = null;
    [SerializeField]
    private Window windowConfirm = null;
    [SerializeField]
    private Window windowGameOver= null;

    float time = 0f;

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

    void Update()
    {
        if(currentGameState == GameStates.Running)
        {
            time += Time.deltaTime;

            textTime.text = UpdateRaceTime(time);
        }
    }

    public string UpdateRaceTime(float currentTime)
    {
        int curMin = Mathf.FloorToInt(currentTime / 60f);
        int curSec = Mathf.FloorToInt(currentTime - curMin * 60);
        int curCent = Mathf.FloorToInt((currentTime * 100) % 100);

        string curTime = string.Format("{0:00}:{1:00}:{2:00}", curMin, curSec, curCent);

        return curTime;
    }

    public void UpdateCurrentPlayer(PieceTypes pieceType)
    {
        if (pieceType == PieceTypes.White)
            imageCurrentPiece.color = colorPieceWhite;
        else
            imageCurrentPiece.color = colorPieceBlack;
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
