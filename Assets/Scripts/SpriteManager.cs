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
        spriteDictionary.Add("Stone", itemSprites[265]);
        spriteDictionary.Add("Health Potion", itemSprites[134]);
        spriteDictionary.Add("Shield", itemSprites[89]);
        spriteDictionary.Add("Reinforced Shield", itemSprites[90]);
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
