using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Unity.Netcode;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class StatisticsScript : NetworkBehaviour
{
    public static StatisticsScript Instance { get; private set; }

    public NetworkVariable<Dictionary<ulong, List<RoundStatistics>>>  PlayerStats = 
        new(new Dictionary<ulong, List<RoundStatistics>>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public TextMeshProUGUI SummeryText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateAllPlayersStatistics()
    {
        var players = FindObjectsOfType<PlayerScript>();
        foreach (var player in players)
        {
            
            PlayerStats.Value.TryGetValue(player.OwnerClientId, out var score);
            if (score == null)
            {
                PlayerStats.Value[player.OwnerClientId] = new List<RoundStatistics>();
            }

            PlayerStats.Value[player.OwnerClientId].Add(player.RoundStatistics.Value);
        }
    }

    public List<RoundStatistics> GetPlayerScore(ulong OwnerClientId)
    {
        return PlayerStats.Value[OwnerClientId];
    }

    public List<RoundStatistics> GetPlayerScore(PlayerScript PlayerScript) 
        => GetPlayerScore(PlayerScript.OwnerClientId);


    //comparing this round with privious
    public string RoundComper()
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        var myStats = GetPlayerScore(this.OwnerClientId);
        StringBuilder text = new StringBuilder("");

        if (myStats[roundIndex].PersonalPoints > myStats[roundIndex].TeamPoints)
        {
            text.AppendLine("You colleted more personal pointes then group points.\n" +
                "Try to collect more group points in the next round");
        }
        else if (myStats[roundIndex].PersonalPoints == myStats[roundIndex].TeamPoints)
        {
            text.AppendLine("You have a good balance between your group points\nand personal points, keep this up!!!");
        }
        else
        {
            text.AppendLine("You colleted more group pointes then personal points.\n" +
                "Try to collect more personal points in the next round");
        }

        return text.ToString();
    }

    public string RoundBetweenComperPersonal()
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        var myStats = GetPlayerScore(this.OwnerClientId);
        StringBuilder text = new StringBuilder("");
        if (roundIndex != 0)
        {
            if (myStats[roundIndex].PersonalPoints > myStats[roundIndex-1].PersonalPoints)
            {
                text.AppendLine("You collected more personal points\nin this round compeared to the previous round.");
            }
            else if (myStats[roundIndex].PersonalPoints == myStats[roundIndex - 1].PersonalPoints)
            {
                text.AppendLine("You collected the same amount of personal points\nin the previous round too");
            }
            else
            {
                text.AppendLine("You collected more personal points\nin the previous round compeared to this round.");
            }
        }

        return text.ToString();
    }

    public string RoundBetweenComperTeam()
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        var myStats = GetPlayerScore(this.OwnerClientId);
        StringBuilder text = new StringBuilder("");
        if (roundIndex != 0)
        {
            if (myStats[roundIndex].TeamPoints > myStats[roundIndex - 1].TeamPoints)
            {
                text.AppendLine("You collected more team points\nin this round compeared to the previous round.");
            }
            else if (myStats[roundIndex].TeamPoints == myStats[roundIndex - 1].TeamPoints)
            {
                text.AppendLine("You collected the same amount of team points\nin the previous round too");
            }
            else
            {
                text.AppendLine("You collected more team points\nin the previous round compeared to this round.");
            }
        }

        return text.ToString();
    }

    public string EndOfGameSumm()
    {
        var myStats = GetPlayerScore(this.OwnerClientId);
        StringBuilder text = new StringBuilder("");

        text.AppendLine($"You collected {myStats.Sum(x=>x.TeamPoints)} team points this game!");
        text.AppendLine($"You collected {myStats.Sum(x => x.PersonalPoints)} personal points this game!");
        text.AppendLine($"You missed {myStats.Sum(x => x.unusedSlots)} hours of work !");
        text.AppendLine($"You didnt play {myStats.Sum(x => x.UnPlayedCardsCount)} card that you had in your hand!");

        return text.ToString();
    }

    public void WriteStatsInSummery()
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        List<RoundStatistics> roundStatistics = PlayerStats.Value[OwnerClientId];
        StringBuilder text = new StringBuilder("");
        text.AppendLine("You personal points this round: " + roundStatistics[roundIndex].PersonalPoints + "\n");
        text.AppendLine(RoundBetweenComperPersonal());
        text.AppendLine("You team points this round: " + roundStatistics[roundIndex].TeamPoints + "\n");
        text.AppendLine(RoundBetweenComperTeam());
        text.AppendLine(RoundComper());
        text.AppendLine("Number of unplayed cards this round: " + roundStatistics[roundIndex].UnPlayedCardsCount + "\n");
        text.AppendLine("Number of unused slots in your schedule this round: " + roundStatistics[roundIndex].unusedSlots + "\n");
        SummeryText.text = text.ToString();
    }
}
