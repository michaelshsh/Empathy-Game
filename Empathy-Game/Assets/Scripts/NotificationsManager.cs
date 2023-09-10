using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using static Constants.PlayerLabels;

public class NotificationsManager : NetworkBehaviour
{
    public static NotificationsManager Singleton;

    [SerializeField] Notification notification;
    [SerializeField] GameObject NotificationWindow;

    private void Awake()
    {
        NotificationsManager.Singleton = this;
    }

    public void SendNotification(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _from, string _to)
    {
        Debug.Log("entered send notification");
        Debug.Log($"on SendNotification, msg - {msg} scheduleSlotName - {scheduleSlotName}");
        if (string.IsNullOrWhiteSpace(msg)) return;

        string N = _from + " " +  msg;
        SendNotificationServerRpc(N, scheduleSlotName, time, _to);
    }

    void AddNotification(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time)
    {
        Debug.Log("added notification");
        Notification N = Instantiate(notification, NotificationWindow.transform);
        N.SetText(msg, scheduleSlotName, time);
        PopUpWindow.Singleton.Addqueue("You received an invitation to schedule an appointment");
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _to)
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
        ReceiveNotificationClientRpc(msg, scheduleSlotName, time,  new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });//sending to the server list of clients IDs that need to get notification.
    }

    [ClientRpc]
    void ReceiveNotificationClientRpc(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, ClientRpcParams clientRpcParams = default)//only the clients that the id in the params
    {
        NotificationsManager.Singleton.AddNotification(msg, scheduleSlotName, time);
    }

    public void ClearNotifications()
    {
        foreach (Transform item in NotificationWindow.transform)
        {
            Destroy(item.gameObject);
        }
    }

}
