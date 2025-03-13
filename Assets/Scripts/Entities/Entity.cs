using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; 

[System.Serializable]
public class Entity : MonoBehaviour
{
    public int viewDistance = 0;

    protected Rigidbody2D This;
    public Sprite artwork;
    protected LevelManager levelManager;
    public Vector2Int facing;
    public Vector3Int gridPosition;
    public Vector2Int[] directions = { 
        Vector2Int.up, 
        Vector2Int.down, 
        Vector2Int.left, 
        Vector2Int.right
    };

    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;

    public string entityName;
    public bool isShielded = false;
    public SpriteRenderer spriteRenderer;

    public Equipment[] gear = new Equipment[5]; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory
    public List<Card> deck = new List<Card>();

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
            Vector3Int newPos = gridPosition + new Vector3Int(facing.x, facing.y, 0);
            SetPosition(newPos);
        }
    }

    public bool CanMove(Vector3Int cellPos)
    {
        Vector3Int targetCell = cellPos + new Vector3Int(facing.x, facing.y, 0);
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
        Debug.Log("Checking view");
        for (int i = 1; i <= viewDistance; i++)
        {
            Vector3Int targetCell = gridPosition + new Vector3Int(facing.x * i, facing.y * i, 0);
            foreach (Entity entity in levelManager.entities)
            {
                if (entity.gridPosition == targetCell)
                {
                    Debug.Log("Entity found. Name: " + entity.entityName);
                    return entity;
                }
            }
        }
        Debug.Log("No entity found");
        return null;
    }

    public void Defend(int damage)
    {
        // If the entity is shielded, the damage is prevented
        if (isShielded)
        {
            isShielded = false;
            return;
        }
        // When taking damage, a random piece of equipment will be used to prevent it
        int randomValue = Random.Range(0, 3);
        if (gear[randomValue] != null)
        {
            gear[randomValue].PreventDamage();
        }else{
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Entity died");
        levelManager.entities.Remove(this);
        Destroy(this.gameObject);
    }

    public virtual void Turn()
    {
        Debug.Log("Entity's turn");
    }
}
