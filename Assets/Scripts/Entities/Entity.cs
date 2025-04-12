using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; 

[System.Serializable]
public enum Direction { Up, Down, Left, Right }
public class Entity : MonoBehaviour
{
    public int viewDistance = 0;

    protected Rigidbody2D This;
    public Sprite artwork;
    public LevelManager levelManager;
    public Direction facing;
    public Vector3Int gridPosition;
    public EntityType entityType;
    public Dictionary<Direction, Vector2Int> Directions = new Dictionary<Direction, Vector2Int>{ 
        [Direction.Up] = Vector2Int.up,
        [Direction.Down] = Vector2Int.down,
        [Direction.Left] = Vector2Int.left,
        [Direction.Right] = Vector2Int.right
    };

    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;

    public bool isAttacker = false;
    public bool isLoaded = false;
    public Vector3Int loadPosition;
    public SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Test comment for commit and push testing
    void Start()
    {
        This = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaults();
    }

    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    public void SetPosition(Vector3Int position)
    {
        This.MovePosition((UnityEngine.Vector2)levelManager.GetFloor().CellToWorld(position));
        gridPosition = position;
    }

    virtual public void Interact()
    {
        Debug.Log("Interacting with entity");
    }

    virtual public void SetDefaults()
    {
        Debug.Log("Setting default entity value");
    }

    public void Move()
    {
        if (CanMove(gridPosition))
        {
            Vector3Int newPos = gridPosition + new Vector3Int(Directions[facing].x, Directions[facing].y, 0);
            SetPosition(newPos);
        }
    }

    public bool CanMove(Vector3Int cellPos)
    {
        Vector3Int targetCell = cellPos + new Vector3Int(Directions[facing].x, Directions[facing].y, 0);
        TileBase tile = levelManager.GetWalls().GetTile(targetCell);

        if (tile != null)
        {
            return false;
        }else if (levelManager.entities.Exists(entity => entity.gridPosition == targetCell))
        {
            return false;
        }else
        {
            return true;
        }
    }

    public Entity CheckView()
    {
        for (int i = 1; i <= viewDistance; i++)
        {
            Vector3Int targetCell = gridPosition + new Vector3Int(Directions[facing].x * i, Directions[facing].y * i, 0);
            foreach (Entity entity in levelManager.entities)
            {
                if (entity.gridPosition == targetCell)
                {
                    Debug.Log("Entity found. Type: " + entity.entityType);
                    return entity;
                }
            }
        }
        return null;
    }

    public void Die()
    {
        Debug.Log("Entity died");
        levelManager.entities.Remove(this);
        Destroy(this.gameObject);
    }
}
