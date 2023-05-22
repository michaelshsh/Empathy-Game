using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardScript : MonoBehaviour
{
    // Start is called before the first frame update 

    public bool drag;
    private bool draggable;
    public SpriteRenderer mySpritRenderer;
    public CardTime.TimeEnum time;
    public TextMeshPro TimeText;
    public int PersonalPoints;
    public int TeamPoints;
    public TextMeshPro PersonalPointsText;
    public TextMeshPro TeamPointsText;
    public TextMeshPro FreeText;
    public bool Played;
    public int SlotIndex;
    public SlotScript slotOnSchedule = null;
    // card boundries
    private Camera MainCamera;
    private float CardWidth;
    private float CardHeight;

    void Start()
    {
        //events
        GameLogicScript.OnStateChange += CardsOnStateChange;

        //time
        time = CardTime.GetRandomTimeEnum();
        TimeText.text = CardTime.EnumToString(time);

        //free text
        FreeText.text = CardText.GeneralText[UnityEngine.Random.Range(0, CardText.GeneralText.Count)];



        //points (should add logic, and ints)
        PersonalPoints = UnityEngine.Random.Range(0, 15);
        TeamPoints = UnityEngine.Random.Range(0, 15);
        PersonalPointsText.text = $"+{PersonalPoints}";
        TeamPointsText.text = $"+{TeamPoints}";

        //drag
        draggable = true;


        //card boundries
        MainCamera = Camera.main;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        CardWidth = spriteRenderer.bounds.size.x / 2f;
        CardHeight = spriteRenderer.bounds.size.y / 2f;
    }
    void LateUpdate()
    {
        var cameraHalfWidth = MainCamera.orthographicSize * ((float)Screen.width / Screen.height);
        var cameraHalfHeight = MainCamera.orthographicSize;

        var xMax = cameraHalfWidth - CardWidth;
        var xMin = -xMax;
        var yMax = cameraHalfHeight - CardHeight;
        var yMin = -yMax;

        var position = transform.position;
        position.x = Mathf.Clamp(position.x, xMin, xMax);
        position.y = Mathf.Clamp(position.y, yMin, yMax);
        transform.position = position;
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
        CardSlotsManager.InstanceSlotManager.availableSlot[SlotIndex] = true;
        gameObject.SetActive(false);
    }
}
