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
        
        if (isOpen)
        {
            spriteRenderer.sprite = openSprite;
        } else
        {
            spriteRenderer.sprite = closedSprite;
        }

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
        // Generate loot and add directly to players inventory.
        Debug.Log("Generating Loot");
        Stone stone = new Stone();
        stone.AddToDeck();

        HealthPotion healthPotion = new HealthPotion();
        healthPotion.AddToDeck();

        Shield shield = new Shield();
        shield.AddToDeck();

        IronShield ironShield = new IronShield();
        ironShield.AddToDeck();
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
