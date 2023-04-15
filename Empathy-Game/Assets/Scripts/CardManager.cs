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
    public List<CardScript> deck = new List<CardScript>(); //should be moved to class CardManager
    // Start is called before the first frame update
    void Start()
    {
        InstanceCardManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
