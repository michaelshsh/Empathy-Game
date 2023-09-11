using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.Netcode;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI msg;
    string scheduleSlotName;

    [SerializeField] Button accept;
    Constants.CardTime.TimeEnum time;
    public ulong sender;
    void Start()
    {
        accept.onClick.AddListener(AcceptClicked);
    }
    public void SetText(string str, string _scheduleSlotName, Constants.CardTime.TimeEnum _time, ulong _sender)
    {
        msg.text = str;
        scheduleSlotName = _scheduleSlotName;
        time = _time;
        sender = _sender;
        Debug.Log($"in SetText()  -  msg.txt - {msg.text}, scheduleSlotName - {scheduleSlotName}, time - {time}, sender - {sender}, SetText()");
    }

    public void RemoveNotification()
    {
        gameObject.SetActive(false);
    }

    public void AcceptClicked()
    {
        Debug.Log("Accept Button clicked");
        SlotScheduleOnTrigger slot = ScheduleSlotsManagerScript.Instance.extractScheduleSlot(ScheduleSlotsManagerScript.Instance.slotsList, scheduleSlotName);
        Debug.Log($"slot name - {slot.name}");
        ScheduleSlotsManagerScript.Instance.TryToInsertCoopCardAt(time, msg, slot.IndexInList);
        NotificationsManager.Singleton.sendApprovalNotificationToServer(time, scheduleSlotName, sender, NetworkManager.Singleton.LocalClientId); // from, to
    }

    

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
