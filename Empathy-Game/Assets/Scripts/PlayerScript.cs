using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Constants;
using Unity.Netcode;
using Unity.Collections;
using static Constants.PlayerLabels;
using Unity.VisualScripting;

public class PlayerScript : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    public NetworkVariable<LabelEnum> mylabel = new(LabelEnum.QA,
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    public NetworkVariable<PlayerScore> Score = new (new PlayerScore { PersonalPoints = 0, TeamPoints=0 }, 
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    public NetworkVariable<GameState> SyncedToState = new (GameState.MainMenu,
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    public NetworkVariable<PlayerRoundStatistics> RoundStatistics = new(new PlayerRoundStatistics(),
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);

    [field: SerializeField] public FixedString128Bytes PlayerName { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        GameLogicScript.Instance.CurrentGameState.OnValueChanged += PlayerOnStateChange;
        labelText = GameObject.Find("PlayerLabel_UI").GetComponent<TextMeshProUGUI>();
        PlayerName = $"UnNamed-{OwnerClientId}";

        if(GameLogicScript.Instance.CurrentGameState.Value==GameState.GameStart)
        {
            PlayerOnStateChange(SyncedToState.Value, GameState.SetupPhase); //setup if loaded mid game
        }
        PlayerOnStateChange(SyncedToState.Value, GameLogicScript.Instance.CurrentGameState.Value);

    }

    private void PlayerOnStateChange(GameState previousValue, GameState newValue)
    {
        Debug.LogWarning($"entering playerstatecahnge with {newValue}");
        if (!IsOwner) return;
        if(newValue == GameState.SetupPhase)
        {
            GetAndSetRandomLabel();
            RoundStatistics.Value = new PlayerRoundStatistics();
        }
        else if (newValue == GameState.RoundStart)
        {
            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();          
        }
        if (newValue == GameState.RoundEnd)
        {
            CountMyPoints();
            KillPlayedCards();
            KillUnplayedCards();
        }

        SyncedToState.Value = newValue; //let server know we are synced
    }

    private void KillUnplayedCards()
    {
        var AllCards = FindObjectsOfType<CardScript>();
        foreach (var card in AllCards)
        {
            CardSlotsManager.InstanceSlotManager.availableSlot[card.SlotIndex] = true;
            Destroy(card.gameObject);
            RoundStatistics.Value.UnPlayedCardsCount++;
        }
    }

    private void KillPlayedCards()
    {
        var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
        foreach (var slot in AllSlots)
        {
            slot.UIText.text = "";
            if(slot.TaskCard != null)
            {
                CardSlotsManager.InstanceSlotManager.availableSlot[slot.TaskCard.SlotIndex] = true;
                Destroy(slot.TaskCard.gameObject);
                slot.TaskCard = null;
            }
        }
    }

    public void GetAndSetRandomLabel()
    {
        mylabel.Value = PlayerLabels.GetRandomLabelEnum();

        labelText.text = $"#{PlayerLabels.EnumToString(mylabel.Value)}";
    }

    private void CountMyPoints()
    {
        var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
        int Ppoints = 0, Tpoints = 0, unusedSlots = 0;
        foreach (var slot in AllSlots)
        {
            if (slot.TaskCard != null)
            {
                Ppoints += slot.TaskCard.PersonalPoints;
                Tpoints += slot.TaskCard.TeamPoints;
            }
            else
            {
                unusedSlots++;
            }
        }

        RoundStatistics.Value.TeamPoints = Tpoints;
        RoundStatistics.Value.PersonalPoints = Ppoints;
        RoundStatistics.Value.unusedSlots = unusedSlots;

        //stats "scriptable object"
        var temp = new PlayerScore()
        {
            TeamPoints = Score.Value.TeamPoints + Tpoints,
            PersonalPoints = Score.Value.PersonalPoints + Ppoints,
        };
        Score.Value = temp;

        
        Debug.Log($"adding {Ppoints}P {Tpoints}T points for player named:{PlayerName}");
    }

    //private void OnDestroy()
    //{
    //    GameLogicScript.OnStateChange -= PlayerOnStateChange;
    //}
}
