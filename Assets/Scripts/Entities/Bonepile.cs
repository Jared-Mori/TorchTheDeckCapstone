using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bonepile : Entity
{
    public List<Card> loot = new List<Card>();
    public override void SetDefaults()
    {
        entityType = EntityType.Bonepile;
        facing = Direction.Down;
    }

    public override void Interact()
    {
        Debug.Log($"Interacting with bonepile at {gridPosition}");
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        levelManager.LoadDeck(loot);
    }
}
