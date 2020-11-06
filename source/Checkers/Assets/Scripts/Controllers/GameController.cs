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
    private float timeToInitialize = 0.1f;
    
    public bool againstAI { get; private set; }

    GameStates currentGameState = GameStates.Null;

    void Start()
    {
        againstAI = false;

        ChangeGameState(GameStates.Initializing);

        StartCoroutine(InitializeGameRoutine());
    }

    IEnumerator InitializeGameRoutine()
    {
        yield return new WaitForSeconds(timeToInitialize);

        ChangeGameState(GameStates.SpawningPieces);

        yield return new WaitForSeconds(timeToSpawPieces);

        ChangeGameState(GameStates.Running);
    }

    public GameStates GetGameState()
    {
        return currentGameState;
    }

    public void ChangeGameState(GameStates newState)
    {
        currentGameState = newState;

        onGameStateChange?.Invoke(newState);
    }
}

public enum GameStates
{
    Null,
    Initializing,
    SpawningPieces,
    Running,
    Paused,
    GameClear,
    GameOver
}
