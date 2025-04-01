using UnityEngine;

[System.Serializable]
public class Door : Entity
{

    public Sprite openSprite, closedSprite;
    public bool isOpen = false;
    public override void SetDefaults()
    {
        entityType = EntityType.Door;
        facing = Direction.Down;
        artwork = Resources.Load<Sprite>("Sprites/Entities/Door");
    }
    public override void Interact()
    {
        Debug.Log("Interacting with door");
        // Open door if player has key.
    }
}
