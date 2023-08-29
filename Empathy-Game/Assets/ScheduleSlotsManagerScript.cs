using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleSlotsManagerScript : MonoBehaviour
{
    [SerializeField] List<SlotScheduleOnTrigger> slotsList;
    // Start is called before the first frame update
    public static ScheduleSlotsManagerScript Instance { get; private set; }

    public void TryToInsertCardAt(CardScript card, int indexInList)
    {
        bool canInsert = false;
        List<SlotScheduleOnTrigger> slotsToInsert = new();

        if(card.time == Constants.CardTime.TimeEnum.OneHour)
        {
            canInsert = slotsList[indexInList].IsAvilableForCard;
            slotsToInsert.Add(slotsList[indexInList]);
        }
        else if(card.time == Constants.CardTime.TimeEnum.TwoHours && indexInList + 1 < slotsList.Count)
        {
            canInsert = slotsList[indexInList].IsAvilableForCard;
            canInsert &= slotsList[indexInList + 1].IsAvilableForCard;
            slotsToInsert.Add(slotsList[indexInList]);
            slotsToInsert.Add(slotsList[indexInList+1]);
        }
        else if (card.time == Constants.CardTime.TimeEnum.ThreeHours && indexInList+2 < slotsList.Count)
        {
            canInsert = slotsList[indexInList].IsAvilableForCard;
            canInsert &= slotsList[indexInList + 1].IsAvilableForCard;
            canInsert &= slotsList[indexInList + 2].IsAvilableForCard;
            slotsToInsert.Add(slotsList[indexInList]);
            slotsToInsert.Add(slotsList[indexInList+1]);
            slotsToInsert.Add(slotsList[indexInList+2]);
        }

        if(canInsert)
        {
            foreach(var slot in slotsToInsert)
            {
                slot.InsertCard(card);
            }
            card.InsertedToASlot = true;
            card.gameObject.SetActive(false);
        }
    }

    public void RemoveCardFromAllItsSlots(SlotScheduleOnTrigger slot, CardScript cardToRemove)
    {
        if (slot.TaskCard != cardToRemove)
            return;

        slot.RemoveCard();
        //if the card is in Adjacent slot
        if (slot.IndexInList > 0)
        {
            RemoveAdjacentUp(slotsList[slot.IndexInList - 1], cardToRemove);

        }
        if (slot.IndexInList < slotsList.Count - 1)
        {
            RemoveAdjacentDown(slotsList[slot.IndexInList + 1], cardToRemove);
        }

        cardToRemove.gameObject.SetActive(true);
        cardToRemove.InsertedToASlot = false;
        cardToRemove.GetComponent<Collider2D>().enabled = false;
        //cardToRemove.drag = true;
        cardToRemove.makeCardIgnoreOtherCards();
        cardToRemove.Invoke("EnableCollider", 1.0f);
    }

    private void RemoveAdjacentUp(SlotScheduleOnTrigger slot, CardScript cardToRemove)
    {
        if (slot.TaskCard != cardToRemove)
            return;

        slot.RemoveCard();
        //if the card is in Adjacent slot
        if (slot.IndexInList > 0)
        {
            RemoveAdjacentUp(slotsList[slot.IndexInList - 1], cardToRemove);

        }
    }
    private void RemoveAdjacentDown(SlotScheduleOnTrigger slot, CardScript cardToRemove)
    {
        if (slot.TaskCard != cardToRemove)
            return;

        slot.RemoveCard();
        //if the card is in Adjacent slot
        if (slot.IndexInList < slotsList.Count - 1)
        {
            RemoveAdjacentDown(slotsList[slot.IndexInList + 1], cardToRemove);
        }
    }

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

    void Start()
    {
        for(int i=0; i< slotsList.Count; i++)
        {
            slotsList[i].IndexInList = i;
        }
    }

    
}
