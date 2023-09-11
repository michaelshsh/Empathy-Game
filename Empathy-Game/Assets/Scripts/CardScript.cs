using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
    //public SlotScript slotOnSchedule = null;
    public GameObject CardObj;
    public PlayerLabels.LabelEnum requiredLabel;
    public bool isCoopCard = false;
    // card boundries
    private Camera MainCamera;
    private float CardWidth;
    private float CardHeight;
    private float CardReturnSpeed;

    public bool InsertedToASlot { get; set; }

    void Start()
    {
        //events
        GameLogicScript.Instance.CurrentGameState.OnValueChanged += CardsOnStateChange;

        //drag
        CardsOnStateChange(GameLogicScript.Instance.CurrentGameState.Value, GameLogicScript.Instance.CurrentGameState.Value);

        //card boundries
        MainCamera = Camera.main;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        CardWidth = spriteRenderer.bounds.size.x / 2f;
        CardHeight = spriteRenderer.bounds.size.y / 2f;

        //card return speed
        CardReturnSpeed = 9f;
    }

    private void CardsOnStateChange(GameState previousValue, GameState newValue)
    {
        if (newValue == GameState.RoundStart)
        {
            draggable = true;
        }
        else
            draggable = false;
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

    // Update is called once per frame
    void Update()
    {
        if (drag && draggable)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            worldPosition.z = -0.5f;
            gameObject.transform.position = (gameObject.transform.position + worldPosition) / 2;            
        }
        else
        {
           gameObject.transform.position = Vector3.MoveTowards(transform.position, CardSlotsManager.InstanceSlotManager.Slots[SlotIndex].position, CardReturnSpeed*Time.deltaTime);
        }
        if (GameLogicScript.Instance.CurrentGameState.Value == GameState.RoundEnd)
        {
            MoveToPlayedCardDeck();
        }    

    }

    public void EnableCollider()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    private void OnDestroy()
    {
        GameLogicScript.Instance.CurrentGameState.OnValueChanged -= CardsOnStateChange;
    }

    private void OnMouseDown()
    {
        drag = true;
        gameObject.transform.position += new Vector3(0, 0, 1);
    }

    private void OnMouseUp()
    {
        drag = false;
        gameObject.transform.position += new Vector3(0, 0, -1);
    }

    private void MoveToPlayedCardDeck()//not sure if it should be here
    {
        CardManager.InstanceCardManager.UsedCards.Add(this);
        CardSlotsManager.InstanceSlotManager.availableSlot[SlotIndex] = true;
        gameObject.SetActive(false);
    }

    public void makeCardIgnoreOtherCards()
    {
        Debug.Log(CardManager.InstanceCardManager.CardsInGame); // testing - to delete later
        Debug.Log(CardManager.InstanceCardManager.CardsInGame.Count);
        for (int i = 0; i < CardManager.InstanceCardManager.CardsInGame.Count; i++)
        {
            Debug.Log(CardManager.InstanceCardManager.CardsInGame[i]);
        }

            for (int i = 0; i < CardManager.InstanceCardManager.CardsInGame.Count; i++)
        {
            Debug.Log(CardManager.InstanceCardManager.CardsInGame[i]);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), CardManager.InstanceCardManager.CardsInGame[i].GetComponent<Collider2D>());
        }
    }
}
