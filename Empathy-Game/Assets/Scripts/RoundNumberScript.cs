using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundNumberScript : MonoBehaviour
{
    public static RoundNumberScript Instance;
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
            roundNumber = 1; //setting to 0 to start at 1 when invoked with RoundStart
        }
        else if (state == GameState.RoundStart)
        {
            //roundText.enabled = true;
            roundText.text = $"Round {roundNumber} out of {maximumRounds}";
        }
        else if (state == GameState.RoundEnd)
        {
            //roundText.enabled = false;
            roundText.text = $"End of round {roundNumber}";
            roundNumber++;
        }

        if(roundNumber > maximumRounds)
        {
            GameLogicScript.Instance.UpdateGameByState(GameState.GameEnd);
        }
    }

    public void SetUpMaxRounds(int maxRounds)
    {
        maximumRounds = maxRounds;
    }
}
