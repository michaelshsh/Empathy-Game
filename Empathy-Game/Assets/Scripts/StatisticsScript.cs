using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Unity.Netcode;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using static Constants.RandomConsts;

public class StatisticsScript : NetworkBehaviour
{
    public static StatisticsScript Instance { get; private set; }

    //public Dictionary<ulong, List<RoundStatistics>>  PlayerStats = 
    //    new Dictionary<ulong, List<RoundStatistics>>();
    

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

    public override void OnNetworkSpawn()
    {
        Instance = this;
    }

    //public void UpdateAllPlayersStatistics()
    //{
    //    var players = FindObjectsOfType<PlayerScript>();
    //    foreach (var player in players)
    //    {
            
    //        PlayerStats.TryGetValue(player.OwnerClientId, out var score);
    //        if (score == null)
    //        {
    //            PlayerStats[player.OwnerClientId] = new List<RoundStatistics>();
    //        }

    //        PlayerStats.Value[player.OwnerClientId].Add(player.RoundStatistics.Value);
    //    }
    //}

    //public List<RoundStatistics> GetPlayerScore(ulong OwnerClientId)
    //{
    //    return PlayerStats.Value[OwnerClientId];
    //}

    //public List<RoundStatistics> GetPlayerScore(PlayerScript PlayerScript) 
    //    => GetPlayerScore(PlayerScript.OwnerClientId);


    //comparing this round with privious
    public string RoundComper(RoundStatistics myStats)
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        //var myStats = GetPlayerScore(this.OwnerClientId);
        StringBuilder text = new StringBuilder("");

        if (myStats.PersonalPoints > myStats.TeamPoints)
        {
            text.AppendLine("You colleted more personal pointes then group points.\n" +
                "Try to collect more group points in the next round");
        }
        else if (myStats.PersonalPoints == myStats.TeamPoints)
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

    public string RoundBetweenComperPersonal(List<RoundStatistics> myStats)
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        //var myStats = GetPlayerScore(this.OwnerClientId);
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

    public string RoundBetweenComperTeam(List<RoundStatistics> myStats)
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        //var myStats = GetPlayerScore(this.OwnerClientId);
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

    //public string EndOfGameSumm()
    //{
    //    var myStats = GetPlayerScore(this.OwnerClientId);
    //    StringBuilder text = new StringBuilder("");

    //    text.AppendLine($"You collected {myStats.Sum(x=>x.TeamPoints)} team points this game!");
    //    text.AppendLine($"You collected {myStats.Sum(x => x.PersonalPoints)} personal points this game!");
    //    text.AppendLine($"You missed {myStats.Sum(x => x.unusedSlots)} hours of work !");
    //    text.AppendLine($"You didnt play {myStats.Sum(x => x.UnPlayedCardsCount)} card that you had in your hand!");

    //    return text.ToString();
    //}

    public void WriteStatsInSummery(List<RoundStatistics> Stats)
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        //List<RoundStatistics> roundStatistics = Instance.PlayerStats.Value[id];
        StringBuilder text = new StringBuilder("");
        text.AppendLine("You personal points this round: " + Stats[roundIndex].PersonalPoints + "\n");
        text.AppendLine(RoundBetweenComperPersonal(Stats));
        text.AppendLine("You team points this round: " + Stats[roundIndex].TeamPoints + "\n");
        text.AppendLine(RoundBetweenComperTeam(Stats));
        text.AppendLine(RoundComper(Stats[roundIndex]));
        text.AppendLine("Number of unplayed cards this round: " + Stats[roundIndex].UnPlayedCardsCount + "\n");
        text.AppendLine("Number of unused slots in your schedule this round: " + Stats[roundIndex].unusedSlots + "\n");

        if (DidPassTheLimitScore())
        {
            text.AppendLine("You and your team have passed the team score limit.\n");
        }
        else
        {
            text.AppendLine("You and your team didn't pass the team score limit.\n ");
            GameLogicScript.Instance.UpdateGameByState(GameState.GameEnd);
        }
            
        SummeryAnimation.Singelton.SummeryText.text = text.ToString();
    }

    public void WriteStatsInSummeryGameOver(List<RoundStatistics> Stats)
    {
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;
        StringBuilder text = new StringBuilder("");
        text.AppendLine("You personal points this round: " + Stats[roundIndex].PersonalPoints + "\n");
        text.AppendLine(RoundBetweenComperPersonal(Stats));
        text.AppendLine("You team points this round: " + Stats[roundIndex].TeamPoints + "\n");
        text.AppendLine(RoundBetweenComperTeam(Stats));
        text.AppendLine(RoundComper(Stats[roundIndex]));
        text.AppendLine("Number of unplayed cards this round: " + Stats[roundIndex].UnPlayedCardsCount + "\n");
        text.AppendLine("Number of unused slots in your schedule this round: " + Stats[roundIndex].unusedSlots + "\n");

        if (DidPassTheLimitScore())
        {
            text.AppendLine("You and your team have passed the team score limit.\n");
        }  
        else
        {
            text.AppendLine("You and your team didn't pass the team score limit.\n ");
            text.AppendLine("You can see your score in the scoreboard.\n");
        }
            

        var Players = FindObjectsOfType<PlayerScript>();
        int max = 0;
        int playerP = 0;
        foreach (var player in Players)
        {
            if (max < player.Score.Value.PersonalPoints)
                max = player.Score.Value.PersonalPoints;
            if (player.OwnerClientId.Equals(NetworkManager.Singleton.LocalClientId))
            {
                playerP = player.Score.Value.PersonalPoints;
            }
            
        }
        if (max <= playerP)
            text.AppendLine("Congratulations you have collected\n" +
                "the maximum number of personal points in the game\n");
        SummeryAnimation.Singelton.SummeryText.text = text.ToString();
    }

    private bool DidPassTheLimitScore()
    {
        int amount = 0;
        int roundIndex = RoundNumberScript.Instance.roundNumber.Value - 1;

        var players = FindObjectsOfType<PlayerScript>();
        foreach(var player in players)
        {
            RoundStatistics roundStatistics = player.RoundStatistics.Value;
            amount += roundStatistics.TeamPoints;
        }
        return amount >= Constants.RandomConsts.TeamPointsLimit * players.Length;//returna true if the team points of the team higher of the limit
    }
}
