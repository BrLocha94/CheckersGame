using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGameStateChange(GameStates newState);

public class GameController : MonoSingleton<GameController>
{
    public static event OnGameStateChange onGameStateChange;

    [Header("Time variables to initialize routine")]
    [SerializeField]
    [Range(0.1f, 2f)]
    private float timeToSpawPieces = 0.1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    private float timeToIntro = 0.1f;
    
    public bool againstAI { get; set; }

    GameStates currentGameState = GameStates.Null;

    void Start()
    {
        againstAI = false;

        ChangeGameState(GameStates.Initializing);

        StartCoroutine(ChangeStateRoutine(timeToIntro, GameStates.Intro));
    }

    IEnumerator ChangeStateRoutine(float time, GameStates nextState)
    {
        yield return new WaitForSeconds(time);

        ChangeGameState(nextState);
    }

    public GameStates GetGameState()
    {
        return currentGameState;
    }

    public void ChangeGameState(GameStates newState)
    {
        currentGameState = newState;

        if (currentGameState == GameStates.SpawningPieces)
            StartCoroutine(ChangeStateRoutine(timeToSpawPieces, GameStates.Running));

        onGameStateChange?.Invoke(newState);
    }
}

public enum GameStates
{
    Null,
    Initializing,
    Intro,
    SpawningPieces,
    Running,
    AIMoviment,
    Paused,
    Tutorial,
    Confirm,
    GameClear,
    GameOver,
    Draw
}
