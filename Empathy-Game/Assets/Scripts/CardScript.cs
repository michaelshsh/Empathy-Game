using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Start is called before the first frame update 
    private bool draggable;
    
    [SerializeField] private bool drag;
    public bool Drag { get { return draggable; } set { draggable = value; } }

    [SerializeField] private SpriteRenderer mySpritRenderer;
    public SpriteRenderer SpriteRenderer { get { return mySpritRenderer; } set { mySpritRenderer = value; } }

    [SerializeField] private CardTime.TimeEnum time;
    public CardTime.TimeEnum Time {get { return time; } set { time = value; } }

    [SerializeField] private TextMeshPro timeText;
    public TextMeshPro TimeText { get => timeText; set => timeText = value; }
    
    [SerializeField] private int personalPoints;
    public int PersonalPoints { get { return personalPoints; } set { personalPoints = value; } }

    [SerializeField] private int teamPoints;
    public int TeamPoints { get { return teamPoints; } set { teamPoints = value; } }


    [SerializeField] private TextMeshPro personalPointsText;
    public TextMeshPro PersonalPointsText { get {  return personalPointsText; } set { personalPointsText = value; } }

    [SerializeField] private TextMeshPro teamPointsText;
    public TextMeshPro TeamPointsText { get { return teamPointsText; } set {  teamPointsText = value; } }

    [SerializeField] private TextMeshPro freeText;
    public TextMeshPro FreeText { get { return freeText; } set { freeText = value; } }

    [SerializeField] private bool played;
    public bool Played { get { return played; } set { played = value; } }

    [SerializeField] private int slotIndex;
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    void Start()
    {
        //events
        GameLogicScript.OnStateChange += CardsOnStateChange;

        //time
        time = CardTime.GetRandomTimeEnum();
        timeText.text = CardTime.EnumToString(time);

        //free text
        freeText.text = CardText.GeneralText[UnityEngine.Random.Range(0, CardText.GeneralText.Count)];



        //points (should add logic, and ints)
        personalPoints = UnityEngine.Random.Range(0, 15);
        teamPoints = UnityEngine.Random.Range(0, 15);
        personalPointsText.text = $"+{personalPoints}";
        teamPointsText.text = $"+{teamPoints}";

        //drag
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
        //Invoke("MoveToPlayedCardDeck", 2f);// Launches a MoveToPlayedCardDeck in 2 seconds - for testing needs to be moved
    }

    private void MoveToPlayedCardDeck()//not sure if it should be here
    {
        CardManager.InstanceCardManager.UsedCards.Add(this);
        CardSlotsManager.InstanceSlotManager.AvailableSlot[SlotIndex] = true;
        gameObject.SetActive(false);
    }
}
