using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI label;

    // Start is called before the first frame update
    void Start()
    {
        GameLogicScript.OnStateChange += PlayerOnStateChange;
    }

    private void PlayerOnStateChange(GameState state)   
    {
        if(state == GameState.RoundStart)
        {
            label.text = $"#{getRandomLabel()}";
        }
    }

    private string getRandomLabel()
    {
        return Constants.Labels.HR;
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
