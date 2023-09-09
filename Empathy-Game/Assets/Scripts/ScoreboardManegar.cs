using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Constants.PlayerLabels;

public class ScoreboardManegar : MonoBehaviour
{
    public static ScoreboardManegar Singelton;

    public GameObject RowPrefab;
    public Transform RowParent;


    private void Awake()
    {
        ScoreboardManegar.Singelton = this;
    }

    public void SetScoreboared(ref PlayerScript[] Players)
    {
        foreach(Transform item in RowParent)
        {
            Destroy(item.gameObject);
        }

        GameObject newGo = Instantiate(RowPrefab, RowParent);
        TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var player in Players)
        {
            texts[0].text = player.PlayerName.ToString();
            texts[1].text = player.Score.Value.PersonalPoints.ToString();
            texts[2].text = player.Score.Value.TeamPoints.ToString();
        }
    }

    private void Start()
    {
        transform.localScale = Vector2.zero;
    }

    public void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f);
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }

}
