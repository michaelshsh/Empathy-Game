using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI msg;
    string scheduleSlotName;
    [SerializeField] Button accept;
    void Start()
    {
        accept.onClick.AddListener(AcceptClicked);
    }
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

    public void AcceptClicked()
    {
        Debug.Log("Accept Button clicked");
        SlotScheduleOnTrigger slot = extractScheduleSlot(ScheduleSlotsManagerScript.Instance.slotsList, scheduleSlotName);
        Debug.Log($"slot name - {slot.name}");
    }

    private SlotScheduleOnTrigger extractScheduleSlot(List<SlotScheduleOnTrigger> slotsScheduleOnTrigger, string scheduleSlotName)
    {
        int slotNumber;
        // Assuming location will always be in the format "slot X" where X is a number.
        if (int.TryParse(scheduleSlotName.Split(' ')[1], out slotNumber))
        {
            // Assuming slot numbers start from 9 and go up to 17.
            if (slotNumber >= 9 && slotNumber <= 17)
            {
                return slotsScheduleOnTrigger[slotNumber - 9];
            }
        }
        throw new ArgumentException($"Invalid location provided, scheduleSlotName - {scheduleSlotName}");
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
