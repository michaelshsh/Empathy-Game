using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimerScript : NetworkBehaviour
{
    public static TimerScript Instance;
    public float RoundTime { get; private set; }
    public NetworkVariable<float> TimeRemaining = new(99, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> TimerIsRunning = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroyed TimerScript");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public override void OnNetworkSpawn()
    {
        Instance = this;    
        timeText = GameObject.Find("Timer_var_text").GetComponent<TextMeshProUGUI>();
        GameLogicScript.Instance.CurrentGameState.OnValueChanged += TimerOnStateChange;
    }

    private void TimerOnStateChange(GameState previousValue, GameState newValue)
    {
        if (newValue == GameState.RoundEnd)
        {
            //timer for "end of round"
            var roundTime = RoundTime;
            SetRoundTime(10); // 10 sec to see post game
            StartTimer();
            SetRoundTime(roundTime); //set round time back for the next round
        }
    }

    //public void Start()
    //{
    //    Instance = this;
    //}
    void Update()
    {
        if (TimerIsRunning.Value)
        {
            if (TimeRemaining.Value > 0)
            {
                if (IsHost)
                {
                    TimeRemaining.Value -= Time.deltaTime;
                }
                DisplayTime(TimeRemaining.Value);
            }
            else
            {
                if (IsHost)
                {
                    TimeRemaining.Value = 0;
                    TimerIsRunning.Value = false;
                }

                //change the round to OVER!
                if (GameLogicScript.Instance.CurrentGameState.Value == GameState.RoundStart)
                {
                    Debug.Log("TimerScript Calling GameState.RoundEnd");
                    GameLogicScript.Instance.UpdateGameByState(GameState.RoundEnd);
                }
                else if (GameLogicScript.Instance.CurrentGameState.Value == GameState.RoundEnd)
                {
                    Debug.Log("TimerScript Calling GameState.SetupPhase");
                    GameLogicScript.Instance.UpdateGameByState(GameState.SetupPhase);
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetRoundTime(float time)
    {
        RoundTime = time;
    }

    public void StartTimer()
    {
        if (!IsHost) return;
        TimeRemaining.Value = RoundTime;
        TimerIsRunning.Value = true;
    }
}
