using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Start is called before the first frame update 

    public bool drag;
    private bool draggable;
    public SpriteRenderer mySpritRenderer;
    public TextMeshPro Time;
    public TextMeshPro PersonalPoints;
    public TextMeshPro TeamPoints;
    public TextMeshPro FreeText;
    public bool Played;
    public int SlotIndex;
    public GameObject spotInSchedule;

    void Start()
    {
        GameLogicScript.OnStateChange += CardsOnStateChange;
        Time.text = "1:30";
        PersonalPoints.text = "+15";
        TeamPoints.text = "0";
        FreeText.text = "Hello im a free text let me live";
        draggable = true;
    }

    private void CardsOnStateChange(GameState state)    
    {
        if(state == GameState.RoundStart)
        {
            draggable = true;
        }
        else
            draggable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (drag && draggable)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            worldPosition.z = 0;
            gameObject.transform.position = (gameObject.transform.position + worldPosition) / 2;
            
        }
    }

    private void OnDestroy()
    {
        GameLogicScript.OnStateChange -= CardsOnStateChange;
    }

    private void OnMouseDown()
    {
        drag = true;
        gameObject.transform.position += new Vector3(0, 0, -1);
        
    }

    private void OnMouseUp()
    {
        drag = false;
        gameObject.transform.position += new Vector3(0, 0, 1);
        if (spotInSchedule != null)
        {
            spotInSchedule.tag = "Untagged";
            spotInSchedule = null;
        }
        //Invoke("MoveToPlayedCardDeck", 2f);// Launches a MoveToPlayedCardDeck in 2 seconds - for testing needs to be moved
    }

    private void MoveToPlayedCardDeck()//not sure if it should be here
    {
        CardManager.InstanceCardManager.UsedCards.Add(this);
        CardSlotsManager.InstanceSlotManager.availableSlot[SlotIndex] = true;
        gameObject.SetActive(false);
    }
}
