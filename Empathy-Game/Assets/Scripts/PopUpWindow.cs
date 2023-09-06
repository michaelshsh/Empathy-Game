using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpWindow : MonoBehaviour
{
    public static PopUpWindow Singleton;

    public TMP_Text popupText;
    private GameObject window;
    private Animator popupAnimator;
    private Queue<string> popupQueue;// for diffrent msg
    private Coroutine queueChecker;

    private void Awake()
    {
        PopUpWindow.Singleton = this;
    }

    private void Start()
    {
        window = transform.GetChild(0).gameObject;
        popupAnimator = window.GetComponent<Animator>();
        window.SetActive(false);
        popupQueue = new Queue<string>();
    }

    public void Addqueue(string text)
    {
        Debug.Log("Entering AddQueue");
        popupQueue.Enqueue(text);
        if (queueChecker == null)
            queueChecker = StartCoroutine(CheckQueue());
    }


    private void ShowPopup(string text)
    {
        Debug.Log("ShowPopUp");
        window.SetActive(true);
        popupText.text = text;
        popupAnimator.Play("popUp");
    }

    private IEnumerator CheckQueue()
    {
        Debug.Log("checkQueue");
        do
        {
            Debug.Log("Queue size:  " + popupQueue.Count);
            ShowPopup(popupQueue.Dequeue());
            do
            {
                yield return null;
            } while (!popupAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        } while (popupQueue.Count > 0);
        window.SetActive(false);
        queueChecker = null;
    }
}
