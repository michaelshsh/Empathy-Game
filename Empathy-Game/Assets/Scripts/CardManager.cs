using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CardManager : MonoBehaviour
{
    public static CardManager InstanceCardManager { get; private set; }
    [SerializeField] private List<CardScript> deck = new List<CardScript>();
    public List<CardScript> Deck { get { return deck; } set { deck = value; } }

    [SerializeField] private List<CardScript> usedCards = new List<CardScript>();
    public List<CardScript> UsedCards { get { return usedCards; } set { usedCards = value; } }
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
    // Start is called before the first frame update
    void Start()
    {
        InstanceCardManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnUsedCardsToDeck()
    {
        if(usedCards.Count >= 1)
        {
            foreach(CardScript card in usedCards)
            {
                deck.Add(card);
            }
            usedCards.Clear();
        }
    }
}
