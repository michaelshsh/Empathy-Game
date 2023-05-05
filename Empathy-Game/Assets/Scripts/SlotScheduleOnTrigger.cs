using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;
    [SerializeField]
    private TextMeshPro UIText;
    private CardScript card = null;
    private bool mouseDown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (card == null && !mouseDown)
        {
            collision.gameObject.SetActive(false);
            GameObject parent = collision.gameObject.transform.Find("Text").gameObject; // get parent of FreeText_Var
            GameObject txt = parent.transform.Find("FreeText_Var").gameObject; // get FreeText_Var
            UIText.text = txt.GetComponent<TextMeshPro>().text;
            card = collision.gameObject.GetComponent<CardScript>();
        }
    }

    private void OnMouseDown()
    {
        mouseDown = true;
        if (card != null)
        {
            card.gameObject.transform.position = new Vector3(50, -50, 0);
            UIText.text = string.Empty;
            card.gameObject.SetActive(true);
            card = null;
        }
    }

    private void OnMouseUp()
    {
        mouseDown = false;
    }
}
