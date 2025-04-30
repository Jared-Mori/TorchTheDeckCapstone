using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{

    public Sprite[] chestSprites;
    public Sprite[] itemSprites;
    public Sprite[] weaponSprites, accessorySprites, armorSprites;
    public Sprite[] slimeSprites;
    public Sprite[] playerSprites;
    public Sprite[] rockSprites = new Sprite[6];
    public Sprite energyBarSprite, energyFillSprite;

    Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Loading sprites...");
        chestSprites = Resources.LoadAll<Sprite>("Sprites/Entities/Treasure chests");
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/#1 - Transparent Icons");
        weaponSprites = Resources.LoadAll<Sprite>("Sprites/Items/Weapons");
        accessorySprites = Resources.LoadAll<Sprite>("Sprites/Items/Transperent");
        armorSprites = Resources.LoadAll<Sprite>("Sprites/Items/Armor");
        slimeSprites = Resources.LoadAll<Sprite>("Sprites/Entities/slime-Sheet");
        playerSprites = Resources.LoadAll<Sprite>("Sprites/Player/IDLE");
        energyBarSprite = Resources.Load<Sprite>("Sprites/Inventory/energyBorder");
        energyFillSprite = Resources.Load<Sprite>("Sprites/Inventory/energyFill");
        rockSprites[0] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_97");
        rockSprites[1] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_98");
        rockSprites[2] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_99");
        rockSprites[3] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_103");
        rockSprites[4] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_104");
        rockSprites[5] = Resources.Load<Sprite>("Sprites/Entities/Rock/TX Props_105");

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
        spriteDictionary.Add("Healing Potion", itemSprites[130]);
        spriteDictionary.Add("Greater Healing Potion", itemSprites[134]);
        spriteDictionary.Add("Divine Healing Potion", itemSprites[138]);
        spriteDictionary.Add("Potion of Haste", itemSprites[137]);
        spriteDictionary.Add("Stamina Draught", itemSprites[287]);
        spriteDictionary.Add("Vitality Draught", itemSprites[285]);
        // Weapons
        spriteDictionary.Add("Iron Sword", weaponSprites[13]);
        spriteDictionary.Add("Adamantite Sword", weaponSprites[47]);
        spriteDictionary.Add("Flaming Sword", weaponSprites[69]);
        spriteDictionary.Add("Obsidian Blade", weaponSprites[60]);
        spriteDictionary.Add("Iron Axe", weaponSprites[27]);
        spriteDictionary.Add("Orichalcum Axe", weaponSprites[87]);
        spriteDictionary.Add("Crimson Cutter", weaponSprites[74]);
        // Bows
        spriteDictionary.Add("Longbow", weaponSprites[9]);
        spriteDictionary.Add("Crossbow", weaponSprites[19]);
        spriteDictionary.Add("Arcane Charged Bow", weaponSprites[85]);
        spriteDictionary.Add("Stratus", weaponSprites[78]);
        // Arrows
        spriteDictionary.Add("Wooden Arrow", itemSprites[92]);
        spriteDictionary.Add("Steel Tipped Arrow", itemSprites[92]);
        spriteDictionary.Add("Poison Arrow", itemSprites[92]);
        spriteDictionary.Add("Lightning Arrow", itemSprites[92]);
        // Armor
        spriteDictionary.Add("Iron Helmet", armorSprites[173]);
        spriteDictionary.Add("Iron Chestpiece", armorSprites[196]);
        spriteDictionary.Add("Iron Boots", armorSprites[260]);
        spriteDictionary.Add("Mythril Helmet", armorSprites[180]);
        spriteDictionary.Add("Mythril Chestpiece", armorSprites[200]);
        spriteDictionary.Add("Mythril Boots", armorSprites[266]);
        spriteDictionary.Add("Ram's Horn Helm", armorSprites[185]);
        spriteDictionary.Add("Invigorating Chestplate", armorSprites[201]);
        spriteDictionary.Add("Frogz kin Boots", armorSprites[234]);
        spriteDictionary.Add("Bone Lord's Helmet", armorSprites[185]);
        spriteDictionary.Add("Bone Lord's Breastplate", armorSprites[197]);
        spriteDictionary.Add("Bone Lord's Greaves", armorSprites[244]);
        spriteDictionary.Add("Archmage's Gall", armorSprites[167]);
        spriteDictionary.Add("Dragon King's Curiass", armorSprites[203]);
        spriteDictionary.Add("Falcon Sabatons", armorSprites[256]);
        // Shields
        spriteDictionary.Add("Shield", weaponSprites[10]);
        spriteDictionary.Add("Reinforced Shield", weaponSprites[17]);
        spriteDictionary.Add("Thorned Shield", weaponSprites[37]);
        spriteDictionary.Add("Divine Shield", weaponSprites[79]);
        // Accessories
        spriteDictionary.Add("Philosopher's Stone", accessorySprites[40]);
        spriteDictionary.Add("Ring of the Forge", accessorySprites[39]);
        spriteDictionary.Add("Quiver Talisman", accessorySprites[42]);
        spriteDictionary.Add("Energy Talisman", accessorySprites[37]);
        spriteDictionary.Add("Scorpion Medalion", accessorySprites[38]);
        spriteDictionary.Add("Dragon King's Scale", accessorySprites[32]);
        spriteDictionary.Add("Fangs of the Vampire", accessorySprites[35]);
        spriteDictionary.Add("Werewolf's Glare", accessorySprites[47]);
        // Enemy Cards
        spriteDictionary.Add("Slime Burst", slimeSprites[0]);
        spriteDictionary.Add("Goblin Dagger", itemSprites[79]);
        spriteDictionary.Add("Skeleton Arrow", itemSprites[39]);
        spriteDictionary.Add("Skeleton Poison Arrow", itemSprites[92]);
        spriteDictionary.Add("Rib Bone", itemSprites[237]);
        spriteDictionary.Add("Vampiric Bite", itemSprites[36]);
        spriteDictionary.Add("Vampire's Robe", armorSprites[205]);
        spriteDictionary.Add("Vampire's Boots", armorSprites[250]);
        spriteDictionary.Add("Darkness", itemSprites[2]);
        spriteDictionary.Add("Howl", itemSprites[53]);
        spriteDictionary.Add("Clawed Slash", itemSprites[273]);
        spriteDictionary.Add("Werewolf's Mane", itemSprites[291]);
        spriteDictionary.Add("Werewolf's Hide", itemSprites[272]);
        spriteDictionary.Add("Werewolf's Pursuit", itemSprites[71]);
        // Boss Cards
        spriteDictionary.Add("Necrotic Touch", itemSprites[49]);
        spriteDictionary.Add("Magic Shield", itemSprites[163]);
        spriteDictionary.Add("Fireball", itemSprites[2]);
        spriteDictionary.Add("Cleanse", itemSprites[18]);
        spriteDictionary.Add("Conjure Arcane Barrage", itemSprites[14]);
        spriteDictionary.Add("Arcane Missile", itemSprites[13]);
        spriteDictionary.Add("Curse", itemSprites[5]);
        spriteDictionary.Add("Necromancer's Crown", armorSprites[186]);
        spriteDictionary.Add("Necromancer's Cloak", itemSprites[116]);
        spriteDictionary.Add("Necromancer's Slippers", armorSprites[248]);
        spriteDictionary.Add("Ring of the Dead", accessorySprites[36]);
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
