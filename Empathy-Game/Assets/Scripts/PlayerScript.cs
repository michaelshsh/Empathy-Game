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
    [field: SerializeField]
    private StatsScriptableObject Stats;

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
        if(state == GameState.RoundEnd)
        {
            
            //update points
            var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
            Debug.Log($"counting points for player {this.gameObject.name}");
            foreach (var slot in AllSlots)
            {
                if (slot.card != null)
                { 
                    //please Maya tell me which to use or are we using both?
                    //local
                    PlayerPersonalPoints += slot.card.PersonalPoints;
                    PlayerTeamPoints += slot.card.TeamPoints;
                    //stats "scriptable object"
                    Stats.PersonalPoints += slot.card.PersonalPoints;
                    Stats.GroupPoints += slot.card.TeamPoints;

                    //maybe add invocation of event here to let UI know to update
                }
                else
                {
                    //Can insert penalty here for unused cards
                }
            }
            Debug.Log($"new vals are {PlayerPersonalPoints}P {PlayerTeamPoints}T points for player {this.gameObject.name}");
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
