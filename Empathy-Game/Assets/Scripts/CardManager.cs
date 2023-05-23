using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardManager : MonoBehaviour
{
    public static CardManager InstanceCardManager { get; private set; }
    private void Awake()
    {
        if (InstanceCardManager != null && InstanceCardManager != this)
        {
            Destroy(this);
        }
        else
        {
            InstanceCardManager = this;
        }
    }
    public List<CardScript> deck = new List<CardScript>();
    public List<CardScript> CardsInGame = new List<CardScript>();
    public List<CardScript> UsedCards = new List<CardScript>();
    // Start is called before the first frame update
    void Start()
    {
        InstanceCardManager = this;
        makeAllCardsIgnoreEachOther();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnUsedCardsToDeck()
    {
        if(UsedCards.Count >= 1)
        {
            foreach(CardScript card in UsedCards)
            {
                deck.Add(card);
            }
            UsedCards.Clear();
        }
    }
    public void makeAllCardsIgnoreEachOther()
    {   // will ignore all collisions between cards
        for (int i = 0; i < deck.Count; i++)
        {
            for (int j = i + 1; j < deck.Count; j++)
            {
                Physics2D.IgnoreCollision(deck[i].GetComponent<Collider2D>(), deck[j].GetComponent<Collider2D>());
            }
        }
    }
}
