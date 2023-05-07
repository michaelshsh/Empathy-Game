using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundNumberScript : MonoBehaviour
{
    public static RoundNumberScript Instance; // pls take a look at this...
    [field: SerializeField]
    public int roundNumber { get; private set; }
    [field: SerializeField]
    public int maximumRounds { get; private set; }
    [SerializeField] private TextMeshProUGUI roundText;
    // Start is called before the first frame update
    void Start()
    {
        roundNumber = 1;
        Instance = this;
        GameLogicScript.OnStateChange += RoundNumberOnStateChange;
    }

    private void RoundNumberOnStateChange(GameState state)
    {
        if (state == GameState.GameStart)
        {
            roundNumber = 0; //setting to 0 to start at 1 when invoked with RoundStart
        }
        else if (state == GameState.RoundStart)
        {
            roundNumber++;
        }
        else
        {
            roundText.enabled = false;
        }

        roundText.text = $"Round {roundNumber} out of {maximumRounds}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpMaxRounds(int maxRounds)
    {
        maximumRounds = maxRounds;
    }
}
