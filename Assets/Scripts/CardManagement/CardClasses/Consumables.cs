using System.Threading.Tasks;
using Unity.IntegerTime;
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
        cardName = "Healing Potion";
        description = "A potion that restores a small amount of health.";
        tooltip = $"Restores {healing} health points.";
        uses = 1;
        rarity = Rarity.Common;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.Heal(user, healing);
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class GreatHealthPotion : Card, HealingItem
{
    public int healing { get; set; } = 10; // Default healing value
    public GreatHealthPotion()
    {
        cardName = "Greater Healing Potion";
        description = "A potion that restores a moderate amount of health.";
        tooltip = $"Restores {healing} health points.";
        uses = 1;
        rarity = Rarity.Uncommon;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.Heal(user, healing);
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class SuperHealthPotion : Card, HealingItem
{
    public int healing { get; set; } = 20; // Default healing value
    public SuperHealthPotion()
    {
        cardName = "Divine Healing Potion";
        description = "A potion that restores a monumental amount of health.";
        tooltip = $"Restores {healing} health points.";
        uses = 1;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.Heal(user, healing);
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class HastePotion : Card, IStatusEffect
{
    public Status status { get; set; } = new Haste();
    public HastePotion()
    {
        cardName = "Potion of Haste";
        description = "A potion to speed you up, at the cost of extra fatigue later.";
        tooltip = "Grants Haste status effect.";
        uses = 1;
        rarity = Rarity.Rare;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        user.statusEffects.Add(status);
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class VitalityDraught : Card
{
    public int vitality { get; set; } = 20; // Default vitality value
    public VitalityDraught()
    {
        cardName = "Vitality Draught";
        description = "A draught that permanently improves your vitality.";
        tooltip = $"Permanently increases max health by {vitality}.";
        uses = 1;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        user.healthMax += vitality;
        user.health += vitality;
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class StaminaDraught : Card
{
    public int stamina { get; set; } = 2; // Default vitality value
    public StaminaDraught()
    {
        cardName = "Stamina Draught";
        description = "A draught that permanently improves your Stamina.";
        tooltip = $"Permanently increases max energy by {stamina}.";
        uses = 1;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        user.energyMax += stamina;
        user.energy += stamina;
        return Task.CompletedTask;
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
        tooltip = "Its a rock.";
        uses = 1;
        rarity = Rarity.Common;
        isStackable = true;
        itemType = ItemType.Item;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        await CombatMechanics.Defend(target, user, damage);
    }
}


