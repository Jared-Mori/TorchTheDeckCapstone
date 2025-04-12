using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public enum EntityType { Player, Chest, Door, Rock, Slime, Goblin, Skeleton, Orc, Dragon }

public class LevelManager : MonoBehaviour
{
    protected Tilemap walls, floor;
    public int level;
    private static int FRAMERATE = 30;

    public List<Entity> entities;
    public InventoryManager inventoryManager;
    public SpriteManager spriteManager;
    public Enemy attacker;
    public Player playerPrefab;
    public Player playerInstance;

    public GameObject gridPrefab;
    public GameObject gridInstance;

    public GameObject chestPrefab;
    public GameObject slimePrefab;
    EntityData[] entityDataArray;
    public InventorySlot Helmet, Chestpiece, Boots, Shield, Accessory, Weapon, Bow;
    bool isLoaded;

    void Start()
    {
        Application.targetFrameRate = FRAMERATE;

        // Instantiate the Grid prefab
        gridInstance = Instantiate(gridPrefab);
        walls = gridInstance.transform.Find("Walls").GetComponent<Tilemap>();
        floor = gridInstance.transform.Find("Floor").GetComponent<Tilemap>();
        isLoaded = false;

        JsonTest(); // Test serialization and deserialization
    }

    void Update()
    {
        if (!isLoaded)
        {
            LoadLevel();
            isLoaded = true;
        }
    }

    public Tilemap GetWalls()
    {
        return walls;
    }

    public Tilemap GetFloor()
    {
        return floor;
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

            this.level = saveData.level;
            this.entityDataArray = saveData.entityDataArray;

            LoadDeck(saveData.deck, saveData.gear);
            DeserializeEntities(entityDataArray);

            Debug.Log("Level data loaded from " + path);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
            InitializeDefaultLevel();
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Saving level " + level);

        entityDataArray = SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = this.level,
            entityDataArray = this.entityDataArray,
            deck = inventoryManager.pileController.hand.Where(card => card != null).ToList(),
            gear = SerializeGear().Where(card => card != null).ToArray()
        };

        // Enable TypeNameHandling to include type information
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() }
        });

        string path = Application.dataPath + "/levelData.json";

        File.WriteAllText(path, json);

        Debug.Log("Level data saved to " + path);
    }

    public void DeleteSaveFile()
    {
        string path = Application.dataPath + "/levelData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at " + path);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
        }
    }

    private void InitializeDefaultLevel()
    {
        // Initialize the level with default values
        level = 1;

        // Ensure the player object is properly instantiated
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
        }
        entities.Add(playerInstance);
        inventoryManager.SetPlayer(playerInstance);

        GameObject chestInstance = Instantiate(chestPrefab);
        entities.Add(chestInstance.GetComponent<Chest>());

        GameObject slimeInstance = Instantiate(slimePrefab);
        entities.Add(slimeInstance.GetComponent<Slime>());
        
        foreach (Entity entity in entities)
        {
            entity.SetLevelManager(this);
        }

        Debug.Log("Initialized default level");
    }

    public void LoadDeck(List<Card> deck, Card[] gear)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card card = deck[i];
            card.AddToDeck();
        }
        LoadGear(gear);
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
                health = entity.health,
                maxHealth = entity.maxHealth,
                energy = entity.energy,
                maxEnergy = entity.maxEnergy,
                isAttacker = entity.isAttacker,
                entityType = entity.entityType
            };
            if(data.entityType == EntityType.Chest)
            {
                Chest chest = entity as Chest;
                data.isOpenedChest = chest.isOpen;
            }
            else if(data.entityType == EntityType.Door)
            {
                Door door = entity as Door;
                data.isOpenedDoor = door.isOpen;
            }

            entityDataArray[i] = data;
        }

        return entityDataArray;
    }

    public void DeserializeEntities(EntityData[] entityDataArray)
    {
        foreach (EntityData data in entityDataArray)
        {
            Entity entity = null;

            switch (data.entityType)
            {
                case EntityType.Player:
                    playerInstance = Instantiate(playerPrefab);
                    entities.Add(playerInstance);
                    inventoryManager.SetPlayer(playerInstance);
                    playerInstance.SetLevelManager(this);
                    break;
                case EntityType.Chest:
                    entity = Instantiate(chestPrefab).GetComponent<Chest>();
                    break;
                case EntityType.Slime:
                    entity = Instantiate(slimePrefab).GetComponent<Slime>();
                    break;
            }

            if (entity != null && data.entityType != EntityType.Player)
            {
                entity.SetLevelManager(this);
                entity.facing = data.facing;
                entity.health = data.health;

                if (data.entityType == EntityType.Chest)
                {
                    Chest chest = entity as Chest;
                    chest.isOpen = data.isOpenedChest;
                }
                else if (data.entityType == EntityType.Door)
                {
                    Door door = entity as Door;
                    door.isOpen = data.isOpenedDoor;
                }

                entity.loadPosition = new Vector3Int(data.xPos, data.yPos, 0);
                entity.isLoaded = true;
                entities.Add(entity);
            }
            else if (data.entityType == EntityType.Player)
            {
                playerInstance.facing = data.facing;
                playerInstance.health = data.health;
                playerInstance.isAttacker = data.isAttacker;

                playerInstance.loadPosition = new Vector3Int(data.xPos, data.yPos, 0);
                playerInstance.isLoaded = true;
            }
        }
    }

    public void JsonTest()
    {
        // Test serialization on bow card
        Card testCard = new Bow();
        string json = JsonConvert.SerializeObject(testCard, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() }
        });
        Debug.Log($"Serialized JSON: {json}");

        // Test deserialization
        Card deserializedCard = JsonConvert.DeserializeObject<Card>(json, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() }
        });
        Debug.Log($"Deserialized card: {deserializedCard.cardName}");
    }
}
