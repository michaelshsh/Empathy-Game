using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameLogicScript : MonoBehaviour
{
    public static GameLogicScript Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    [SerializeField] public GameState gameState { get; private set; }
    public static event Action<GameState> OnStateChange; //can subscribe to it, to get notified when a state is changed

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        UpdateGameByState(GameState.GameStart);
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
                RoundStartHandler();
                break;
            case GameState.RoundEnd:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            case GameState.GameStart:
                GameStartHandler();
                break;
        }

        OnStateChange?.Invoke(newState);
    }

    private void RoundStartHandler()
    {
        TimerScript.Instance.StartTimer();
    }

    private void GameStartHandler()
    {
        TimerScript.Instance.SetRoundTime(5);
        RoundNumberScript.Instance.SetUpMaxRounds(6);
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
    GameStart,
    RoundEnd,
    RoundStart,
    Victory,
    Lose
}