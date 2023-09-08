using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI msg;
    string scheduleSlotName;

    public void SetText(string str, string _scheduleSlotName)
    {
        msg.text = str;
        scheduleSlotName = _scheduleSlotName;
        Debug.Log($"in SetText()  -  msg.txt - {msg.text}, scheduleSlotName - {scheduleSlotName} ");
    }

    public void RemoveNotification()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
