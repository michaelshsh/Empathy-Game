using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class CardSlotsManager : MonoBehaviour
{
    public static CardSlotsManager InstanceSlotManager { get; private set; }
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
    public Transform[] Slots; // should be moved to class CardManager or create new class - Slot manager
    public bool[] availableSlot; // should be moved to class CardManager or create new class - Slot manager
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
        /*********delete if there are no problems*****/
        //if (CardManager.InstanceCardManager.deck.Count >= 1)
        //{
        //    CardScript card = CardManager.InstanceCardManager.deck[UnityEngine.Random.Range(0, CardManager.InstanceCardManager.deck.Count)];

        //    for (int i = 0; i < availableSlot.Length; i++)
        //    {
        //        if (availableSlot[i] == true)
        //        {
        //            card.gameObject.SetActive(true);
        //            card.transform.position = Slots[i].position;
        //            card.SlotIndex = i;
        //            card.Played = false;
        //            availableSlot[i] = false;
        //            CardManager.InstanceCardManager.deck.Remove(card);
        //            CardManager.InstanceCardManager.CardsInGame.Add(card);
        //            return;
        //        }
        //    }
        //}
        /**********************************************/

        for (int i = 0; i < availableSlot.Length; i++)//Dynamically drawing a card
        {
            if (availableSlot[i] == true)
            {
                CardScript newCard = Instantiate(CardManager.InstanceCardManager.CardPrefab, Slots[i].position, Quaternion.identity).GetComponent<CardScript>();
                CardManager.InstanceCardManager.InitOrCreateACoopCard(newCard);

                Debug.Log($"New {((newCard.isCoopCard)?("Co-op"):("Solo"))} card was created in slot " + i);
                availableSlot[i] = false;
                newCard.transform.position = Slots[i].position;
                newCard.SlotIndex = i;
                newCard.Played = false;
                CardManager.InstanceCardManager.CardsInGame.Add(newCard);
                break;
            }
        }
    }

}
