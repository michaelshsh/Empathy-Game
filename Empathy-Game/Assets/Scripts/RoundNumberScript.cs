using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class RoundNumberScript : NetworkBehaviour
{
    public static RoundNumberScript Instance;
    public NetworkVariable<int> roundNumber = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> MaximumRounds = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private TextMeshProUGUI roundText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroyed RoundNumberScript");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public override void OnNetworkSpawn() //void Start()
    {
        Instance = this;
        GameLogicScript.Instance.CurrentGameState.OnValueChanged += RoundNumberOnStateChange;
        RoundNumberScript.Instance.roundNumber.OnValueChanged += updateText;
        updateText(0,roundNumber.Value);
    }

    private void RoundNumberOnStateChange(GameState oldState, GameState newValue)
    {
        if (IsHost)
        {
            if (newValue == GameState.GameStart)
            {
                roundNumber.Value = 0; //round start will call ++
            }
            else if (newValue == GameState.RoundStart)
            {
                ++roundNumber.Value;
            }

            if (roundNumber.Value > MaximumRounds.Value && newValue != GameState.GameEnd)
            {
                StartCoroutine(GameLogicScript.Instance.UpdateGameByState(GameState.GameEnd));
            }
        }

        updateText(0, roundNumber.Value); //update text, without the value changing
    }

    private void updateText(int previousValue, int newValue)
    {
        var newState = GameLogicScript.Instance.CurrentGameState.Value;
        if (newState == GameState.RoundStart)
        {
            roundText.text = $"Round {roundNumber.Value} out of {MaximumRounds.Value}";
        }
        else if (newState == GameState.RoundEnd)
        {
            roundText.text = $"End of round {roundNumber.Value}";
        }
    }

    public void SetUpMaxRounds(int maxRounds)
    {
        if(!IsServer) { return; }
        MaximumRounds.Value = maxRounds;
    }
}
