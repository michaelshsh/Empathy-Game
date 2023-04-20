using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Constants;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI labelText;
    private Labels.LabelEnum mylabel;

    // Start is called before the first frame update
    void Start()
    {
        GameLogicScript.OnStateChange += PlayerOnStateChange;
    }

    private void PlayerOnStateChange(GameState state)   
    {
        if(state == GameState.RoundStart)
        {
            //was getAndSetRandomLabel before, now get setted by gameLogicScripts
        }
    }

    public void getAndSetRandomLabel()
    {
        mylabel = Labels.GetRandomLabelEnum();

        labelText.text = $"#{Labels.EnumToString(mylabel)}";
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
