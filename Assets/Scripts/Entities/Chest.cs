using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

[System.Serializable]
public class Chest : Entity
{
    public Sprite openSprite, closedSprite;
    public bool isOpen = false;
    public override void SetDefaults()
    {
        entityType = EntityType.Chest;
        closedSprite = levelManager.spriteManager.chestSprites[2];
        openSprite = levelManager.spriteManager.chestSprites[7];
        facing = Direction.Down;
        spriteRenderer.sprite = closedSprite; // Set the initial sprite to closed
    }
    public async Task GenerateLoot(int levelNumber)
    {
        List<Card> loot = new List<Card>();

        switch (levelNumber)
        {
            case 1:
                loot = LootGenerator.GenerateDrops(1, 1, 0, 0);
                break;
            case 2:
                loot = LootGenerator.GenerateDrops(2, 2, 1, 0);
                break;
            case 3:
                loot = LootGenerator.GenerateDrops(3, 2, 2, 1);
                break;
            case 4:
                loot = LootGenerator.GenerateDrops(4, 3, 2, 2);
                break;
            case 5:
                loot = LootGenerator.GenerateDrops(4, 3, 2, 3);
                break;
            default:
                loot = LootGenerator.GenerateDrops(1, 0, 0, 0);
                break;
        }
        await levelManager.LoadDeck(loot);
    }

    public override void Interact()
    {
        Debug.Log($"Interacting with chest at {gridPosition}");
        if (isOpen)
        {
            Debug.Log("Chest is already open");
        } else
        {
            Debug.Log("Opening the chest");
            isOpen = true;
            spriteRenderer.sprite = openSprite; // Change to open sprite
            StatTracker.IncrementChestsOpened(); // Increment the chests opened
            _ = GenerateLoot(levelManager.level.levelNumber);
        }
    }
}
