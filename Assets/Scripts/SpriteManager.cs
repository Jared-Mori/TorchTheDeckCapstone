using UnityEngine;

public class SpriteManager : MonoBehaviour
{

    public Sprite[] chestSprites;
    public Sprite[] itemSprites;
    public Sprite[] slimeSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chestSprites = Resources.LoadAll<Sprite>("Sprites/Entities/Treasure chests");
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/#1 - Transparent Icons");
        slimeSprites = Resources.LoadAll<Sprite>("Sprites/Entities/slime-Sheet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
