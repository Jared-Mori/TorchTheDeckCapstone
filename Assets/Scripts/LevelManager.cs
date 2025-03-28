using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Newtonsoft.Json;

public enum EntityType { Player, Chest, Door, Slime, Goblin, Skeleton, Orc, Dragon }
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
    

    void Start()
    {
        DeleteSaveFile();
        Application.targetFrameRate = FRAMERATE;

        // Instantiate the Grid prefab
        gridInstance = Instantiate(gridPrefab);
        walls = gridInstance.transform.Find("Walls").GetComponent<Tilemap>();
        floor = gridInstance.transform.Find("Floor").GetComponent<Tilemap>();
        
        LoadLevel();
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

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            this.level = saveData.level;
            EntityData[] entityDataArray = JsonConvert.DeserializeObject<EntityData[]>(saveData.entityDataArray.ToString());

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

        EntityData[] entityDataArray = SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = this.level,
            entityDataArray = entityDataArray
        };

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

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
                gear = entity.gear,
                deck = entity.deck,
                isAttacker = entity.isAttacker,
                entityType = entity.entityType
            };
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
                    entity = playerInstance;
                    break;
                case EntityType.Chest:
                    entity = Instantiate(chestPrefab).GetComponent<Chest>();
                    break;
                case EntityType.Slime:
                    entity = Instantiate(slimePrefab).GetComponent<Slime>();
                    break;
            }

            if (entity != null)
            {
                entity.facing = data.facing;
                entity.health = data.health;
                entity.gear = data.gear;
                entity.deck = data.deck;
                entity.isAttacker = data.isAttacker;

                entity.SetPosition(new Vector3Int(data.xPos, data.yPos, 0));
                entity.SetLevelManager(this);

                entities.Add(entity);
            }
        }
    }
}
