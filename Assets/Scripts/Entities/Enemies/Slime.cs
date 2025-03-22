using UnityEngine;
using System.Collections.Generic;

public class Slime : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        viewDistance = 1;
        maxHealth = 5;
        health = maxHealth;
        artwork = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = artwork;

        // temporary path for testing
        SetPosition(new Vector3Int(9, 9, 0));
        path = new List<Direction> { Direction.Up, Direction.Up, 
                                     Direction.Left, Direction.Left, 
                                     Direction.Down, Direction.Down, 
                                     Direction.Right,Direction.Right };
    }
}
