using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour
{
    protected Tilemap walls, floor;
    public int level;
    private static int FRAMERATE = 30;

    public List<Entity> entities = new List<Entity>();
    public InventoryManager inventoryManager;
    public Enemy attacker;
    public Player playerPrefab;
    public Player playerInstance;

    public GameObject gridPrefab;
    public GameObject gridInstance;

    public GameObject chestPrefab;
    

    void Start()
    {
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

        string path = Application.persistentDataPath + "/levelData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            LevelManager loadedData = JsonConvert.DeserializeObject<LevelManager>(json);

            this.level = loadedData.level;
            this.entities = loadedData.entities;
            this.playerInstance = loadedData.playerInstance;

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

        string json = JsonConvert.SerializeObject(this, Formatting.Indented);

        string path = Application.persistentDataPath + "/levelData.json";

        File.WriteAllText(path, json);

        Debug.Log("Level data saved to " + path);
    }

    public void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/levelData.json";

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
        entities = new List<Entity>();

        // Ensure the player object is properly instantiated
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
        }

        playerInstance.SetLevelManager(this);
        inventoryManager.SetPlayer(playerInstance);

        GameObject chestInstance = Instantiate(chestPrefab);
        entities.Add(chestInstance.GetComponent<Chest>());
        
        foreach (Entity entity in entities)
        {
            entity.SetLevelManager(this);
        }

        Debug.Log("Initialized default level");
    }
}
