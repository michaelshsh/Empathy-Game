using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SummeryScript : MonoBehaviour
{
    public static SummeryScript InstanceSummeryManager { get; private set; }
    [field: SerializeField]
    private List<int> PersonalPointsPerRound;
    [field: SerializeField]
    private List<int> GroupPointsPerRound;
    [field: SerializeField]
    private int round;

    private void Awake()
    {
        if (InstanceSummeryManager != null && InstanceSummeryManager != this)
        {
            Destroy(this);
        }
        else
        {
            InstanceSummeryManager = this;
        }
    }

    private void UpdatePersonalPoints(int point)
    {
        PersonalPointsPerRound[round] = PersonalPointsPerRound[round] + point;
    }

    private void UpdateGroupPoints(int point)
    {
        GroupPointsPerRound[round] = GroupPointsPerRound[round] + point;
    }

    private void StartRound()
    {
        PersonalPointsPerRound.Add(25);
        GroupPointsPerRound.Add(10);
        PersonalPointsPerRound.Add(20);
        GroupPointsPerRound.Add(20);
        round = PersonalPointsPerRound.Count - 1;
    }
    //comparing this round with privious
    public string RoundComper()
    {
        StringBuilder text = new StringBuilder("");

        if (PersonalPointsPerRound[round] > GroupPointsPerRound[round])
        {
            text.AppendLine("You colleted more personal pointes then group points.\n" +
                "Try to collect more group points int the next round");
        }
        else if (PersonalPointsPerRound[round] == GroupPointsPerRound[round])
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
        StringBuilder text = new StringBuilder("");
        if (round != 0)
        {
            if (PersonalPointsPerRound[round] > PersonalPointsPerRound[round - 1])
            {
                text.AppendLine("You collected more personal points\nin this round compeared to the previous round.");
            }
            else if (PersonalPointsPerRound[round] == PersonalPointsPerRound[round - 1])
            {
                text.AppendLine("You collected the same amount of personal points\nin the previous round to");
            }
            else
            {
                text.AppendLine("You collected more personal points\nin the previous round compeared to this round.");
            }
        }

        return text.ToString();
    }

    public string RoundBetweenComperGroup()
    {
        StringBuilder text = new StringBuilder("");
        if (round != 0)
        {
            if (GroupPointsPerRound[round] > GroupPointsPerRound[round - 1])
            {
                text.AppendLine("You collected more group points\nin this round compeared to the previous round.");
            }
            else if (GroupPointsPerRound[round] == GroupPointsPerRound[round - 1])
            {
                text.AppendLine("You collected the same amount of group points\nin the previous round to");
            }
            else
            {
                text.AppendLine("You collected more group points in the previous round\ncompeared to this round.");
            }
        }

        return text.ToString();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        InstanceSummeryManager = this;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
