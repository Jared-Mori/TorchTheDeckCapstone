using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int uses;
    public bool isStackable = false;
    public int count = 0;
    public Entity owner;
    protected InventoryManager inventoryManager;
    public LevelManager levelManager;

    public int Uses
    {
        get { return uses; }
    }

    public int rarity; // 0 = common, 1 = uncommon, 2 = rare, 3 = legendary

    public virtual void Use()
    {
        uses--;
        if (uses <= 0)
        {
            RemoveCard(owner.deck);
        }
    }

    public virtual void Effect()
    {
        // This method will be overridden by subclasses
        // Used to provide specific functionality for each card
    }

    public void AddToDeck(List<Card> deck, Entity newOwner)
    {
        if (deck.Contains(this) && isStackable)
        {
            count++;
        }
        else
        {
            owner = newOwner;
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

    public void SetLevelManager()
    {
        this.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
}

[System.Serializable]
public class HealthPotion : Card
{
    public HealthPotion()
    {
        cardName = "Health Potion";
        description = "A potion that restores 5 health.";
        SetLevelManager();
        artwork = levelManager.spriteManager.itemSprites[134];
        uses = 1;
        rarity = 1;
        isStackable = true;
    }

    public override void Effect()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.health += 5;
        Use();
    }
}
