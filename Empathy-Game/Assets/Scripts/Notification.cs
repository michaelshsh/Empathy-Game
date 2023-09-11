using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static Constants.CardTime;

public class Notification : MonoBehaviour
{
    public static Notification Singelton;

    [SerializeField] TextMeshProUGUI msg;
    string scheduleSlotName;
    [SerializeField] Button accept;
    [SerializeField] Button reject;
    Constants.CardTime.TimeEnum time;
    [SerializeField] ulong _from;
    void Start()
    {
        Notification.Singelton = this;
        accept.onClick.AddListener(AcceptClicked);
        reject.onClick.AddListener(RejectClicked);
    }
    public void SetText(string str, string _scheduleSlotName, Constants.CardTime.TimeEnum _time, ulong sender)
    {
        scheduleSlotName = _scheduleSlotName;
        time = _time;
        _from = sender;
        SlotScheduleOnTrigger slot = extractScheduleSlot(ScheduleSlotsManagerScript.Instance.slotsList, scheduleSlotName);
        int time_ = 9 + slot.IndexInList;
        msg.text = str + " for " + Constants.CardTime.EnumToString(_time) + " hours at " + time_.ToString() ;
        Debug.Log($"in SetText()  -  msg.txt - {msg.text}, scheduleSlotName - {scheduleSlotName}, time - {time}");
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
        ScheduleSlotsManagerScript.Instance.TryToInsertCoopCardAt(time, msg, slot.IndexInList);
        NotificationsManager.Singleton.DelteNotificationFromOtherPlayersAccept(msg.text, _from);
        // slot.SetTextOnSlot(msg.text);
    }

    public void RejectClicked()
    {
        RemoveNotification();
    }

    public SlotScheduleOnTrigger extractScheduleSlot(List<SlotScheduleOnTrigger> slotsScheduleOnTrigger, string scheduleSlotName)
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

    public string GetText()
    {
        return msg.text;
    }

    public void SetTextReminder(string s)
    {
        msg.text = s;
    }
}
