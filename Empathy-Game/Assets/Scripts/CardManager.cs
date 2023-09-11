using Constants;
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
    public GameObject CardPrefab;

    public int ChanceToDrawCoopCard = 30;   
    public int ChanceToDrawSoloCard = 70;
    private NonRepeatingTextGetter textGetter = new();

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

    public void OnDrawClick()
    {
        CardSlotsManager.InstanceSlotManager.DrawCard();
    }

    public CardScript InitOrCreateACoopCard(CardScript optinalCardToinit = null)
    {
        var coopCard = optinalCardToinit ?? new CardScript();
        InitOrCreateASoloCard(coopCard);

        List<PlayerLabels.LabelEnum> playerLabels = new();
        var players = FindObjectsOfType<PlayerScript>();
        
        foreach(var player in players)
        {
            if(!player.IsOwner) //dont add yourself
            {
                playerLabels.Add(player.mylabel.Value);
            }
        }

        if(playerLabels.Count != 0) //if no labels, fallback to solo card
        {
            var randomLabel = playerLabels[Random.Range(0, playerLabels.Count)];
            var randomText = textGetter.NonReapetingTextByLabel(randomLabel);

            //coop only revelent fields
            coopCard.isCoopCard = true;
            coopCard.requiredLabel = randomLabel;
            coopCard.FreeText.text = $"|REQUIRES:{PlayerLabels.EnumToString(randomLabel)}|\n {randomText}";

            coopCard.PersonalPoints /= 2; //less personal
            coopCard.TeamPoints *= 2; // more team points

            setupCardUI(coopCard);
        }

        return coopCard;
    }

    public CardScript InitOrCreateASoloCard(CardScript optinalCardToinit = null)
    {
        CardScript card = optinalCardToinit ?? new CardScript();
        card.FreeText.text = textGetter.NonReapetingTextGeneral();

        //time
        card.time = CardTime.GetRandomTimeEnum();

        //points (should add logic, and ints)
        card.PersonalPoints = UnityEngine.Random.Range(1, 15);
        card.TeamPoints = UnityEngine.Random.Range(1, 15);

        setupCardUI(card);

        return card;
    }

    private void setupCardUI(CardScript card)
    {
        //should set up coopCard.FreeText.text on the calling method!
        card.TimeText.text = CardTime.EnumToString(card.time);
        card.PersonalPointsText.text = $"+{card.PersonalPoints}";
        card.TeamPointsText.text = $"+{card.TeamPoints}";
    }

    public CardScript InitARandomCard(CardScript card)
    {
        CardScript newCard = card ?? new CardScript();

        int totalChances = ChanceToDrawCoopCard + ChanceToDrawSoloCard;
        int rndCardChange = Random.Range(0, totalChances);

        if(rndCardChange < ChanceToDrawCoopCard)
        {
            InitOrCreateACoopCard(newCard);
        }
        else
        {
            InitOrCreateASoloCard(newCard);
        }

        return newCard;
    }
}
