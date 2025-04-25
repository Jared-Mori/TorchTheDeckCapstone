using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    // Level class to manage the level data and tilemaps
    public int levelNumber = 1; // Level number
    public int chestCount = 0; // Number of chests in the level
    public int[] enemyCount = new int[4]; // Number of enemies in the level
    string levelName = "Floor"; // Level name
    private GameObject floor1, floor2, floor3, floor4, floor5; // Prefab for the map
    [SerializeField] public Vector3Int MapOffset; // Offset for the map position
    public Tilemap walls, floor, objects, chests, enemies;

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
                break;
            case 2:
                chestCount = 0;
                break;
            case 3:
                chestCount = 3;
                enemyCount[0] = 3; // 3 Tier 1 enemies
                enemyCount[1] = 2; // 2 Tier 2 enemies
                break;
            case 4:
                chestCount = 4;
                enemyCount[0] = 2; // 2 Tier 1 enemies
                enemyCount[1] = 5; // 3 Tier 2 enemies
                enemyCount[2] = 3; // 1 Tier 3 enemies
                break;
            case 5:
                Debug.Log("Level 4");
                chestCount = 4;
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
                map = Instantiate(floor4, MapOffset, Quaternion.identity, transform);
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
    }

    public Tilemap GetWalls()
    {
        return walls;
    }

    public Tilemap GetObjects()
    {
        return objects;
    }

    public Tilemap GetFloor()
    {
        return floor;
    }
}
