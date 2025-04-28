using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Placeholder classes for various card types
/// Used for starting combat without specific card equipped
/// </summary>
public interface TempCard
{
    // This interface can be used to mark all temporary cards
}
public class TempWeapon : Card, Weapon, TempCard
{
    public int damage { get; set; } = 0; // Default damage value

    public TempWeapon()
    {
        cardName = "TempWeapon";
        description = "A placeholder weapon. It does nothing.";
        tooltip = "";
        itemType = ItemType.Weapon;
        uses = 1;
        rarity = 0;
    }
}

public class TempBow : Card, Bow, TempCard
{
    public Task BowEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This bow does nothing extra!");
        return Task.CompletedTask;
    }

    public TempBow()
    {
        cardName = "TempBow";
        description = "A placeholder bow. It does nothing.";
        tooltip = "";
        itemType = ItemType.Bow;
        uses = 1;
        rarity = 0;
    }
}

public class TempHelm : Card, Armor, TempCard
{
    public int armor { get; set; } = 0; // Default armor value

    public TempHelm()
    {
        cardName = "TempHelm";
        description = "A placeholder helmet. It does nothing.";
        tooltip = "";
        itemType = ItemType.Helmet;
        uses = 1;
        rarity = 0;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This helmet does nothing extra!");
        return Task.CompletedTask;
    }
}

public class TempChest : Card, Armor, TempCard
{
    public int armor { get; set; } = 0; // Default armor value

    public TempChest()
    {
        cardName = "TempChest";
        description = "A placeholder Chestpiece. It does nothing.";
        tooltip = "";
        itemType = ItemType.Chestpiece;
        uses = 1;
        rarity = 0;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This Chestpiece does nothing extra!");
        return Task.CompletedTask;
    }
}

public class TempBoots : Card, Armor, TempCard
{
    public int armor { get; set; } = 0; // Default armor value

    public TempBoots()
    {
        cardName = "TempBoots";
        description = "A placeholder pair of boots. It does nothing.";
        tooltip = "";
        itemType = ItemType.Boots;
        uses = 1;
        rarity = 0;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This pair of boots does nothing extra!");
        return Task.CompletedTask;
    }
}

public class TempShield : Card, Armor, TempCard
{
    public int armor { get; set; } = 0; // Default armor value

    public TempShield()
    {
        cardName = "TempShield";
        description = "A placeholder shield. It does nothing.";
        tooltip = "";
        itemType = ItemType.Shield;
        uses = 1;
        rarity = 0;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This shield does nothing extra!");
        return Task.CompletedTask;
    }
}

public class TempAccessory : Card, Accessory, TempCard
{
    public TempAccessory()
    {
        cardName = "TempAccessory";
        description = "A placeholder accessory. It does nothing.";
        tooltip = "";
        itemType = ItemType.Accessory;
        uses = 1;
        rarity = 0;
    }

    public Task AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This accessory does nothing extra!");
        return Task.CompletedTask;
    }
}
