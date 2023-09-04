using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Unity.Netcode;
using System.Linq;

public class SummeryScript : NetworkBehaviour
{
    public static SummeryScript Instance { get; private set; }

    public NetworkVariable<Dictionary<ulong, List<PlayerRoundStatistics>>>  ScoreList = 
        new(new Dictionary<ulong, List<PlayerRoundStatistics>>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
        if(!IsServer) return;

        var players = FindObjectsOfType<PlayerScript>();
        foreach (var player in players)
        {
            
            ScoreList.Value.TryGetValue(player.OwnerClientId, out var score);
            if (score == null)
            {
                ScoreList.Value[player.OwnerClientId] = new List<PlayerRoundStatistics>();
            }

            ScoreList.Value[player.OwnerClientId].Add(player.RoundStatistics.Value);
        }
    }

    public List<PlayerRoundStatistics> GetPlayerScore(ulong OwnerClientId)
    {
        return ScoreList.Value[OwnerClientId];
    }

    public List<PlayerRoundStatistics> GetPlayerScore(PlayerScript PlayerScript) 
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
}
