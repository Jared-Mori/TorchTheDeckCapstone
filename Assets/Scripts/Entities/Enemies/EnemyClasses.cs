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
        health = maxHealth;
        maxEnergy = 1;
        energy = maxEnergy;
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
    }
}
