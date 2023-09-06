using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NotificationsManager : NetworkBehaviour
{
    public static NotificationsManager Singleton;

    [SerializeField] Notification notification;
    [SerializeField] GameObject NotificationWindow;

    private void Awake()
    {
        NotificationsManager.Singleton = this;
    }

    public void SendNotification(string msg, string _from = null, string _to = null)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;

        string N = _from + msg;
       
        SendNotificationServerRpc(N, _to);
    }

    void AddNotification(string msg)
    {
        Notification N = Instantiate(notification, NotificationWindow.transform);
        N.SetText(msg);
        PopUpWindow.Singleton.Addqueue("You received an invitation to schedule an appointment");
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(string msg, string _to)
    {
        var Players = FindObjectsOfType<PlayerScript>();
        List<ulong> ids = new List<ulong>();

        foreach (var player in Players)
        {
            if (_to.Equals(player.mylabel.Name))
            {
                ids.Add(player.OwnerClientId);
            }
        }
        ReceiveNotificationClientRpc(msg, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>(ids) } });//sending to the server list of clients IDs that need to get notification.
    }

    [ClientRpc]
    void ReceiveNotificationClientRpc(string msg, ClientRpcParams clientRpcParams = default)//only the clients that the id in the params
    {
        NotificationsManager.Singleton.AddNotification(msg);
    }

}
