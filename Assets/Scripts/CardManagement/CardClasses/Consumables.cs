using UnityEngine;

/// <summary>
/// Consumable classes
/// summary>
public interface HealingItem
{
    int healing { get; set; }
}

[System.Serializable]
public class HealthPotion : Card, HealingItem
{
    public int healing { get; set; } = 5; // Default healing value
    public HealthPotion()
    {
        cardName = "Health Potion";
        description = "A potion that restores 5 health.";
        uses = 1;
        rarity = 1;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Heal(user, healing);
    }
}

[System.Serializable]
public class Stone : Card, Weapon
{
    public int damage { get; set; } = 1; // Default damage value
    public Stone()
    {
        cardName = "Stone";
        description = "A hefty stone. You could probably throw it.";
        uses = 1;
        rarity = 0;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
    }
}
