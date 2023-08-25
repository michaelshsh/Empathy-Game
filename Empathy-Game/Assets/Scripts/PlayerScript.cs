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

public struct PlayerScore : INetworkSerializable
{
    public int PersonalPoints;
    public int TeamPoints;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PersonalPoints);
        serializer.SerializeValue(ref TeamPoints);
    }
}

public class PlayerScript : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    public NetworkVariable<LabelEnum> mylabel = new(LabelEnum.QA,
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    public NetworkVariable<PlayerScore> Score = new (new PlayerScore { PersonalPoints = 0, TeamPoints=0 }, 
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> SyncedToRound = new (0,
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);
    [field: SerializeField] public FixedString128Bytes PlayerName { get; private set; }

    public override void OnNetworkSpawn()
    {
        GameLogicScript.Instance.CurrentGameState.OnValueChanged += PlayerOnStateChange;
        labelText = GameObject.Find("PlayerLabel_UI").GetComponent<TextMeshProUGUI>();
        PlayerName = $"UnNamed-{OwnerClientId}";

        if(GameLogicScript.Instance.CurrentGameState.Value==GameState.GameStart)
        {
            PlayerOnStateChange(GameState.MainMenu, GameState.SetupPhase); //setup if loaded mid game
        }
        PlayerOnStateChange(GameState.MainMenu, GameLogicScript.Instance.CurrentGameState.Value);

    }

    private void PlayerOnStateChange(GameState previousValue, GameState newValue)
    {
        Debug.LogWarning($"entering playerstatecahnge with {newValue}");
        if (!IsOwner) return;
        if(newValue == GameState.SetupPhase)
        {
            GetAndSetRandomLabel();
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
            KillCards();
            ResetSlotsCards();
        }

        SyncedToRound.Value = RoundNumberScript.Instance.roundNumber.Value; //let server know we are synced
    }

    private void KillCards()
    {
        var AllCards = FindObjectsOfType<CardScript>();
        foreach (var card in AllCards)
        {
            CardSlotsManager.InstanceSlotManager.availableSlot[card.SlotIndex] = true;
            Destroy(card.gameObject);
        }
    }

    private void ResetSlotsCards()
    {
        var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
        foreach (var slot in AllSlots)
        {
            slot.UIText.text = "";
            slot.card = null;
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
        int Ppoints = 0, Tpoints = 0;
        foreach (var slot in AllSlots)
        {
            if (slot.card != null)
            {
                Ppoints += slot.card.PersonalPoints;
                Tpoints += slot.card.TeamPoints;
            }
            else
            {
                //Can insert penalty here for unused cards
            }
        }
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
