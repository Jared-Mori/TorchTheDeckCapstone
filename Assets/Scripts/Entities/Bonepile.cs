using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Bonepile : Entity
{
    public List<Card> loot = new List<Card>();
    public override void SetDefaults()
    {
        entityType = EntityType.Bonepile;
        facing = Direction.Down;
    }

    public void TutorialPile()
    {
        loot.Add(new IronSword());
        loot.Add(new IronShield());
        loot.Add(new HealthPotion());
    }

    public override void Interact()
    {
        Debug.Log($"Interacting with bonepile at {gridPosition}");
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        levelManager.LoadDeck(loot);
        StatTracker.IncrementBonePilesCollected(); // Increment the bonepiles opened
        levelManager.entities.Remove(this); // Remove the bonepile from the level
        Destroy(gameObject); // Destroy the bonepile object
        string path = Application.dataPath + "/bonepile.json";
        if (File.Exists(path)) { File.Delete(path); }
    }
}
