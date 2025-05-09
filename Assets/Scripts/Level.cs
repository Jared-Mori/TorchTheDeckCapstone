using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    // Level class to manage the level data and tilemaps
    public int levelNumber = 1; // Level number
    public int chestCount = 0; // Number of chests in the level
    public int rockCount = 0; // Number of rocks in the level
    public int[] enemyCount = new int[4]; // Number of enemies in the level
    string levelName = "Floor"; // Level name
    [SerializeField] private Door door; // Prefab for the player
    private GameObject floor1, floor2, floor3, floor4, floor5; // Prefab for the map
    
    [SerializeField] public Vector3Int MapOffset; // Offset for the map position
    public Tilemap walls, floor, objects, chests, enemies, rocks;
    GameObject playerSpawn;

    public void Start()
    {
        levelNumber = 1;
        floor1 = Resources.Load<GameObject>("Prefabs/Floor1");
        floor2 = Resources.Load<GameObject>("Prefabs/Floor2");
        floor3 = Resources.Load<GameObject>("Prefabs/Floor3");
        floor4 = Resources.Load<GameObject>("Prefabs/Floor4");
        floor5 = Resources.Load<GameObject>("Prefabs/Floor5");
    }
    public void SetLevelData(int levelNumber)
    {
        this.levelNumber = levelNumber;
        levelName = "Floor" + levelNumber;

        switch (levelNumber)
        {
            case 1:
                chestCount = 1;
                rockCount = 3;
                break;
            case 2:
                chestCount = 0;
                rockCount = 4;
                enemyCount[0] = 1; // 2 Tier 1 enemies
                break;
            case 3:
                chestCount = 3;
                rockCount = 6;
                enemyCount[0] = 3; // 3 Tier 1 enemies
                enemyCount[1] = 2; // 2 Tier 2 enemies
                break;
            case 4:
                chestCount = 4;
                rockCount = 10;
                enemyCount[0] = 2; // 2 Tier 1 enemies
                enemyCount[1] = 5; // 3 Tier 2 enemies
                enemyCount[2] = 3; // 1 Tier 3 enemies
                break;
            case 5:
                rockCount = 7;
                chestCount = 4;
                enemyCount[0] = 0;
                enemyCount[1] = 0;
                enemyCount[2] = 0;
                enemyCount[3] = 1; // 1 Tier 4 enemies
                break;
            default:
                Debug.LogError("Invalid level number: " + levelNumber);
                break;
        }
    }
    public void SpawnMap(int floorNum)
    {
        GameObject map = null;
        switch (floorNum)
        {
            case 1:
                map = Instantiate(floor1, MapOffset, Quaternion.identity, transform);
                break;
            case 2:
                map = Instantiate(floor2, MapOffset, Quaternion.identity, transform);
                break;
            case 3:
                map = Instantiate(floor3, MapOffset, Quaternion.identity, transform);
                break;
            case 4:
                map = Instantiate(floor4, MapOffset, Quaternion.identity, transform);
                break;
            case 5:
                map = Instantiate(floor5, MapOffset, Quaternion.identity, transform);
                break;
            default:
                Debug.LogError("Invalid floor number: " + floorNum);
                break;
        }
        walls = map.transform.Find("Walls").GetComponent<Tilemap>();
        floor = map.transform.Find("Floor").GetComponent<Tilemap>();
        objects = map.transform.Find("BeforePlayer1").GetComponent<Tilemap>();
        chests = map.transform.Find("ChestSpawns").GetComponent<Tilemap>();
        enemies = map.transform.Find("EnemySpawns").GetComponent<Tilemap>();
        rocks = map.transform.Find("Rocks").GetComponent<Tilemap>();
        playerSpawn = map.transform.Find("PlayerSpawn").gameObject;
    }

    public void SetPlayerPosition(Player player)
    {
        Debug.Log("Setting player position to " + playerSpawn.transform.position);
        player.transform.position = playerSpawn.transform.position;
    }

    public Tilemap GetFloor()
    {
        return floor;
    }

    public Tilemap GetWalls()
    {
        return walls;
    }
    public Tilemap GetObjects()
    {
        return objects;
    }

    public Tilemap GetChests()
    {
        return chests;
    }
    public Tilemap GetEnemies()
    {
        return enemies;
    }
    public Tilemap GetRocks()
    {
        return rocks;
    }
}
