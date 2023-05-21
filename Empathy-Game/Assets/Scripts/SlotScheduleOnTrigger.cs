using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    [SerializeField]
    private SlotScript slot;
    [SerializeField]
    private TextMeshPro UIText;
    public CardScript card = null;
    private bool mouseDown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
        CardScript cardThatHadEnteredSlot = collision.gameObject.GetComponent<CardScript>();
        if (card == null && !mouseDown && cardThatHadEnteredSlot != null && cardThatHadEnteredSlot.slotOnSchedule == null)
        {
            Debug.Log("OnTriggerEnter2D if");
            cardThatHadEnteredSlot.slotOnSchedule = slot;
            collision.gameObject.SetActive(false);                 
            GameObject parent = collision.gameObject.transform.Find("Text").gameObject; // get parent of FreeText_Var
            GameObject txt = parent.transform.Find("FreeText_Var").gameObject; // get FreeText_Var
            UIText.text = txt.GetComponent<TextMeshPro>().text;
            card = cardThatHadEnteredSlot;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        mouseDown = true;
        if (card != null)
        {
            Debug.Log("Mouse Down if");
            UIText.text = string.Empty;
            card.gameObject.SetActive(true);
            card.drag = true;
            card.slotOnSchedule = null;
            card = null;
        }
    }

    private void OnMouseUp()
    {
        mouseDown = false;
    }
}
