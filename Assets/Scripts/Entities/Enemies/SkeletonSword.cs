using UnityEngine;

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