using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Constants.PlayerLabels;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    public TextMeshPro UIText;
    public CardScript TaskCard = null;
    public int IndexInList;
    public bool isUsedAsCoopCard = false;
    public bool IsAvilableForCard => (TaskCard == null && !isUsedAsCoopCard);
    private bool mouseDown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided");
        CardScript cardThatHadEnteredSlot = collision.gameObject.GetComponent<CardScript>();
        if (TaskCard == null && cardThatHadEnteredSlot != null && !mouseDown && !cardThatHadEnteredSlot.InsertedToASlot)
        {
            Debug.Log($"{cardThatHadEnteredSlot} had a collision with slot in index {IndexInList}");
            ScheduleSlotsManagerScript.Instance.TryToInsertCardAt(cardThatHadEnteredSlot, IndexInList);
        }
        if (cardThatHadEnteredSlot.isCoopCard)
        {
            NotificationsManager.Singleton.SendNotification("wants to meet with you", gameObject.name,  "QA", Constants.PlayerLabels.EnumToString(cardThatHadEnteredSlot.requiredLabel));
        }
    }

    public bool InsertCard(CardScript card)
    {
        if(!IsAvilableForCard)
            return false;
        TaskCard = card;
        GameObject parent = card.gameObject.transform.Find("Text").gameObject; // get parent of FreeText_Var
        GameObject txt = parent.transform.Find("FreeText_Var").gameObject; // get FreeText_Var
        UIText.text = txt.GetComponent<TextMeshPro>().text;
        return true;
    }

    public bool RemoveCard()
    {
        if (TaskCard == null)
            return false;

        UIText.text = string.Empty;
        TaskCard = null;
        
        return true;
    }

    private void OnMouseDown()
    {
        mouseDown = true;
        if (TaskCard != null)
            ScheduleSlotsManagerScript.Instance.RemoveCardFromAllItsSlots(this, this.TaskCard);
    }
    private void OnMouseUp()
    {
        mouseDown = false;
    }
    private void Update()
    {
        
    }

}
