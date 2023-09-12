using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using static Constants.PlayerLabels;

public class NotificationsManager : NetworkBehaviour
{
    public static NotificationsManager Singleton;

    [SerializeField] Notification notificationDialog;
    [SerializeField] Notification notification;
    [SerializeField] GameObject NotificationWindow;

    private void Awake()
    {
        NotificationsManager.Singleton = this;
    }

    public void SendNotification(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _to)
    {
        Debug.Log("entered send notification");
        Debug.Log($"on SendNotification, msg - {msg} scheduleSlotName - {scheduleSlotName}");
        if (string.IsNullOrWhiteSpace(msg)) return;
        var Players = FindObjectsOfType<PlayerScript>();
        string name = "";
        foreach (var player in Players)
        {
            if (player.OwnerClientId == OwnerClientId)
            {
                name = Constants.PlayerLabels.EnumToString(player.mylabel.Value);
                break;
            }
        }

        string N = name + " " +  msg;
        SendNotificationServerRpc(N, scheduleSlotName, time, _to, new ServerRpcParams { Receive = new ServerRpcReceiveParams { SenderClientId = OwnerClientId } });
    }

    void AddNotification(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, ulong sender)
    {
        Debug.Log("added notification");
        Debug.Log("Sender id: " + sender);
        Notification N = Instantiate(notification, NotificationWindow.transform);
        N.SetText(msg, scheduleSlotName, time, sender);
        PopUpWindow.Singleton.Addqueue("You received an invitation to schedule an appointment");
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, string _to, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("in server");
        var Players = FindObjectsOfType<PlayerScript>();
        List<ulong> ids = new List<ulong>();
        var clientId = serverRpcParams.Receive.SenderClientId;

        foreach (var player in Players)
        {
            Debug.Log(Constants.PlayerLabels.EnumToString(player.mylabel.Value));
            if (_to.Equals(Constants.PlayerLabels.EnumToString(player.mylabel.Value)))
            {
                Debug.Log("added " + player.OwnerClientId + "to the list");
                if(!(player.OwnerClientId.Equals(clientId)))
                    ids.Add(player.OwnerClientId);
            }
        }
        ReceiveNotificationClientRpc(msg, scheduleSlotName, time, clientId, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });//sending to the server list of clients IDs that need to get notification.
    }

    [ClientRpc]
    void ReceiveNotificationClientRpc(string msg, string scheduleSlotName, Constants.CardTime.TimeEnum time, ulong sender, ClientRpcParams clientRpcParams = default)//only the clients that the id in the params
    {
        NotificationsManager.Singleton.AddNotification(msg, scheduleSlotName, time, sender);
    }

    public void ClearNotifications()
    {
        foreach (Transform item in NotificationWindow.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void DelteNotificationFromOtherPlayersAccept(string msg, ulong sender)
    {
        var Players = FindObjectsOfType<PlayerScript>();
        string name = "";
        foreach (var player in Players)
        {
            if (player.OwnerClientId == OwnerClientId)
            {
                name = Constants.PlayerLabels.EnumToString(player.mylabel.Value);
                break;
            }
        }
        SendToPlayerRemainderClientRpc(msg, name, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { sender } } });
        DeleteNotificationsAcceptServerRpc(msg, name);
    }

    //[ServerRpc(RequireOwnership = false)]
    //void SendToPlayerRemainderServerRpc(string msg, string _from, ServerRpcParams serverRpcParams = default)
    //{
    //    SendToPlayerRemainderClientRpc(msg, _from, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { serverRpcParams.Receive.SenderClientId } } });
    //}

    [ClientRpc]
    void SendToPlayerRemainderClientRpc(string msg, string _from, ClientRpcParams clientRpcParams = default)
    {
        Notification N = Instantiate(notificationDialog, NotificationWindow.transform);
        N.SetTextReminder("Accepted " + "from "+ _from + " " + msg);
        PopUpWindow.Singleton.Addqueue("You received an acception to schedule appointment");
    }

    [ServerRpc(RequireOwnership = false)]
    void DeleteNotificationsAcceptServerRpc(string msg, string _from)
    {
        List<ulong> ids = new List<ulong>();
        var Players = FindObjectsOfType<PlayerScript>();
        foreach (var player in Players)
        {
            if (_from.Equals(Constants.PlayerLabels.EnumToString(player.mylabel.Value)))
            {
                ids.Add(player.OwnerClientId);
            }
        }
        ListOfResiversClientRpc(msg, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });
    }

    [ClientRpc]
    void ListOfResiversClientRpc(string msg, ClientRpcParams clientRpcParams = default)
    {
        RemoveNotifications(msg);
    }

    void RemoveNotifications(string msg)
    {
        Notification[] allNotifications =  NotificationWindow.GetComponentsInChildren<Notification>();
        foreach(Notification item in allNotifications)
        {
            if (msg.Equals(item.GetText()))
            {
                Destroy(item);
            }
        }
    }

}
