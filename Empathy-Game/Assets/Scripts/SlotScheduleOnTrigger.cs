using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotScheduleOnTrigger : MonoBehaviour
{
    public GameObject slot;
    public TextMeshPro UIText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        collision.gameObject.transform.localPosition = Vector3.zero;
        GameObject parent = collision.gameObject.transform.Find("Text").gameObject;
        GameObject txt = parent.transform.Find("FreeText_Var").gameObject;
        UIText.text = txt.GetComponent<TextMeshPro>().text;
    }
}
