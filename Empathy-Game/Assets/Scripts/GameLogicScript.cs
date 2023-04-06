using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    public TimerScript roundTimer;
    // Start is called before the first frame update
    void Start()
    {
        roundTimer.timeRemaining = 5;
        roundTimer.timerIsRunning = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}