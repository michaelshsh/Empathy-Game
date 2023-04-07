using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicScript : MonoBehaviour
{
    public TimerScript roundTimer;

    public List<CardScript> deck = new List<CardScript>();
    public Transform[] Slots; // the cads slots objects
    public bool[] availableSlot;

    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            CardScript card = deck[Random.Range(0, deck.Count)];

            for(int i = 0; i < availableSlot.Length; i++)
            {
                if(availableSlot[i] == true)
                {
                    card.gameObject.SetActive(true);
                    card.transform.position = Slots[i].position;
                    availableSlot[i] = false;
                    deck.Remove(card);
                    return;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        roundTimer.timeRemaining = 5;
        roundTimer.timerIsRunning = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}