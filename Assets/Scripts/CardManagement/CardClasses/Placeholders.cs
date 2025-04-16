using UnityEngine;

/// <summary>
/// Placeholder classes for various card types
/// Used for starting combat without specific card equipped
/// </summary>

public class TempWeapon : Card, Weapon
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

public class TempBow : Card, Bow
{
    public void BowEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This bow does nothing extra!");
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

public class TempHelm : Card, Armor
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

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This helmet does nothing extra!");
    }
}

public class TempChest : Card, Armor
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

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This Chestpiece does nothing extra!");
    }
}

public class TempBoots : Card, Armor
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

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This pair of boots does nothing extra!");
    }
}

public class TempShield : Card, Armor
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

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This shield does nothing extra!");
    }
}

public class TempAccessory : Card, Accessory
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

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This accessory does nothing extra!");
    }
}
