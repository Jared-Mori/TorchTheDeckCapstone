using UnityEngine;

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
