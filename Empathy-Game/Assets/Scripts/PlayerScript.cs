using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Constants;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    private PlayerLabels.LabelEnum mylabel;
    [field: SerializeField]
    public StatsScriptableObject Score;

    // Start is called before the first frame update
    void Start()
    {
        GameLogicScript.OnStateChange += PlayerOnStateChange;
        Score = StatsScriptableObject.CreateInstance<StatsScriptableObject>();
        this.Score.PersonalPoints = 0;
        this.Score.TeamPoints = 0;
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
