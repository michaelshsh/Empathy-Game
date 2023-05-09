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
    public int PlayerPersonalPoints { get; private set; }
    [field: SerializeField]
    public int PlayerTeamPoints { get; private set; }

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
