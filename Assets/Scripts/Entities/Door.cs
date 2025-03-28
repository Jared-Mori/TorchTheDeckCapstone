using UnityEngine;

[System.Serializable]
public class Door : Entity
{
    public override void SetDefaults()
    {
        entityName = "Door";
        facing = Direction.Down;
        artwork = Resources.Load<Sprite>("Sprites/Entities/Door");
    }
    public override void Interact()
    {
        Debug.Log("Interacting with door");
        // Open door if player has key.
    }
}
