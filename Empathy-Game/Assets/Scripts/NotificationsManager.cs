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

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SendNotification(string msg, string _from = null, string _to = null)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;

        string N = _from + msg;
        SendNotificationServerRpc(N);
    }

    void AddNotification(string msg)
    {
        Notification N = Instantiate(notification, NotificationWindow.transform);
        N.SetText(msg);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(string msg)
    {
        ReceiveNotificationClientRpc(msg);
    }

    [ClientRpc]
    void ReceiveNotificationClientRpc(string msg)
    {
        NotificationsManager.Singleton.AddNotification(msg);
    }
}
