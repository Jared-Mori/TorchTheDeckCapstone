using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum ItemType { Helmet, Chestpiece, Boots, Shield, Accessory, Weapon, Bow, Arrow, Item }
[System.Serializable]
[JsonConverter(typeof(CardConverter))]
public class Card
{
    public string cardName = "Card";
    public string description = "Description";
    public int uses = 1;
    public bool isStackable = false;
    public int count = 1;
    public int rarity = 0; // 0 = common, 1 = uncommon, 2 = rare, 3 = legendary
    public ItemType itemType = ItemType.Item; // Default to Item type

    public virtual void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Effect method called on card");
        // This method will be overridden by subclasses
        // Used to provide specific functionality for each card
    }

    public virtual void Test()
    {
        Debug.Log("I am a card!");
    }

    public virtual bool Use(){
        uses--;
        if (uses <= 0)
        {
            return true; // Card is used up
        }
        return false; // Card still has uses left
    }

    public void AddToDeck()
    {
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        pileController.AddCard(this);
    }

    public void RemoveCard(List<Card> deck)
    {
        if (deck.Contains(this))
        {
            deck.Remove(this);
        }
        else
        {
            Debug.Log("Card not found in deck.");
        }
    }
}

