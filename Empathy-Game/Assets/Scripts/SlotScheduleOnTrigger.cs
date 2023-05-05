using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    public GameObject slot;
    public TextMeshPro UIText;
    private CardScript card = null;
    private bool mouseDown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entering OnTriggerEnter2D");
        if (card == null && !mouseDown)
        {
            collision.gameObject.SetActive(false);
            GameObject parent = collision.gameObject.transform.Find("Text").gameObject;
            GameObject txt = parent.transform.Find("FreeText_Var").gameObject;
            UIText.text = txt.GetComponent<TextMeshPro>().text;
            card = collision.gameObject.GetComponent<CardScript>();
            Debug.Log("card was set to be the new card");

        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        mouseDown = true;
        if (card != null)
        {
            card.gameObject.transform.position = new Vector3(50, -50, 0);
            UIText.text = "";
            card.gameObject.SetActive(true);
            card = null;

        }
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        mouseDown = false;
    }
}