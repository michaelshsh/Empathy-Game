using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public Button DrawButton;
    public VisualElement SummeryPenal;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        DrawButton = root.Q<Button>("Draw_BT");
        SummeryPenal = root.Q<VisualElement>("SummeryPanel");

        DrawButton.clicked += CardSlotsManager.InstanceSlotManager.DrawCard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
