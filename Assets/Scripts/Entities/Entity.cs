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
        This.MovePosition(new Vector2(position.x, position.y)); // Move the entity to the new position
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


    public virtual Entity CheckView()
    {
        return null; // Placeholder for view check logic
    }
}
