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
    public NetworkVariable<RoundStatistics> RoundStatistics = new(new RoundStatistics(),
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);

    public NetworkVariable<FixedString128Bytes> PlayerName = new(new FixedString128Bytes(),
                                                    NetworkVariableReadPermission.Everyone,
                                                    NetworkVariableWritePermission.Owner);

    public List<RoundStatistics> Stats = new List<RoundStatistics>();
    [field: SerializeField] public FixedString128Bytes PlayerName { get; private set; }


    private RoundStatistics localStats;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        GameLogicScript.Instance.CurrentGameState.OnValueChanged += PlayerOnStateChange;
        PlayerName.Value = $"UnNamed-{OwnerClientId}";
        labelText = GameObject.Find("PlayerLabel_UI").GetComponent<TextMeshProUGUI>();

        if (GameLogicScript.Instance.CurrentGameState.Value==GameState.GameStart || GameLogicScript.Instance.CurrentGameState.Value == GameState.RoundStart)
        {
            PlayerOnStateChange(SyncedToState.Value, GameState.SetupPhase); //setup if loaded mid game
        }
        PlayerOnStateChange(SyncedToState.Value, GameLogicScript.Instance.CurrentGameState.Value);

    }

    private void PlayerOnStateChange(GameState previousValue, GameState newValue)
    {
        if (!IsOwner) return;
        if(newValue == GameState.SetupPhase)
        {
            GetAndSetRandomLabel();
            localStats = new RoundStatistics();
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
            CountMyPointsAndKillUsedCards();
            KillUnplayedCards();
            RoundStatistics.Value = localStats;
            UpdateRoundStats();
        }
        if (newValue == GameState.ShowSummery)
        {
            var players = FindObjectsOfType<PlayerScript>();
            ShowSummery(players);
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
            localStats.UnPlayedCardsCount++;
        }
    }

    public void GetAndSetRandomLabel()
    {
        mylabel.Value = PlayerLabels.GetRandomLabelEnum();

        labelText.text = $"#{PlayerLabels.EnumToString(mylabel.Value)}";
    }

    private void CountMyPointsAndKillUsedCards()
    {
        var AllSlots = FindObjectsOfType<SlotScheduleOnTrigger>();
        int Ppoints = 0, Tpoints = 0, unusedSlots = 0;
        List<SlotScheduleOnTrigger> SlotsWithCardsToCount = new();

        foreach (var slot in AllSlots)
        {
            if (slot.TaskCard != null)
            {
                SlotsWithCardsToCount.Add(slot);
            }
            else
            {
                unusedSlots++;
            }
        }

        foreach (var slot in SlotsWithCardsToCount)
        {
            if (slot.TaskCard != null)
            {
                var card = slot.TaskCard;
                Ppoints += card.PersonalPoints;
                Tpoints += card.TeamPoints;
                ScheduleSlotsManagerScript.Instance.RemoveCardFromAllItsSlots(slot, card);
                CardSlotsManager.InstanceSlotManager.availableSlot[card.SlotIndex] = true;
                Destroy(card.gameObject);
            }
            slot.isUsedAsCoopCard = false;
        }

        localStats.TeamPoints = Tpoints;
        localStats.PersonalPoints = Ppoints;
        localStats.unusedSlots = unusedSlots;

        //stats "scriptable object"
        var temp = new PlayerScore()
        {
            TeamPoints = Score.Value.TeamPoints + Tpoints,
            PersonalPoints = Score.Value.PersonalPoints + Ppoints,
        };
        Score.Value = temp;
        
        Debug.Log($"adding {Ppoints}P {Tpoints}T points for player named:{PlayerName.Value}");
    }

    public void ShowSummery(PlayerScript[] Players)
    {
        Debug.Log("player id: " + OwnerClientId);
        StatisticsScript.Instance.WriteStatsInSummery(Stats);
        SummeryAnimation.Singelton.OnOpeningWindow();
        ScoreboardManegar.Singelton.SetScoreboared(Players);
        NotificationsManager.Singleton.ClearNotifications();
    }

    public void UpdateRoundStats()
    {
        Stats.Add(localStats);
    }

    //private void OnDestroy()
    //{
    //    GameLogicScript.OnStateChange -= PlayerOnStateChange;
    //}
}
