using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameLogicScript : NetworkBehaviour
{
    public static GameLogicScript Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("found duplicate of gamelogicscript");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //public GameState CurrentGameState { get; private set; } = GameState.GameStart;
    public NetworkVariable<GameState> CurrentGameState = new(GameState.Lobby, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    //public static event Action<GameState> OnStateChange; //can subscribe to it, to get notified when a state is changed

    public override void OnNetworkSpawn() //void Start()
    {
        Instance = this;

        if(IsServer)
        {
            CurrentGameState.Value = GameState.GameStart;
        }
        UpdateGameByState(CurrentGameState.Value);
    }

    public void UpdateGameByState(GameState newState)
    {
        if (!IsServer) return;
        Debug.Log($"Changing state to: {newState}");

        switch (newState) //before invoking all
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
            case GameState.GameEnd:
                break;
        }

        Debug.Log($"Invoking state {newState} to all");
        CurrentGameState.Value = newState; //this is the invoke! 

        switch (newState) //after invoking all
        {
            case GameState.MainMenu:
                break;
            case GameState.Lobby:
                break;
            case GameState.RoundStart:

                break;
            case GameState.RoundEnd:
                RoundEndAfterInvoked();
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            case GameState.GameStart:
                UpdateGameByState(GameState.RoundStart);
                break;
            case GameState.GameEnd:
                break;
        }
    }

    private void RoundEndAfterInvoked()
    {
        if (!IsServer) return;

        //wait for players to count points and kill cards //BUG: players update thier score after! why??
        var players = FindObjectsOfType<PlayerScript>();
        foreach (var player in players)
        {
            if (player.SyncedToRound.Value != RoundNumberScript.Instance.roundNumber.Value)
            {
                Debug.LogError($"Player {player.PlayerName} was not synced! round {player.SyncedToRound.Value} with score of: {player.Score.Value.PersonalPoints}P {player.Score.Value.TeamPoints}T");
            }
            else
                Debug.Log($"player {player.PlayerName} synced to round {player.SyncedToRound.Value} with score of: {player.Score.Value.PersonalPoints}P {player.Score.Value.TeamPoints}T");
        }

        //go to post round screen? display it directly? // will pop from its own code?
        //round number will incrimante alone through its code at round start!
        //timer will count end of round time
    }

    private void RoundStartHandler()
    {
        //start timer
        TimerScript.Instance.StartTimer();
    }

    private void GameStartHandler()
    {
        TimerScript.Instance.SetRoundTime(15);
        RoundNumberScript.Instance.SetUpMaxRounds(6);
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
    Lose,
    GameEnd
}
