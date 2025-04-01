using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{

    public Sprite[] chestSprites;
    public Sprite[] itemSprites;
    public Sprite[] slimeSprites;
    public Sprite[] playerSprites;

    Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chestSprites = Resources.LoadAll<Sprite>("Sprites/Entities/Treasure chests");
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/#1 - Transparent Icons");
        slimeSprites = Resources.LoadAll<Sprite>("Sprites/Entities/slime-Sheet");
        playerSprites = Resources.LoadAll<Sprite>("Sprites/Player/IDLE");

        setSpriteDictionary();
    }

    public void setSpriteDictionary()
    {
        spriteDictionary.Add("Stone", itemSprites[265]);
        spriteDictionary.Add("Health Potion", itemSprites[134]);
        spriteDictionary.Add("Shield", itemSprites[89]);
        spriteDictionary.Add("Reinforced Shield", itemSprites[90]);
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
