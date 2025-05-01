using UnityEngine;

public class Goblin : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Goblin;
        viewDistance = 3;
        maxHealth = 7;
        maxEnergy = 3;
        health = maxHealth;
        energy = maxEnergy;
    }
}
