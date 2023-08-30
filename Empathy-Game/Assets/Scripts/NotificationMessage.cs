using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] string from;

    public void SetText(string msg)
    {
        messageText.text = msg;
    }
}
