using UnityEngine;

[System.Serializable]
public class Door : Entity
{
    [SerializeField] private int toLevel;
    public override void SetDefaults()
    {
        entityType = EntityType.Door;
        facing = Direction.Down;
    }
    public override void Interact()
    {
        Debug.Log($"Traveling to floor {toLevel} with door");
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        levelManager.InitializeDefaultLevel(toLevel); // Load the next level.
        // Open door if player has key.
    }
}
