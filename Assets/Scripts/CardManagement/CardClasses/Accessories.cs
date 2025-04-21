using UnityEngine;

/// <summary>
/// Accessory classes
/// </summary>
public interface Accessory
{
    void AccessoryEffect(CombatDetails user, CombatDetails target);
}

[System.Serializable]
public class RingOfTheForge : Card, Accessory
{
    public RingOfTheForge()
    {
        cardName = "Ring of the Forge";
        description = "A geometric ring of dark metal. A blacksmith's joy.";
        tooltip = "Grants equipped weapons, bows, and armor +1 durability.";
        uses = 10;
        rarity = Rarity.Rare;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in user.deck)
        {
            if (card is Weapon || card is Bow || card is Armor)
            {
                card.uses += 1;
            }
        }
    }
}

[System.Serializable]
public class QuiverTalisman : Card, Accessory
{
    public QuiverTalisman()
    {
        cardName = "Quiver Talisman";
        description = "A talisman in the shape of a quiver. An archer's best friend.";
        tooltip = "Grants a Steel Arrow to the user.";
        uses = 10;
        rarity = Rarity.Uncommon;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        pc.AddCard(new SteelArrow());
    }
}

[System.Serializable]
public class EnergyTalisman : Card, Accessory
{
    public EnergyTalisman()
    {
        cardName = "Energy Talisman";
        description = "A talisman that glows with a faint light. An Adventurer's reprieve.";
        tooltip = "Grants 1 energy to the user.";
        uses = 10;
        rarity = Rarity.Common;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        user.energy += 1;
    }
}

[System.Serializable]
public class ScorpionMedalion : Card, Accessory, IStatusEffect
{
    public Status status {get; set; } = new Poison();
    public ScorpionMedalion()
    {
        cardName = "Scorpion Medalion";
        description = "A medalion in the shape of a scorpion. The sand's sting.";
        tooltip = "Adds a random status effect to the enemy. Burn, Poison, or Paralysis.";
        uses = 10;
        rarity = Rarity.Rare;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        int randStatus = Random.Range(0, 3);
        if (randStatus == 0)      { status = new Burn();}
        else if (randStatus == 1) { status = new Poison(); }
        else if (randStatus == 2) { status = new Paralysis(); }
        target.statusEffects.Add(status);
    }
}

[System.Serializable]
public class DragonKingsScale : Card, Accessory
{
    public DragonKingsScale()
    {
        cardName = "Dragon King's Scale";
        description = "A scale from the Dragon King. A true warrior's pride.";
        tooltip = "Grants +1 damage and durability to all equipped weapons.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in user.deck)
        {
            if (card is Weapon)
            {
                card.uses += 1;
                Weapon weapon = (Weapon)card;
                weapon.damage += 1;
            }
        }
    }
}

[System.Serializable]
public class PhilosophersStone : Card, Accessory
{
    public PhilosophersStone()
    {
        cardName = "Philosopher's Stone";
        description = "A Stone of great power. An alchemist's magnum opus.";
        tooltip = "Upgrades all healing potions to greater potions, greater potions to super potions, and super potions to vitality draughts.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card is HealthPotion)
            {
                int index = pc.hand.IndexOf(card);
                pc.RemoveCard(index);
                pc.AddCard(new GreatHealthPotion());
            }
            else if (card is GreatHealthPotion)
            {
                int index = pc.hand.IndexOf(card);
                pc.RemoveCard(index);
                pc.AddCard(new SuperHealthPotion());
            }
            else if (card is SuperHealthPotion)
            {
                int index = pc.hand.IndexOf(card);
                pc.RemoveCard(index);
                pc.AddCard(new VitalityDraught());
            }
        }
    }
}

[System.Serializable]
public class FangsOfTheVampire : Card, Accessory
{
    public FangsOfTheVampire()
    {
        cardName = "Fangs of the Vampire";
        description = "A pair of fangs from a vampire. The bloodletter's sustenance.";
        tooltip = "Grants the user a Vampiric Bite card.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        pc.AddCard(new VampiricBite());
    }
}

[System.Serializable]
public class WerewolfsGlare : Card, Accessory
{
    public WerewolfsGlare()
    {
        cardName = "Werewolf's Glare";
        description = "A pendant in the shape of a wolf's head. The beast's ferocity.";
        tooltip = "Grants the user a Howl card.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        pc.AddCard(new Howl());
    }
}
