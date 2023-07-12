using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Constants;
using Unity.Netcode;
using Unity.Collections;

public class PlayerScript : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    [field: SerializeField] public PlayerLabels.LabelEnum mylabel { get; private set; }
    [SerializeField] public StatsScriptableObject Score;
    [field: SerializeField] public FixedString128Bytes PlayerName { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameLogicScript.OnStateChange += PlayerOnStateChange;
        Score = StatsScriptableObject.CreateInstance<StatsScriptableObject>();
        this.Score.PersonalPoints = 0;
        this.Score.TeamPoints = 0;
        labelText = GameObject.Find("PlayerLabel_UI").GetComponent<TextMeshProUGUI>();
        PlayerName = "UNNAMED";
    }

    private void PlayerOnStateChange(GameState state)   
    {
        if(!IsOwner) return;
        if(state == GameState.RoundStart)
        {
            //was getAndSetRandomLabel before, now get setted by gameLogicScripts
        }
        if(state == GameState.RoundEnd)
        {

        }
    }

    public void getAndSetRandomLabel()
    {
        mylabel = PlayerLabels.GetRandomLabelEnum();

        labelText.text = $"#{PlayerLabels.EnumToString(mylabel)}";
    }

    //private void OnDestroy()
    //{
    //    GameLogicScript.OnStateChange -= PlayerOnStateChange;
    //}
}
