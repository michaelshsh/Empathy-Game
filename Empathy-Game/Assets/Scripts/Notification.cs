using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI msg;

    public void SetText(string str)
    {
        msg.text = str;
    }
}
