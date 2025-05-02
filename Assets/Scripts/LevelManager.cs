using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using DG.Tweening;
using System.Threading.Tasks;

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
            StatTracker.SetNecromancerDefeated(false);
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

            _ = LoadDeck(saveData.deck);
            LoadGear(saveData.gear);
            SpawnManager.RespawnEntities(entities, entityDataArray);

            inventoryManager.SetPlayer(playerInstance);

            Debug.Log("Level data loaded from " + path);
        }
        else
        {
            if (StatTracker.IsTutorialCompleted())
            {
                // If the tutorial is completed, load straight to level 3
                InitializeDefaultLevel(3);
            }
            else
            {
                // If the tutorial is not completed, load the tutorial level
                InitializeDefaultLevel(1);
            }
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
        
        SpawnManager.SpawnEntities(entities, level);
        level.SetPlayerPosition(playerInstance);
        inventoryManager.SetPlayer(playerInstance);

        // Check for bone pile data
        BonePileCheck();
        
        PileController pc = inventoryManager.pileController;

        Debug.Log("Initialized default level");
    }

    public void GoToNextLevel(int toFloor)
    {
        DOTween.KillAll(true);
        // Create a temporary list to store entities to remove
        List<Entity> entitiesToRemove = new List<Entity>();

        foreach (Entity entity in entities)
        {
            if (entity is Player)
            {
                // Set the player's position to the zero vector
                entity.SetPosition(Vector3Int.zero);
            }
            else
            {
                // Add non-player entities to the removal list
                entitiesToRemove.Add(entity);
            }
        }

        // Remove and destroy all non-player entities
        foreach (Entity entity in entitiesToRemove)
        {
            Debug.Log("Destroying entity: " + entity.name);
            Destroy(entity.gameObject);
            entities.Remove(entity);
        }

        // Destroy the current floor
        GameObject floor = GameObject.Find("Floor" + level.levelNumber + "(Clone)");
        if (floor != null)
        {
            Destroy(floor);
        }

        Debug.Log("Loading next level: " + toFloor);
        // Initialize the next level
        InitializeDefaultLevel(toFloor);
    }

    public void BonePileCheck()
    {
        string path = Application.dataPath + "/bonepile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            // Enable TypeNameHandling to deserialize with type information
            BonePile bonePile = JsonConvert.DeserializeObject<BonePile>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            
            if (bonePile != null && bonePile.level == level.levelNumber)
            {
                // Load the bone pile data into the game
                // For example, you can instantiate the bone pile prefab and set its properties
                GameObject bonePilePrefab = Resources.Load<GameObject>("Prefabs/BonePilePrefab"); // Replace with your actual prefab path
                GameObject bonePileInstance = Instantiate(bonePilePrefab, new Vector3(bonePile.xPos, bonePile.yPos, 0), Quaternion.identity);
                Bonepile bonepile = bonePileInstance.GetComponent<Bonepile>();
                bonepile.loot = bonePile.deck;
                entities.Add(bonepile); // Add the bone pile to the entities list
            }
            else
            {
                Debug.Log("No bone pile data found for level " + level.levelNumber);
            }
        }
    }

    public async Task LoadDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card card = deck[i];
            List<Card> hand = inventoryManager.pileController.hand;
            if (card.itemType != ItemType.Default && card.itemType != ItemType.Item && card.itemType != ItemType.Arrow)
            {
                await inventoryManager.pileController.AddCard(card);
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
                    await inventoryManager.pileController.AddCard(card);
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

        foreach (Card card in deck)
        {
            card.count = 1; // Reset count to 1 for serialization
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
                xPos = entity.transform.position.x,
                yPos = entity.transform.position.y,
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
