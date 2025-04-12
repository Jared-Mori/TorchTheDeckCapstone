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

    public bool Use(){
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

[System.Serializable]
public class HealthPotion : Card
{
    public HealthPotion()
    {
        cardName = "Health Potion";
        description = "A potion that restores 5 health.";
        uses = 1;
        rarity = 1;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Heal(user, 5);
    }
}

[System.Serializable]
public class Stone : Card
{
    public Stone()
    {
        cardName = "Stone";
        description = "A hefty stone. You could probably throw it.";
        uses = 1;
        rarity = 0;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, 1);
    }
}

[System.Serializable]
public class IronSword : Card
{
    public IronSword()
    {
        cardName = "Iron Sword";
        description = "A rusted sword. It's still sharp, but probably won't hold out for long.";
        uses = 4;
        rarity = 1;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
    }
}

[System.Serializable]
public class IronHelmet : Card
{
    public IronHelmet()
    {
        cardName = "Iron Helmet";
        description = "A sturdy helmet. It's seen better days, but it'll protect your head.";
        this.itemType = ItemType.Helmet;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class IronChestpiece : Card
{
    public IronChestpiece()
    {
        cardName = "Iron Chestpiece";
        description = "A heavy chestpiece. It's a bit rusty, but it'll protect your torso.";
        itemType = ItemType.Chestpiece;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class IronBoots : Card
{
    public IronBoots()
    {
        cardName = "Iron Boots";
        description = "A pair of armored boots. They're a bit scuffed, but they'll protect your feet.";
        itemType = ItemType.Boots;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class Shield : Card
{
    public Shield()
    {
        cardName = "Shield";
        description = "A wooden shield. It's cracked in places, but it'll protect you from attacks.";
        itemType = ItemType.Shield;
        uses = 3;
        rarity = 1;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        user.isShielded = true;
    }
}

[System.Serializable]
public class IronShield : Card
{
    public IronShield()
    {
        cardName = "Reinforced Shield";
        description = "A metal reinforced shield. It's heavy, but still holds together.";
        itemType = ItemType.Shield;
        uses = 6;
        rarity = 2;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        user.isShielded = true;
    }
}

[System.Serializable]
public class Bow : Card
{
    public Bow()
    {
        cardName = "Short Bow";
        description = "A simple bow. It's not very powerful, but it's better than nothing.";
        itemType = ItemType.Bow;
        uses = 3;
        rarity = 1;
    }

    public void BowEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This bow does nothing extra!");
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Arrow arrow)
        {
            arrow.ArrowEffect(user, target);
            BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
    }
}

[System.Serializable]
public class Arrow : Card
{
    public Arrow()
    {
        cardName = "Arrow";
        description = "A simple arrow. It's not very powerful, but it's better than nothing.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = 0;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.TakeDamage(target, 1);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            ArrowEffect(user, target);
            bow.BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
        }
    }
}