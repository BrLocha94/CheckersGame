using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGameStateChange(GameStates newState);

public class GameController : MonoSingleton<GameController>
{
    public static event OnGameStateChange onGameStateChange;

    bool againstAI = false;

    GameStates currentGameState = GameStates.Null;

    void Start()
    {
        ChangeGameState(GameStates.Initializing);
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
    Running,
    Paused,
    Finishing
}
