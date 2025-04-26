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
        spriteIndex = Random.Range(0, 6); // Randomly select a sprite index between 0 and 2
        sprite = levelManager.spriteManager.rockSprites[spriteIndex]; // Get the sprite from the sprite manager
        spriteRenderer.sprite = sprite;
    }

    public override void Interact()
    {
        Debug.Log("Interacting with rock");
        StatTracker.IncrementRocksCollected(); // Increment the rocks collected
        if (spriteIndex == 6){
            for (int i = 0; i < 3; i++){
                Stone stone = new Stone();
                stone.AddToDeck();
            }
        } else {
            Stone stone = new Stone();
            stone.AddToDeck();
        }
        levelManager.entities.Remove(this); // Remove the rock from the level
        Destroy(gameObject); // Destroy the rock game object
    }
}
