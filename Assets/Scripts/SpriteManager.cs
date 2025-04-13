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
        spriteDictionary.Add("Health Potion", itemSprites[134]);
        // Weapons
        // Bows
        // Arrows
        // Armor
        spriteDictionary.Add("Shield", itemSprites[89]);
        spriteDictionary.Add("Reinforced Shield", itemSprites[90]);
        // Status Effects
        spriteDictionary.Add("Poison", itemSprites[53]);
        spriteDictionary.Add("Burn", itemSprites[15]);
        spriteDictionary.Add("Paralysis", itemSprites[14]);
        spriteDictionary.Add("Exhausted", itemSprites[47]);
        spriteDictionary.Add("Haste", itemSprites[52]);
        // Other
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
