using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{

    public Sprite[] chestSprites;
    public Sprite[] itemSprites;
    public Sprite[] slimeSprites;
    public Sprite[] playerSprites;
    public Sprite energyBarSprite, energyFillSprite;

    Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Loading sprites...");
        chestSprites = Resources.LoadAll<Sprite>("Sprites/Entities/Treasure chests");
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/#1 - Transparent Icons");
        slimeSprites = Resources.LoadAll<Sprite>("Sprites/Entities/slime-Sheet");
        playerSprites = Resources.LoadAll<Sprite>("Sprites/Player/IDLE");
        energyBarSprite = Resources.Load<Sprite>("Sprites/Inventory/energyBorder");
        energyFillSprite = Resources.Load<Sprite>("Sprites/Inventory/energyFill");

        SetSpriteDictionary();
    }

    public void SetSpriteDictionary()
    {
        Debug.Log("Setting sprite dictionary...");
        // TempCards
        spriteDictionary.Add("TempHelm", itemSprites[104]);
        spriteDictionary.Add("TempChest", itemSprites[108]);
        spriteDictionary.Add("TempBoots", itemSprites[120]);
        spriteDictionary.Add("TempShield", itemSprites[58]);
        spriteDictionary.Add("TempAccessory", itemSprites[126]);
        spriteDictionary.Add("TempBow", itemSprites[93]);
        spriteDictionary.Add("TempWeapon", itemSprites[74]);
        // Consumables
        spriteDictionary.Add("Stone", itemSprites[265]);
        spriteDictionary.Add("Healing Potion", itemSprites[134]);
        spriteDictionary.Add("Greater Healing Potion", itemSprites[134]);
        spriteDictionary.Add("Divine Healing Potion", itemSprites[134]);
        spriteDictionary.Add("Haste Potion", itemSprites[134]);
        spriteDictionary.Add("Stamina Draught", itemSprites[134]);
        spriteDictionary.Add("Vitality Draught", itemSprites[134]);
        // Weapons
        spriteDictionary.Add("Iron Sword", itemSprites[74]);
        spriteDictionary.Add("Adamantite Sword", itemSprites[74]);
        spriteDictionary.Add("Flaming Sword", itemSprites[74]);
        spriteDictionary.Add("Obsidian Blade", itemSprites[74]);
        spriteDictionary.Add("Iron Axe", itemSprites[74]);
        spriteDictionary.Add("Orichalcum Axe", itemSprites[74]);
        spriteDictionary.Add("Crimson Cutter", itemSprites[74]);
        // Bows
        spriteDictionary.Add("Longbow", itemSprites[93]);
        spriteDictionary.Add("Crossbow", itemSprites[93]);
        spriteDictionary.Add("Arcane Charged Bow", itemSprites[93]);
        spriteDictionary.Add("Stratus", itemSprites[93]);
        // Arrows
        spriteDictionary.Add("Wooden Arrow", itemSprites[92]);
        spriteDictionary.Add("Steel Tipped Arrow", itemSprites[92]);
        spriteDictionary.Add("Poison Arrow", itemSprites[92]);
        spriteDictionary.Add("Lightning Arrow", itemSprites[92]);
        // Armor
        spriteDictionary.Add("Iron Helmet", itemSprites[104]);
        spriteDictionary.Add("Iron Chestpiece", itemSprites[108]);
        spriteDictionary.Add("Iron Boots", itemSprites[120]);
        spriteDictionary.Add("Mythril Helmet", itemSprites[104]);
        spriteDictionary.Add("Mythril Chestpiece", itemSprites[108]);
        spriteDictionary.Add("Mythril Boots", itemSprites[120]);
        spriteDictionary.Add("Ram's Horn Helm", itemSprites[104]);
        spriteDictionary.Add("Invigorating Chestplate", itemSprites[108]);
        spriteDictionary.Add("Frogz kin Boots", itemSprites[120]);
        spriteDictionary.Add("Bone Lord's Helmet", itemSprites[104]);
        spriteDictionary.Add("Bone Lord's Breastplate", itemSprites[108]);
        spriteDictionary.Add("Bone Lord's Greaves", itemSprites[120]);
        spriteDictionary.Add("Archmage's Gall", itemSprites[104]);
        spriteDictionary.Add("Dragon King's Curiass", itemSprites[108]);
        spriteDictionary.Add("Falcon Sabatons", itemSprites[120]);
        // Shields
        spriteDictionary.Add("Shield", itemSprites[89]);
        spriteDictionary.Add("Reinforced Shield", itemSprites[90]);
        spriteDictionary.Add("Thorned Shield", itemSprites[91]);
        spriteDictionary.Add("Divine Shield", itemSprites[91]);
        // Accessories
        spriteDictionary.Add("Philosopher's Stone", itemSprites[126]);
        spriteDictionary.Add("Ring of the Forge", itemSprites[126]);
        spriteDictionary.Add("Quiver Talisman", itemSprites[126]);
        spriteDictionary.Add("Energy Talisman", itemSprites[126]);
        spriteDictionary.Add("Scorpion Medalion", itemSprites[126]);
        spriteDictionary.Add("Dragon King's Scale", itemSprites[126]);
        spriteDictionary.Add("Fangs of the Vampire", itemSprites[126]);
        spriteDictionary.Add("Werewolf's Glare", itemSprites[126]);
        // Enemy Cards
        
        // Status Effects
        spriteDictionary.Add("Poison", itemSprites[53]);
        spriteDictionary.Add("Burn", itemSprites[15]);
        spriteDictionary.Add("Paralysis", itemSprites[14]);
        spriteDictionary.Add("Exhausted", itemSprites[47]);
        spriteDictionary.Add("Haste", itemSprites[52]);
        // Other
        spriteDictionary.Add("Damage", itemSprites[50]);
        spriteDictionary.Add("Healing", itemSprites[43]);
        spriteDictionary.Add("Energy Border", energyBarSprite);
        spriteDictionary.Add("Energy Fill", energyFillSprite);
    }

    public Sprite GetSprite(string name)
    {
        if (spriteDictionary.ContainsKey(name))
        {
            return spriteDictionary[name];
        }
        else
        {
            Debug.LogError("Sprite not found: " + name);
            return null;
        }
    }
}
