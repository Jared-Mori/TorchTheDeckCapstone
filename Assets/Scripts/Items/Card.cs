using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public string description;
    public int uses;
    public bool isStackable = false;
    public int count = 0;

    public int Uses
    {
        get { return uses; }
    }

    public int rarity; // 0 = common, 1 = uncommon, 2 = rare, 3 = legendary

    public virtual void Effect(combatDetails playerDetails, combatDetails enemyDetails)
    {
        // This method will be overridden by subclasses
        // Used to provide specific functionality for each card
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
    }

    public override void Effect(combatDetails player, combatDetails enemy)
    {
        player.health += 5;
        if (player.health > player.healthMax)
        {
            player.health = player.healthMax;
        }
    }
}
