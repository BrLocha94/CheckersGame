using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameStateText : MonoBehaviour
{
    Text textUi;

    void Awake()
    {
        textUi = GetComponent<Text>();    
    }

    void OnGameStateChange(GameStates newGameState)
    {
        #if UNITY_EDITOR

        textUi.text = newGameState.ToString();
        
        #endif
    }

    private void OnEnable()
    {
        GameController.onGameStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameController.onGameStateChange -= OnGameStateChange;
    }
}
