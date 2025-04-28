using UnityEngine;

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
