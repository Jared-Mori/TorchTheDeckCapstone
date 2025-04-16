using UnityEngine;

/// <summary>
/// Weapon classes
/// </summary>

public interface Weapon
{
    int damage { get; set; }
}

/// <summary>
/// Swords
/// /// </summary>
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

[System.Serializable]
public class AdamantiteSword : Card, Weapon
{
    public int damage { get; set; } = 3; // Default damage value
    public AdamantiteSword()
    {
        cardName = "Adamantite Sword";
        description = "A sword made of adamantite. It's heavy, but its edge is impeccable.";
        uses = 8;
        rarity = Rarity.Uncommon;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
    }
}

[System.Serializable]
public class FlamingSword : Card, Weapon, IStatusEffect
{
    public int damage { get; set; } = 2; // Default damage value
    public Status status { get; set; } = new Burn(); // Default status effect
    public FlamingSword()
    {
        cardName = "Flaming Sword";
        description = "A sword engulfed in flames. It burns with a fierce heat.";
        uses = 8;
        rarity = Rarity.Rare;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
        target.statusEffects.Add(status);
    }
}

[System.Serializable]
public class ObsidianBlade : Card, Weapon
{
    public int damage { get; set; } = 2; // Default damage value
    public ObsidianBlade()
    {
        cardName = "Obsidian Blade";
        description = "A blade made of obsidian. The more you cut, the sharper it gets.";
        uses = 8;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);

        this.damage += 1; // Increase damage by 1 after each use
    }
}

/// <summary>
/// Axes
/// /// </summary>

[System.Serializable]
public class IronAxe : Card, Weapon
{
    public int damage { get; set; } = 4; // Default damage value
    public IronAxe()
    {
        cardName = "Iron Axe";
        description = "A rusted axe. It's still sharp, but probably won't hold out for long.";
        uses = 2;
        rarity = Rarity.Common;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
    }
}

[System.Serializable]
public class OrichalcumAxe : Card, Weapon
{
    public int damage { get; set; } = 2; // Default damage value
    public OrichalcumAxe()
    {
        cardName = "Orichalcum Axe";
        description = "An axe made of Orichalcum. Said to be nearly indestructable.";
        uses = 12;
        rarity = Rarity.Rare;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
    }
}

[System.Serializable]
public class CrimsonCutter : Card, Weapon
{
    public int damage { get; set; } = 4; // Default damage value
    public CrimsonCutter()
    {
        cardName = "Crimson Cutter";
        description = "A crimson axe. It drains the life from those it cleaves.";
        uses = 4;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, damage);
        CombatMechanics.Heal(user, damage); // Heal the user for the damage dealt
    }
}
