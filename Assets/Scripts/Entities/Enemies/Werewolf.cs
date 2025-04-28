using UnityEngine;

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
