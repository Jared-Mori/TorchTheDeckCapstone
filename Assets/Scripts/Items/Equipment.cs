using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public enum EquipmentType { Helmet, Chestpiece, Boots, Shield, Accessory, Equipment}
public class Equipment : Card
{
    public EquipmentType equipmentType;

    public void PreventDamage()
    {
        Debug.Log("Prevented damage");
    }
}

[System.Serializable]
public class IronHelmet : Equipment
{
    public IronHelmet()
    {
        cardName = "Iron Helmet";
        description = "A sturdy helmet. It's seen better days, but it'll protect your head.";
        this.equipmentType = EquipmentType.Helmet;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class IronChestpiece : Equipment
{
    public IronChestpiece()
    {
        cardName = "Iron Chestpiece";
        description = "A heavy chestpiece. It's a bit rusty, but it'll protect your torso.";
        equipmentType = EquipmentType.Chestpiece;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class IronBoots : Equipment
{
    public IronBoots()
    {
        cardName = "Iron Boots";
        description = "A pair of armored boots. They're a bit scuffed, but they'll protect your feet.";
        equipmentType = EquipmentType.Boots;
        uses = 3;
        rarity = 1;
    }
}

[System.Serializable]
public class Shield : Equipment
{
    public Shield()
    {
        cardName = "Shield";
        description = "A wooden shield. It's cracked in places, but it'll protect you from attacks.";
        // item sprite 89
        equipmentType = EquipmentType.Shield;
        uses = 3;
        rarity = 1;
    }

    public override void Effect(combatDetails player, combatDetails enemy)
    {
        player.isShielded = true;
    }
}

[System.Serializable]
public class IronShield : Equipment
{
    public IronShield()
    {
        cardName = "Reinforced Shield";
        description = "A metal reinforced shield. It's heavy, but still holds together.";
        // item sprite 90
        equipmentType = EquipmentType.Shield;
        uses = 6;
        rarity = 2;
    }

    public override void Effect(combatDetails player, combatDetails enemy)
    {
        player.isShielded = true;
    }
}