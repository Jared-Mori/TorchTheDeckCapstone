using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType { Helmet, Chestpiece, Boots, Shield, Accessory, Weapon, Bow, Item }
[System.Serializable]
public class Card
{
    public string cardName;
    public string description;
    public int uses;
    public bool isStackable = false;
    public int count = 1;
    public ItemType itemType;

    public int Uses
    {
        get { return uses; }
    }

    public int rarity; // 0 = common, 1 = uncommon, 2 = rare, 3 = legendary

    public virtual void Effect(CombatDetails user, CombatDetails target)
    {
        // This method will be overridden by subclasses
        // Used to provide specific functionality for each card
    }

    public virtual void Test()
    {
        Debug.Log("I am a card!");
    }

    public bool Use(){
        if (uses > 0)
        {
            uses--;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddToDeck(List<Card> deck)
    {
        if (deck == null)
        {
            deck = new List<Card>();
        }

        if (deck.Contains(this) && isStackable)
        {
            count++;
        }
        else
        {
            deck.Add(this);
        }
    }

    public void RemoveCard(List<Card> deck)
    {
        if (deck.Contains(this) && isStackable)
        {
            count--;
            if (count <= 0)
            {
                deck.Remove(this);
            }
        }
        else
        {
            deck.Remove(this);
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
        // Logic for attacking with the sword
        Debug.Log("Attacking with sword");
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
        // item sprite 89
        itemType = ItemType.Shield;
        uses = 3;
        rarity = 1;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
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
        // item sprite 90
        itemType = ItemType.Shield;
        uses = 6;
        rarity = 2;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        user.isShielded = true;
    }
}