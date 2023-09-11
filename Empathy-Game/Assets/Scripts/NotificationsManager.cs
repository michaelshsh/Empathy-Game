using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NotificationsManager : NetworkBehaviour
{
    public static NotificationsManager Singleton;

    [SerializeField] Notification notification;
    [SerializeField] GameObject NotificationWindow;

    private void Awake()
    {
        NotificationsManager.Singleton = this;
    }

    public void SendNotification(ulong sender, string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _from, string _to)
    {
        Debug.Log("entered send notification");
        Debug.Log($"on SendNotification, msg - {msg} scheduleSlotName - {scheduleSlotName}");
        if (string.IsNullOrWhiteSpace(msg)) return;

        string N = _from + " " +  msg;
        SendNotificationServerRpc(sender, N, scheduleSlotName, time, _to);
    }

    void AddNotification(ulong sender, string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time)
    {
        Debug.Log("added notification");
        Notification N = Instantiate(notification, NotificationWindow.transform);
        N.SetText(msg, scheduleSlotName, time, sender);
        PopUpWindow.Singleton.Addqueue("You received an invitation to schedule an appointment");
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(ulong sender, string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _to)
    {
        Debug.Log("in server");
        var Players = FindObjectsOfType<PlayerScript>();
        List<ulong> ids = new List<ulong>();

        foreach (var player in Players)
        {
            Debug.Log(Constants.PlayerLabels.EnumToString(player.mylabel.Value));
            if (_to.Equals(Constants.PlayerLabels.EnumToString(player.mylabel.Value)))
            {
                Debug.Log("added " + player.OwnerClientId + "to the list");
                ids.Add(player.OwnerClientId);
            }
        }
        ReceiveNotificationClientRpc(sender, msg, scheduleSlotName, time,  new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });//sending to the server list of clients IDs that need to get notification.
    }

    [ClientRpc]
    void ReceiveNotificationClientRpc(ulong sender, string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, ClientRpcParams clientRpcParams = default)//only the clients that the id in the params
    {
        NotificationsManager.Singleton.AddNotification(sender, msg, scheduleSlotName, time);
    }

    public void ClearNotifications()
    {
        foreach (Transform item in NotificationWindow.transform)
        {
            Destroy(item.gameObject);
        }
    }
    public void sendApprovalNotificationToServer(Constants.CardTime.TimeEnum time, string scheduleSlotName, ulong to, ulong from)
    {
        Debug.Log("in sendApprovalNotificationToServer");
        SendApprovalNotificationToServerRpc(time, scheduleSlotName, to, from);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendApprovalNotificationToServerRpc(Constants.CardTime.TimeEnum time, string scheduleSlotName, ulong to, ulong from)
    {
        List<ulong> ids = new List<ulong>();
        ids.Add(to);
        Debug.Log($"in SetText()  - to - {to}, scheduleSlotName - {scheduleSlotName}, time - {time}, from - {from} in SendApprovalNotificationToServerRpc");
        ReceiveNotificationApprovalClientRpc(time, scheduleSlotName, from, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });//sending to the server list of clients IDs that need to get notification.
    }

    [ClientRpc]
    void ReceiveNotificationApprovalClientRpc(Constants.CardTime.TimeEnum time, string scheduleSlotName, ulong from, ClientRpcParams clientRpcParams = default)
    {
        // debug all the parameters values
        Debug.Log("in ReceiveNotificationApprovalClientRpc");
        Debug.Log($"time - {time}, scheduleSlotName - {scheduleSlotName}, from {from}");
        SlotScheduleOnTrigger slot = ScheduleSlotsManagerScript.Instance.extractScheduleSlot(ScheduleSlotsManagerScript.Instance.slotsList, scheduleSlotName);
        ScheduleSlotsManagerScript.Instance.LockCardsUntilEndRound(slot, scheduleSlotName, time);
    }
}
