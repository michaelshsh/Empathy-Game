using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationWindowAnimation : MonoBehaviour
{
    public static NotificationWindowAnimation Singlton;

    private void Awake()
    {
        NotificationWindowAnimation.Singlton = this;
    }
    private void Start()
    {
        transform.localScale = Vector2.zero;
    }

    public void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f);
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }
}
