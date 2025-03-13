using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Equipment : Card
{
    public static Dictionary<int, Equipment> allEquipmentTypes = new Dictionary<int, Equipment>
    {
        { 0, new Equipment { equipmentType = "Helmet" } },
        { 1, new Equipment { equipmentType = "Chestpiece" } },
        { 2, new Equipment { equipmentType = "Boots" } },
        { 3, new Equipment { equipmentType = "Shield" } },
        { 5, new Equipment { equipmentType = "Accessory" } }
    };
    public string equipmentType;

    public void PreventDamage()
    {
        Use();
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
        artwork = Resources.Load<Sprite>("Sprites/Items/Helmet");
        equipmentType = "Helmet";
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
        artwork = Resources.Load<Sprite>("Sprites/Items/Chestpiece");
        equipmentType = "Chestpiece";
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
        artwork = Resources.Load<Sprite>("Sprites/Items/Boots");
        equipmentType = "Boots";
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
        artwork = Resources.Load<Sprite>("Sprites/Items/Shield");
        equipmentType = "Shield";
        uses = 3;
        rarity = 1;
    }

    public override void Effect()
    {
        owner.isShielded = true;
        Use();
    }
}