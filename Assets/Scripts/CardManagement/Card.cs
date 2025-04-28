using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

public static class Rarity
{
    public const int Common = 0;
    public const int Uncommon = 1;
    public const int Rare = 2;
    public const int Legendary = 3;
}
public enum ItemType { Helmet, Chestpiece, Boots, Shield, Accessory, Weapon, Bow, Arrow, Item, Default }
[System.Serializable]
[JsonConverter(typeof(CardConverter))]
public class Card
{
    public string cardName = "Card";
    public string description = "Description";
    public string tooltip = "Tooltip";
    public int uses = 1;
    public bool isStackable = false;
    public int count = 1;
    public int rarity = Rarity.Common; // 0 = common, 1 = uncommon, 2 = rare, 3 = legendary
    public ItemType itemType = ItemType.Item; // Default to Item type

    public virtual Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Effect method called on card");
        // This method will be overridden by subclasses
        // Used to provide specific functionality for each card
        return Task.CompletedTask;
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

    public async Task AddToDeck()
    {
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        await pileController.AddCard(this);
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

    public void AnimateDamageEffect(int damage, CombatDetails target)
    {
        Debug.Log("Triggering animation for damage: " + damage.ToString());
        AnimationController animationController = GameObject.Find("AnimationController")?.GetComponent<AnimationController>();

        if (target.entityType == EntityType.Player)
        { animationController.TriggerAnimation(damage.ToString(), "Damage", true); }
        else
        { animationController.TriggerAnimation(damage.ToString(), "Damage", false); }
    }
}

public class DefaultCard : Card
{
    public DefaultCard()
    {
        cardName = "DEEFAULT CARD";
        description = " ";
        tooltip = " ";
        itemType = ItemType.Default;
        uses = 1;
        rarity = Rarity.Common;
    }
}

