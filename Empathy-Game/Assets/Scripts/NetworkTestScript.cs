using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NetworkTestScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button HostButton;
    [SerializeField] private UnityEngine.UI.Button ClientButton;
    // Start is called before the first frame update
    void Start()
    {
        /*HostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host button");
            NetworkManager.Singleton.StartHost();
        });
        ClientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client button");
            NetworkManager.Singleton.StartClient();
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
