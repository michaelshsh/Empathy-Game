using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    public GameObject slot;
    public TextMeshPro UIText;
    private CardScript card = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entering OnTriggerEnter2D");
        if (card == null)
        {
            collision.gameObject.SetActive(false);
            GameObject parent = collision.gameObject.transform.Find("Text").gameObject;
            GameObject txt = parent.transform.Find("FreeText_Var").gameObject;
            UIText.text = txt.GetComponent<TextMeshPro>().text;
            card = collision.gameObject.GetComponent<CardScript>();
            slot.gameObject.GetComponent<Collider2D>().isTrigger = false;
            Debug.Log("card was set to be the new card");

        }  
    }
    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (card != null)
        {
            card.gameObject.transform.position = Vector3.zero;
            card.gameObject.SetActive(true);             
            card = null;
            Debug.Log("card set to null");
            //slot.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
