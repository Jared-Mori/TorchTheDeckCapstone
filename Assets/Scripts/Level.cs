using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    // Level class to manage the level data and tilemaps
    int levelNumber = 0; // Level number
    string levelName = "Floor"; // Level name
    private GameObject floor1, floor2, floor3, floor4; // Prefab for the map
    [SerializeField] public Vector3Int MapOffset; // Offset for the map position
    protected Tilemap walls, floor, objects;

    public void Start()
    {
        floor1 = Resources.Load<GameObject>("Prefabs/Floor1");
        floor2 = Resources.Load<GameObject>("Prefabs/Floor2");
        floor3 = Resources.Load<GameObject>("Prefabs/Floor3");
        if (floor1 == null || floor2 == null)
        {
            Debug.LogError("Floor prefabs not found. Ensure they exist in Resources/Prefabs.");
        }
        else
        {
            Debug.Log("Floor prefabs loaded successfully.");
        }
    }
    public void SetLevelData(int levelNumber)
    {
        this.levelNumber = levelNumber;
        levelName = "Floor" + levelNumber;
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
            default:
                Debug.LogError("Invalid floor number: " + floorNum);
                break;
        }
        walls = map.transform.Find("Walls").GetComponent<Tilemap>();
        floor = map.transform.Find("Floor").GetComponent<Tilemap>();
        objects = map.transform.Find("BeforePlayer1").GetComponent<Tilemap>();
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
