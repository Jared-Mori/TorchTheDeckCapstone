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
        maxEnergy = 1;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;

        if (!isLoaded) 
        {
            health = maxHealth;
            SetPosition(new Vector3Int(0, 0, 0));
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

public class Goblin : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Goblin;
        viewDistance = 3;
        maxHealth = 7;
        maxEnergy = 3;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}

public class SkeletonArcher : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.SkeletonArcher;
        viewDistance = 5;
        maxHealth = 10;
        maxEnergy = 3;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}

public class SkeletonSword : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.SkeletonSword;
        viewDistance = 3;
        maxHealth = 15;
        maxEnergy = 3;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}

public class Vampire : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Vampire;
        viewDistance = 4;
        maxHealth = 25;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}

public class Werewolf : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Werewolf;
        viewDistance = 4;
        maxHealth = 30;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}

/// <summary>
/// Boss enemy class
/// </summary>

public interface IBoss
{
    
}

public class Necromancer : Enemy , IBoss
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Necromancer;
        viewDistance = 3;
        maxHealth = 60;
        sprite = levelManager.spriteManager.slimeSprites[0];
        spriteRenderer.sprite = sprite;
    }
}
