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
        StatTracker.IncrementFloorsCleared(); // Increment the floors cleared
        if (toLevel == 6){
            if (StatTracker.IsNecromancerDefeated() == true) {
                StatTracker.IncrementRunsCompleted(); // Increment the runs completed
                Menues.VictoryScreen(); // Go to the victory screen
            }
        }
        else {
            levelManager.GoToNextLevel(toLevel); // Go to the next level
        }
        // Open door if player has key.
    }
}
