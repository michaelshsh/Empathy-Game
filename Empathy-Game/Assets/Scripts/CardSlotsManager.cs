using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardSlotsManager : MonoBehaviour
{
    public static CardSlotsManager InstanceSlotManager { get; private set; }
    [SerializeField] private Transform[] slots;
    public Transform[] Slots { get { return slots; } set { slots = value; } }

    [SerializeField] private bool[] availableSlot;
    public bool[] AvailableSlot { get { return availableSlot; } set { availableSlot = value; } }

    private void Awake()
    {
        if (InstanceSlotManager != null && InstanceSlotManager != this)
        {
            Destroy(this);
        }
        else
        {
            InstanceSlotManager = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InstanceSlotManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard()
    {
        if (CardManager.InstanceCardManager.Deck.Count >= 1)
        {
            CardScript card = CardManager.InstanceCardManager.Deck[UnityEngine.Random.Range(0, CardManager.InstanceCardManager.Deck.Count)];

            for (int i = 0; i < availableSlot.Length; i++)
            {
                if (availableSlot[i] == true)
                {
                    card.gameObject.SetActive(true);
                    card.transform.position = Slots[i].position;
                    card.SlotIndex = i;
                    card.Played = false;
                    availableSlot[i] = false;
                    CardManager.InstanceCardManager.Deck.Remove(card);
                    return;
                }
            }
        }
    }

}
