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
    [SerializeField] public PlayerLabels.LabelEnum mylabel;
    [field: SerializeField] public StatsScriptableObject Score;
    [SerializeField] public FixedString128Bytes PlayerName { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameLogicScript.OnStateChange += PlayerOnStateChange;
        Score = StatsScriptableObject.CreateInstance<StatsScriptableObject>();
        this.Score.PersonalPoints = 0;
        this.Score.TeamPoints = 0;
        labelText = GameObject.Find("PlayerLabel_UI").GetComponent<TextMeshProUGUI>();
        labelText.text = "Was able to take over!!";
    }

    private void PlayerOnStateChange(GameState state)   
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameLogicScript.OnStateChange -= PlayerOnStateChange;
    }
}
