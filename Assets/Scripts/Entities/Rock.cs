using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Rock : Entity
{
    int spriteIndex; // Index of the sprite in the sprite sheet
    public Sprite sprite;

    public override void SetDefaults()
    {
        entityType = EntityType.Rock;
        facing = Direction.Down;
        spriteIndex = Random.Range(0, 6); // Randomly select a sprite index between 0 and 5
        sprite = levelManager.spriteManager.rockSprites[spriteIndex]; // Get the sprite from the sprite manager
        spriteRenderer.sprite = sprite;
    }

    // Wrapper method for Interact
    public override void Interact()
    {
        // Call the async Interact method without awaiting it
        _ = InteractAsync();
    }

    public async Task InteractAsync()
    {
        Debug.Log("Interacting with rock");
        StatTracker.IncrementRocksCollected(); // Increment the rocks collected

        if (spriteIndex == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                await AddStoneToDeck(); // Add a stone to the player's deck
            }
        }
        else
        {
            await AddStoneToDeck(); // Add a stone to the player's deck
        }

        levelManager.entities.Remove(this); // Remove the rock from the level
        Destroy(gameObject); // Destroy the rock game object
    }

    public async Task AddStoneToDeck()
    {
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        foreach (Card card in pileController.hand)
        {
            if (card is Stone)
            {
                Debug.Log("Found existing stone card in hand, increasing count.");
                card.count++;
                return;
            }
        }
        Debug.Log("Adding new stone card to deck.");
        Stone stone = new Stone();
        await pileController.AddCard(stone); // Add the stone card to the player's deck
        await pileController.UpdateHandAsync(); // Update the deck UI
    }
}
