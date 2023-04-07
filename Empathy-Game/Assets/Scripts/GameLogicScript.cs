using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    public static GameLogicScript Instance;
    public TimerScript roundTimer;
    public GameState gameState;

    //can subscribe to it, to get notified when a state is changed
    public static event Action<GameState> OnStateChange;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        UpdateGameByState(GameState.RoundStart);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameByState(GameState newState)
    {
        gameState = newState;
        Debug.Log($"Changing state to: {newState}");

        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Lobby:
                break;
            case GameState.RoundStart:
                StartTimer(5);
                break;
            case GameState.RoundEnd:
                break;
            case GameState.Victory: 
                break;
            case GameState.Lose:
                break;
        }

        OnStateChange?.Invoke(newState);
    }

    private void StartTimer(float roundTime)
    {
        roundTimer.timeRemaining = roundTime;
        roundTimer.timerIsRunning = true;
    }
}

public class Constants
{
    public class Labels
    {
        public const string Dev = "Developer";
        public const string IT = "Tech Support";    
        public const string PM = "Product Manager";
        public const string HR = "Human Resorces";
    }
}

public enum GameState
{
    MainMenu,
    Lobby,
    RoundEnd,
    RoundStart,
    Victory,
    Lose
}