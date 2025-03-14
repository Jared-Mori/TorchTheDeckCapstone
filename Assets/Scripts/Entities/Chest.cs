using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class Chest : Entity
{
    Sprite[] sprites;
    Sprite openSprite, closedSprite;
    bool isOpen = false;
    public override void SetDefaults()
    {
        entityName = "Chest";
        sprites = Resources.LoadAll<Sprite>("Sprites/Entities/Treasure chests");
        closedSprite = sprites[2];
        openSprite = sprites[7];
        spriteRenderer.sprite = closedSprite;
        SetPosition(new Vector3Int(5, 5, 0));
    }
    public void GenerateLoot()
    {
        Debug.Log("Generating Loot");
        Stone stone = new Stone();
        stone.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added stone to player deck: " + levelManager.playerInstance.deck);
        // Generate loot and add directly to players inventory.

        HealthPotion healthPotion = new HealthPotion();
        healthPotion.AddToDeck(levelManager.playerInstance.deck, levelManager.playerInstance);
        Debug.Log("Added health potion to player deck: " + levelManager.playerInstance.deck);
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
