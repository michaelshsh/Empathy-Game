using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static TimerScript Instance;
    [SerializeField] public float timeRemaining { get; private set; }
    [SerializeField] public bool timerIsRunning { get; private set; }
    [SerializeField] private TextMeshProUGUI timeText;
    void Start()
    {
        Instance = this;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                //change the round to OVER!
                Debug.Log("TimerScript Calling GameState.RoundEnd");
                GameLogicScript.Instance.UpdateGameByState(GameState.RoundEnd);
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

    public void SetRoundTime(int time)
    {
        timeRemaining = time;
    }

    public void StartTimer()
    { timerIsRunning = true; }
}