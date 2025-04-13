using UnityEngine;

/// <summary>
/// Armor classes
/// </summary>
public interface Armor
{
    void ArmorEffect(CombatDetails user, CombatDetails target);
}

[System.Serializable]
public class IronHelmet : Card, Armor
{
    public IronHelmet()
    {
        cardName = "Iron Helmet";
        description = "A sturdy helmet. It's seen better days, but it'll protect your head.";
        this.itemType = ItemType.Helmet;
        uses = 3;
        rarity = 1;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
    }
}

[System.Serializable]
public class IronChestpiece : Card, Armor
{
    public IronChestpiece()
    {
        cardName = "Iron Chestpiece";
        description = "A heavy chestpiece. It's a bit rusty, but it'll protect your torso.";
        itemType = ItemType.Chestpiece;
        uses = 3;
        rarity = 1;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
    }
}

[System.Serializable]
public class IronBoots : Card, Armor
{
    public IronBoots()
    {
        cardName = "Iron Boots";
        description = "A pair of armored boots. They're a bit scuffed, but they'll protect your feet.";
        itemType = ItemType.Boots;
        uses = 3;
        rarity = 1;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
    }
}

[System.Serializable]
public class Shield : Card, Armor
{
    public Shield()
    {
        cardName = "Shield";
        description = "A wooden shield. It's cracked in places, but it'll protect you from attacks.";
        itemType = ItemType.Shield;
        uses = 3;
        rarity = 1;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
    }

    public override bool Use(){

        return base.Use();
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        user.isShielded = true;
    }
}

[System.Serializable]
public class IronShield : Card, Armor
{
    public IronShield()
    {
        cardName = "Reinforced Shield";
        description = "A metal reinforced shield. It's heavy, but still holds together.";
        itemType = ItemType.Shield;
        uses = 6;
        rarity = 2;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.UseEnergy(user, 1);
        user.isShielded = true;
    }
}
