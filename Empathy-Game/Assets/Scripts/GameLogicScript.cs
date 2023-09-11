using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public async void UpdateGameByState(GameState newState)
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
                RoundStartHandlerBeforeInvoke();
                break;
            case GameState.SetupPhase:
                break;
            case GameState.RoundEnd:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            case GameState.GameStart:
                GameStartHandlerBeforeInvoke();
                break;
            case GameState.GameEnd:
                break;
            case GameState.ShowSummery:
                break;
        }

        Debug.Log($"Invoking state {newState} to all");
        CurrentGameState.Value = newState; //this is the invoke! 
        await AwaitPlayerSync();

        switch (newState) //after invoking all
        {
            case GameState.MainMenu:
                break;
            case GameState.Lobby:
                break;
            case GameState.RoundStart:
                break;
            case GameState.SetupPhase:
                UpdateGameByState(GameState.RoundStart);
                break;
            case GameState.RoundEnd:
                RoundEndAfterInvoked();
                UpdateGameByState(GameState.ShowSummery);
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            case GameState.GameStart:
                UpdateGameByState(GameState.SetupPhase);
                break;
            case GameState.GameEnd:
                FinishGameAfterInvoke();
                break;
            case GameState.ShowSummery:
                break;
        }
    }

    private void FinishGameAfterInvoke()
    {
        LobbyManager.Instance.LeaveLobby();
        // to finish the game through a button from the roundEnd window?
        // SceneManager.LoadScene("MainMenu");
    }

    private void RoundEndAfterInvoked()
    {
        if (!IsServer) return;
        var players = FindObjectsOfType<PlayerScript>();
        foreach (var player in players)
        {
            Debug.Log($"player {player.PlayerName.Value} synced. score of: {player.Score.Value.PersonalPoints}P {player.Score.Value.TeamPoints}T");
        }
        //StatisticsScript.Instance.UpdateAllPlayersStatistics();
        //foreach (var player in players)
        //{
        //    var stats = StatisticsScript.Instance.GetPlayerScore(player)[RoundNumberScript.Instance.roundNumber.Value-1];

        //    Debug.Log($"{player.PlayerName}: Stats:" +
        //        $"\n{stats.PersonalPoints}: Personal" +
        //        $"\n{stats.TeamPoints}: Team" +
        //        $"\n{stats.UnPlayedCardsCount}: UnPlayedCardsCount" +
        //        $"\n{stats.unusedSlots}: unusedSlots");
        //}
        //go to post round screen? display it directly? // will pop from its own code?
        //round number will incrimante alone through its code at round start!
        //timer will count end of round time
    }

    private async Task AwaitPlayerSync()
    {
        var players = FindObjectsOfType<PlayerScript>();
        foreach (var player in players)
        {
            while (player.SyncedToState.Value != CurrentGameState.Value)
            {
                //Debug.Log($"wating 50 for player {player.PlayerName}");
                await Task.Delay(50); //(player.SyncedToState.Value == CurrentGameState.Value);
            }

            Debug.Log($"player {player.PlayerName.Value} synced.");
        }

        Debug.Log($"All players are synced");
    }

    private void RoundStartHandlerBeforeInvoke()
    {
        //start timer
        TimerScript.Instance.StartTimer();
    }

    private void GameStartHandlerBeforeInvoke()
    {
        TimerScript.Instance.SetRoundTime(Constants.GameSettings.RoundTime);
        RoundNumberScript.Instance.SetUpMaxRounds(Constants.GameSettings.RoundsCount);
    }
}

public enum GameState 
{
    MainMenu,
    Lobby,
    GameStart,
    SetupPhase,
    RoundEnd,
    RoundStart,
    Victory,
    Lose,
    GameEnd,
    ShowSummery
}
