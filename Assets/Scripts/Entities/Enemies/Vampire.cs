using UnityEngine;

public class Vampire : Enemy
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        entityType = EntityType.Vampire;
        viewDistance = 4;
        maxHealth = 25;
        health = maxHealth;
        maxEnergy = 5;
        energy = maxEnergy;
    }
}
