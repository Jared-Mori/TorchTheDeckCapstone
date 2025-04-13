using UnityEngine;

/// <summary>
/// Weapon classes
/// </summary>

public interface Weapon
{
    int damage { get; set; }
}

[System.Serializable]
public class IronSword : Card, Weapon
{
    public int damage { get; set; } = 2; // Default damage value
    public IronSword()
    {
        cardName = "Iron Sword";
        description = "A rusted sword. It's still sharp, but probably won't hold out for long.";
        uses = 4;
        rarity = 1;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
    }
}
