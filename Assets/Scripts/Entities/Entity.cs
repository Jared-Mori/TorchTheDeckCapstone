using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public enum Direction { Up, Down, Left, Right }
[System.Serializable]
public enum EntityType { 
    Player, 
    Chest, Door, Rock, // static entities

    // Enemy types divided by tier
    Slime, Goblin, // Tier 1
    SkeletonArcher, SkeletonSword, // Tier 2
    Vampire, Werewolf, // Tier 3
    Necromancer // Tier 4
}


public class Entity : MonoBehaviour
{
    public int viewDistance = 0;

    protected Rigidbody2D This;
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
    public Animator animator; // Reference to the Animator
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Test comment for commit and push testing
    void Start()
    {
        This = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        SetDefaults();
    }

    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    public void SetPosition(Vector3Int position)
    {
        This.DOMove((UnityEngine.Vector2)levelManager.level.GetFloor().CellToWorld(position), 1f);
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
        if (facing == Direction.Right)
        {
            spriteRenderer.flipX = true;
        }
        else if (facing == Direction.Left)
        {
            spriteRenderer.flipX = false;
        }
        Vector3Int newPos = gridPosition + new Vector3Int(Directions[facing].x, Directions[facing].y, 0);
        SetPosition(newPos);
    }


    public Entity CheckView()
    {
        // Get the direction the entity is facing
        Vector2 direction = Directions[facing];

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance);

        // Check if the raycast hit something
        if (hit.collider != null)
        {
            Entity hitEntity = hit.collider.GetComponent<Entity>();
            if (hitEntity != null)
            {
                Debug.Log($"Entity detected at {hit.point}");
                return hitEntity; // Return the detected player
            }
        }

        Debug.Log("No player detected in view");
        return null; // No player detected
    }

    public void Die()
    {
        Debug.Log("Entity died");
        levelManager.entities.Remove(this);
        Destroy(this.gameObject);
    }
}
