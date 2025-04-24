using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    protected Tilemap walls, floor, objects;
    public Level level;
    private static int FRAMERATE = 30;

    public List<Entity> entities;
    public InventoryManager inventoryManager;
    public SpriteManager spriteManager;
    public Player playerPrefab;
    public Player playerInstance;
    public GameObject chestPrefab;
    public GameObject slimePrefab;
    EntityData[] entityDataArray;
    public InventorySlot Helmet, Chestpiece, Boots, Shield, Accessory, Weapon, Bow;
    bool isLoaded;

    void Start()
    {
        Application.targetFrameRate = FRAMERATE;
        isLoaded = false;
    }

    void Update()
    {
        if (!isLoaded)
        {
            inventoryManager.SetInventoryPanel();
            LoadLevel();
            isLoaded = true;
        }
    }

    public void LoadLevel()
    {
        Debug.Log("Loading level " + level);

        string path = Application.dataPath + "/levelData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // Enable TypeNameHandling to deserialize with type information
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            level.SetLevelData(saveData.level);
            level.SpawnMap(saveData.level);
            this.entityDataArray = saveData.entityDataArray;

            LoadDeck(saveData.deck);
            LoadGear(saveData.gear);
            SpawnManager.RespawnEntities(entities, entityDataArray);

            Debug.Log("Level data loaded from " + path);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
            InitializeDefaultLevel(level.levelNumber);
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Saving level " + level);

        entityDataArray = SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = level.levelNumber,
            entityDataArray = this.entityDataArray,
            deck = SerializeDeck().ToList(),
            gear = SerializeGear()
        };

        // Enable TypeNameHandling to include type information
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() },
            NullValueHandling = NullValueHandling.Include // Include null values
        });

        string path = Application.dataPath + "/levelData.json";

        File.WriteAllText(path, json);

        Debug.Log("Level data saved to " + path);
    }

    public void InitializeDefaultLevel(int levelNumber)
    {
        level.SetLevelData(levelNumber);
        level.SpawnMap(levelNumber);
        // Ensure the player object is properly instantiated
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
            entities.Add(playerInstance);
            inventoryManager.SetPlayer(playerInstance);
        }else
        {
            playerInstance.SetPosition(Vector3Int.zero);
        }

        SpawnManager.SpawnEntities(entities, level);
        
        foreach (Entity entity in entities)
        {
            entity.SetLevelManager(this);
        }

        Debug.Log("Initialized default level");
    }

    public void LoadDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card card = deck[i];
            List<Card> hand = inventoryManager.pileController.hand;
            if (card.itemType != ItemType.Default && card.itemType != ItemType.Item && card.itemType != ItemType.Arrow)
            {
                inventoryManager.pileController.AddCard(card);
            }
            else
            {
                Card existingCard = hand.FirstOrDefault(c => c.cardName == card.cardName);
                if (existingCard != null)
                {
                    existingCard.count++;
                }
                else
                {
                    inventoryManager.pileController.AddCard(card);
                }
            }
        }
    }

    public List<Card> SerializeDeck()
    {
        List<Card> deck = new List<Card>();
        foreach (Card card in inventoryManager.pileController.hand)
        {
            if (card != null && !(card is DefaultCard))
            {
                for (int i = 0; i < card.count; i++)
                {
                    deck.Add(card);
                }
            }
        }
        
        return deck;
    }
    public Card[] SerializeGear()
    {
        Card[] gear = new Card[7];
        gear[0] = Helmet.cardWrapper != null ? Helmet.cardWrapper.card : null;
        gear[1] = Chestpiece.cardWrapper != null ? Chestpiece.cardWrapper.card : null;
        gear[2] = Boots.cardWrapper != null ? Boots.cardWrapper.card : null;
        gear[3] = Shield.cardWrapper != null ? Shield.cardWrapper.card : null;
        gear[4] = Accessory.cardWrapper != null ? Accessory.cardWrapper.card : null;
        gear[5] = Weapon.cardWrapper != null ? Weapon.cardWrapper.card : null;
        gear[6] = Bow.cardWrapper != null ? Bow.cardWrapper.card : null;

        for(int i = 0; i < 7; i++)
        {
            if (gear[i] is DefaultCard){
                gear[i] = null;
            }
        }

        Debug.Log("Serialized gear: " + string.Join(", ", gear.Select(g => g != null ? g.cardName : "null")));
        return gear;
    }

    public void LoadGear(Card[] gear)
    {
        if (gear == null || gear.Length != 7)
        {
            Debug.LogWarning("Invalid gear array. Cannot load gear.");
            return;
        }

        Helmet.SetCard(gear[0] != null ? gear[0] : null);
        Chestpiece.SetCard(gear[1] != null ? gear[1] : null);
        Boots.SetCard(gear[2] != null ? gear[2] : null);
        Shield.SetCard(gear[3] != null ? gear[3] : null);
        Accessory.SetCard(gear[4] != null ? gear[4] : null);
        Weapon.SetCard(gear[5] != null ? gear[5] : null);
        Bow.SetCard(gear[6] != null ? gear[6] : null);
    }

    public EntityData[] SerializeEntities()
    {
        EntityData[] entityDataArray = new EntityData[entities.Count];

        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];
            EntityData data = new EntityData
            {
                facing = entity.facing,
                xPos = entity.gridPosition.x,
                yPos = entity.gridPosition.y,
                isAttacker = entity.isAttacker,
                entityType = entity.entityType,
                health = entity.health,
                maxHealth = entity.maxHealth,
                energy = entity.energy,
                maxEnergy = entity.maxEnergy
            };
            if(data.entityType == EntityType.Chest)
            {
                Chest chest = entity as Chest;
                data.isOpenedChest = chest.isOpen;
            }

            entityDataArray[i] = data;
        }

        return entityDataArray;
    }
}
