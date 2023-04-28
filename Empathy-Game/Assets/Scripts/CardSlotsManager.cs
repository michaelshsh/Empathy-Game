using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardSlotsManager : MonoBehaviour
{
    public GameObject dailyScheduleSlot;
    public static CardSlotsManager InstanceSlotManager { get; private set; }
    private void Awake()
    {
        if (InstanceSlotManager != null && InstanceSlotManager != this)
        {
            Destroy(this);
        }
        else
        {
            InstanceSlotManager = this;
        }
    }
    public Transform[] Slots; // should be moved to class CardManager or create new class - Slot manager
    public bool[] availableSlot; // should be moved to class CardManager or create new class - Slot manager
    // Start is called before the first frame update
    void Start()
    {
        InstanceSlotManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        // for every CardScript, check if  isCardNearDailyScheduleSlot is true for him
        foreach(CardScript card in CardManager.InstanceCardManager.PlayedCards)
        {
            if(isCardNearDailyScheduleSlot(card, dailyScheduleSlot))
            {
                makeCardOnTopDailyScheduleSlot(card, dailyScheduleSlot); break;


            }
        }

    }

    public void DrawCard()
    {
        if (CardManager.InstanceCardManager.deck.Count >= 1)
        {
            CardScript card = CardManager.InstanceCardManager.deck[UnityEngine.Random.Range(0, CardManager.InstanceCardManager.deck.Count)];

            for (int i = 0; i < availableSlot.Length; i++)
            {
                if (availableSlot[i] == true)
                {
                    card.gameObject.SetActive(true);
                    card.transform.position = Slots[i].position;
                    card.SlotIndex = i;
                    card.Played = false;
                    availableSlot[i] = false;
                    CardManager.InstanceCardManager.deck.Remove(card);
                    CardManager.InstanceCardManager.PlayedCards.Add(card);
                    return;
                }
            }
        }
    }

    public void makeCardOnTopDailyScheduleSlot(CardScript card, GameObject dailyScheduleSlot) 
    {
        // make the card on top of the daily schedule slot
        // the card should cover all the daily schedule slot
        card.transform.position = new Vector3(dailyScheduleSlot.transform.position.x, dailyScheduleSlot.transform.position.y, dailyScheduleSlot.transform.position.z - 1);
        
    }

    public bool isCardNearDailyScheduleSlot(CardScript card, GameObject dailyScheduleSlot)
    {
        // if the card's x,y position is inside the dailyScheduleSlot x,y position then return true
        if (card.transform.position.x >= dailyScheduleSlot.transform.position.x - 0.5f &&
                       card.transform.position.x <= dailyScheduleSlot.transform.position.x + 0.5f &&
                                  card.transform.position.y >= dailyScheduleSlot.transform.position.y - 0.5f &&
                                             card.transform.position.y <= dailyScheduleSlot.transform.position.y + 0.5f)
        {
            Debug.Log("Card is near daily schedule slot");
            // debug the card's id
            Debug.Log(card);
            return true;
            
        }
        else
        {
            return false;

        }
    }

}
