using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardSlotsManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> dailyScheduleSlots;



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
        foreach (GameObject slot in dailyScheduleSlots)
        {
            if (slot.tag == "occupied")
                continue;
            foreach (CardScript card in CardManager.InstanceCardManager.PlayedCards)
            {
                if (isCardNearDailyScheduleSlot(card, slot))
                {
                    makeCardOnTopDailyScheduleSlot(card, slot);
                    Debug.Log("setting" + card + "on" + slot);
                }
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
        dailyScheduleSlot.tag = "occupied"; // make the daily schedule slot tag to occupied
        Debug.Log("Setting Occupied");
        card.spotInSchedule = dailyScheduleSlot;
    }

    public bool isCardNearDailyScheduleSlot(CardScript card, GameObject dailyScheduleSlot)
    {
        // if the card's x,y position is inside the dailyScheduleSlot x,y position then return true
        if (card.transform.position.x >= dailyScheduleSlot.transform.position.x - 0.8f &&
                       card.transform.position.x <= dailyScheduleSlot.transform.position.x + 0.8f &&
                                  card.transform.position.y >= dailyScheduleSlot.transform.position.y - 0.8f &&
                                             card.transform.position.y <= dailyScheduleSlot.transform.position.y + 0.8f)
        {
            return true;       
        }
        else
        {
            return false;

        }
    }

}
