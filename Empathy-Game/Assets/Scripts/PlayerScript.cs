using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Constants;
using Unity.Netcode;
using Unity.Collections;
using static Constants.PlayerLabels;

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
        PlayerOnStateChange(GameState.MainMenu, GameLogicScript.Instance.CurrentGameState.Value); //act now
    }

    private void PlayerOnStateChange(GameState previousValue, GameState newValue)
    {
        if (!IsOwner) return;
        if (newValue == GameState.RoundStart)
        {
            GetAndSetRandomLabel();

            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();
            CardSlotsManager.InstanceSlotManager.DrawCard();          
        }
        if (newValue == GameState.RoundEnd)
        {
            CountMyPoints();
            //kill cards
        }

        SyncedToRound.Value = RoundNumberScript.Instance.roundNumber.Value; //let server know we are synced
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
