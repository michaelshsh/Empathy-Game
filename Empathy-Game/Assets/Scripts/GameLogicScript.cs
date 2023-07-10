using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
                RoundEndHandler();
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

        OnStateChange?.Invoke(newState);
    }

    private void RoundEndHandler()
    {
        //count points
        CountPointsForPlayers();

        // kill cards? or not now?

        //go to post round screen? display it directly? // will pop from its own code

        //round number will incrimante alone through its code

        //timer before next round starts
        var roundTime = TimerScript.Instance.RoundTime;
        TimerScript.Instance.SetRoundTime(10); // 10 sec to see post game
        TimerScript.Instance.StartTimer();
        TimerScript.Instance.SetRoundTime(roundTime); //set round time back for the next round
    }

    private static void CountPointsForPlayers()
    {
        var AllPlayers = FindObjectsOfType<PlayerScript>();
        foreach (var player in AllPlayers)
        {
            var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
            Debug.Log($"counting points for player named: {player.PlayerName}");
            int Ppoints = 0, Tpoints = 0;
            foreach (var slot in AllSlots)
            {
                if (slot.card != null)
                {
                    Ppoints += slot.card.PersonalPoints;
                    Tpoints += slot.card.TeamPoints;
                }
                else
                {
                    //Can insert penalty here for unused cards
                }
            }
            //stats "scriptable object"
            player.Score.PersonalPoints += Ppoints;
            player.Score.TeamPoints += Tpoints;

            //maybe add invocation of event here to let UI know to update instaed of method?
            UiController.Instance.updateScore(player.Score);
            Debug.Log($"adding {Ppoints}P {Tpoints}T points for player named:{player.PlayerName}");
        }
    }

    private void RoundStartHandler()
    {
        //empty schedule
        // schedule not implimenteed yet

        //draw 4 cards
        CardSlotsManager.InstanceSlotManager.DrawCard();
        CardSlotsManager.InstanceSlotManager.DrawCard();
        CardSlotsManager.InstanceSlotManager.DrawCard();
        CardSlotsManager.InstanceSlotManager.DrawCard();

        //random player label
        var players = FindObjectsOfType<PlayerScript>();
        Debug.Log($"found {players.Length} players to give random labels to");
        foreach (var player in players)
        {
            player.getAndSetRandomLabel();
        }

        //start timer
        TimerScript.Instance.StartTimer();
    }

    private void GameStartHandler()
    {
        TimerScript.Instance.SetRoundTime(15);
        RoundNumberScript.Instance.SetUpMaxRounds(6);
        UpdateGameByState(GameState.RoundStart);
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
