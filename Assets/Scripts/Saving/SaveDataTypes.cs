using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class EntityData
{
    public EntityType entityType;
    public int xPos, yPos;
    public Direction facing;
    public bool isAttacker;
    public bool isOpenedChest = false;
    public int health, maxHealth;
    public int energy, maxEnergy;
}

[System.Serializable]
public class SaveData
{
    public int level;
    public EntityData[] entityDataArray;
    public List<Card> deck = new List<Card>();
    public Card[] gear = new Card[7]; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory, 5: Weapon, 6: Bow
}

[System.Serializable]
public class BonePile
{
    public int level;
    public int xPos, yPos;
    public List<Card> deck = new List<Card>();

    public BonePile(int level, int xPos, int yPos, List<Card> deck, Card[] gear)
    {
        this.level = level;
        this.xPos = xPos;
        this.yPos = yPos;
        this.deck = deck;

        foreach (Card card in gear)
        {
            if (card != null && card is not TempCard)
            {
                this.deck.Add(card);
            }
        }
    }
}