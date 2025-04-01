using UnityEngine;
using System.Collections.Generic;

public class Slime : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Slime;
        viewDistance = 1;
        maxHealth = 5;
        artwork = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = artwork;

        if (!isLoaded) 
        {
            health = maxHealth;
            SetPosition(new Vector3Int(9, 9, 0));
        } else
        {
            SetPosition(loadPosition);
        }

        // temporary path for testing
        path = new List<Direction> { Direction.Up, Direction.Up, 
                                     Direction.Left, Direction.Left, 
                                     Direction.Down, Direction.Down, 
                                     Direction.Right,Direction.Right };
    }
}
