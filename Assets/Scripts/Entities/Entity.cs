using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public enum Direction { Up, Down, Left, Right }
[System.Serializable]
public enum EntityType { 
    Player, 
    Chest, Door, Rock, Bonepile, // static entities

    // Enemy types divided by tier
    Slime, Goblin, // Tier 1
    SkeletonArcher, SkeletonSword, // Tier 2
    Vampire, Werewolf, // Tier 3
    Necromancer // Tier 4
}


public class Entity : MonoBehaviour
{
    public int viewDistance = 0;
    public static int MOVESPEED = 2;
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
        Debug.Log("Setting position of " + entityType + " to " + position);
        This.MovePosition((UnityEngine.Vector2)levelManager.level.GetFloor().CellToWorld(position));
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

    public void Move(Vector2 input)
    {
        Vector2 normalizedInput = input.normalized;

        if (normalizedInput.x > 0) facing = Direction.Right;
        else if (normalizedInput.x < 0) facing = Direction.Left;

        if (facing == Direction.Right)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (facing == Direction.Left)
        {
            spriteRenderer.flipX = true; // Face left
        }
        This.linearVelocity = normalizedInput * MOVESPEED; // Adjust speed as needed
    }


    public Entity CheckView()
    {
        // Get the direction the entity is facing
        Vector2 direction = Directions[facing];

        // Offset the boxcast origin slightly in the direction of the ray
        Vector2 boxOrigin = (Vector2)transform.position + direction * 0.1f;

        // Define the size of the box (width and height)
        Vector2 boxSize = new Vector2(1f, 1f); // Adjust these values to increase the detection area

        // Define a layer mask to exclude the Player and Walls layers
        int layerMask = ~LayerMask.GetMask("Player", "Wall");

        // Perform the boxcast
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, direction, viewDistance, layerMask);

        // Debug the boxcast
        Debug.DrawRay(boxOrigin, direction * viewDistance, Color.red, 0.1f); // Visualize the direction
        Debug.DrawLine(boxOrigin - boxSize / 2, boxOrigin + boxSize / 2, Color.green, 0.1f); // Visualize the box

        // Check if the boxcast hit something
        if (hit.collider != null)
        {
            Debug.Log($"BoxCast hit: {hit.collider.gameObject.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            Entity hitEntity = hit.collider.GetComponent<Entity>();
            if (hitEntity != null)
            {
                Debug.Log($"{hitEntity.entityType} detected at {hit.point}");
                return hitEntity; // Return the detected entity
            }
            else
            {
                Debug.Log("Hit object does not have an Entity component");
            }
        }

        Debug.Log("No entity detected in view");
        return null; // No entity detected
    }
}
