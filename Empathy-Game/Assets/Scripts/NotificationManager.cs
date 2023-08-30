using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class NotificationManager : NetworkBehaviour
{
    public static NotificationManager Singleton; //change name

    [SerializeField] NotificationMessage notificationPrefab;
    [SerializeField] GameObject notificationWindow;
    public Button test;

    void Awake()
    {
        NotificationManager.Singleton = this;
        test.onClick.AddListener(delegate { SendNotification("this is a notification", "player"); });
    }

    void Update()
    {
        
    }

    public void SendNotification(string msg, string _from = null, string target = null)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;
        Debug.Log("entered send notification, the msg is: " + msg);

        string S = _from + msg;
        SendNotificationServerRpc(S, target);
    }

    void AddNotification(string msg, string target)
    {
        NotificationMessage N = Instantiate(notificationPrefab, notificationWindow.transform);
        N.SetText(msg);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendNotificationServerRpc(string msg, string target)
    {
        ReciveNotificationClientRpc(msg, target);
    }

    [ClientRpc]
    void ReciveNotificationClientRpc(string msg, string target)
    {
        NotificationManager.Singleton.AddNotification(msg, target);
    }
}
