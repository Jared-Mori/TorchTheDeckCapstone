using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class Chest : Entity
{
    Sprite openSprite, closedSprite;
    public bool isOpen = false;
    public override void SetDefaults()
    {
        entityType = EntityType.Chest;
        closedSprite = levelManager.spriteManager.chestSprites[2];
        openSprite = levelManager.spriteManager.chestSprites[7];


        if (!isLoaded) 
        {
            facing = Direction.Down;
            SetPosition(new Vector3Int(5, 5, 0));
        } else
        {
            SetPosition(loadPosition);
        }
    }
    public void GenerateLoot()
    {
        List<Card> loot = new List<Card>();
        loot = LootGenerator.GenerateDrops(4, 4, 4, 4);
        levelManager.LoadDeck(loot);
    }

    public override void Interact()
    {
        if (isOpen)
        {
            Debug.Log("Chest is already open");
        } else
        {
            Debug.Log("Opening the chest");
            isOpen = true;
            GenerateLoot();
        }
    }
}
