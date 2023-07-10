using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static TimerScript Instance;
    [SerializeField] public float RoundTime { get; private set; }
    [SerializeField] public float TimeRemaining { get; private set; }
    [SerializeField] public bool TimerIsRunning { get; private set; }
    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {
        Instance = this;
    }
    void Update()
    {
        if (TimerIsRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
                DisplayTime(TimeRemaining);
            }
            else
            {
                TimeRemaining = 0;
                TimerIsRunning = false;

                //change the round to OVER!
                if(GameLogicScript.Instance.gameState == GameState.RoundStart) 
                {
                    Debug.Log("TimerScript Calling GameState.RoundEnd");
                    GameLogicScript.Instance.UpdateGameByState(GameState.RoundEnd);
                }
                else if(GameLogicScript.Instance.gameState == GameState.RoundEnd)
                {
                    Debug.Log("TimerScript Calling GameState.RoundStart");
                    GameLogicScript.Instance.UpdateGameByState(GameState.RoundStart);
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
        TimeRemaining = RoundTime;
        TimerIsRunning = true;
    }
}