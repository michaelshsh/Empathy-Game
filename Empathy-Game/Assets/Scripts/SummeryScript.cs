using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummeryScript : MonoBehaviour
{
    public static SummeryScript InstanceSummeryManager { get; private set; }
    [field: SerializeField]
    private List<int> PersonalPointsPerRound;
    [field: SerializeField]
    private List<int> GroupPointsPerRound;
    [field: SerializeField]
    private int round = 0;

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
        PersonalPointsPerRound[round - 1] = PersonalPointsPerRound[round - 1] + point;
    }

    private void UpdateGroupPoints(int point)
    {
        GroupPointsPerRound[round - 1] = GroupPointsPerRound[round - 1] + point;
    }

    private void StartRound()
    {
        round++;
        PersonalPointsPerRound.Add(0);
        GroupPointsPerRound.Add(0);
    }
    //comparing this round with privious

    // Start is called before the first frame update
    void Start()
    {
        InstanceSummeryManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
