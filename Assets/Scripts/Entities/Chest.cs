using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class Chest : Entity
{
    Sprite openSprite, closedSprite;
    bool isOpen = false;
    public override void SetDefaults()
    {
        entityName = "Chest";
        facing = Direction.Down;
        closedSprite = levelManager.spriteManager.chestSprites[2];
        openSprite = levelManager.spriteManager.chestSprites[7];
        spriteRenderer.sprite = closedSprite;
        SetPosition(new Vector3Int(5, 5, 0));
    }
    public void GenerateLoot()
    {
        // Generate loot and add directly to players inventory.
        Debug.Log("Generating Loot");
        Stone stone = new Stone();
        stone.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added stone to player deck: " + levelManager.playerInstance.deck);

        HealthPotion healthPotion = new HealthPotion();
        healthPotion.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added health potion to player deck: " + levelManager.playerInstance.deck);

        Shield shield = new Shield();
        shield.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added shield to player deck: " + levelManager.playerInstance.deck);

        IronShield ironShield = new IronShield();
        ironShield.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added iron shield to player deck: " + levelManager.playerInstance.deck);
    }

    public void DisplayLoot()
    {
        Debug.Log("Displaying Loot");
        // Display loot to the player.
    }

    public override void Interact()
    {
        if (isOpen)
        {
            Debug.Log("Chest is already open");
        } else
        {
            Debug.Log("Opening the chest");
            spriteRenderer.sprite = openSprite;
            isOpen = true;
            GenerateLoot();
            DisplayLoot();
        }
    }
}
