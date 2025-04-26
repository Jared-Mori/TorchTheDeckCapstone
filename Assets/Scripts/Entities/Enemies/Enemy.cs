using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Enemy : Entity
{
    private float walkBuffer = 0.5f;
    private float walkCooldownTimer = 0f;

    public override void SetDefaults()
    {
        Debug.Log("Setting default enemy values");
        facing = Direction.Down;
        viewDistance = 5;
    }

    public override void Interact()
    {
        Debug.Log("Attacking player");
        this.isAttacker = true;
        levelManager.SaveLevel();
        DOTween.KillAll(); // Stop all tweens to prevent any lingering animations
        SceneManager.LoadScene("CombatScene");
    }

    public void Update()
    {
        if (walkCooldownTimer > 0)
        {
            walkCooldownTimer -= Time.deltaTime;
        }

        if (walkCooldownTimer <= 0)
        {
            Walk(); // Perform semi-random movement
            walkCooldownTimer = walkBuffer;
        }

        Entity target = CheckView();
        if (target == levelManager.playerInstance)
        {
            //Interact();
        }
    }

    public void Walk()
    {
        // Choose a random direction
        Direction randomDirection = (Direction)Random.Range(0, 4); // 0 = Up, 1 = Down, 2 = Left, 3 = Right
        Vector3Int nextPosition = gridPosition + new Vector3Int(Directions[randomDirection].x, Directions[randomDirection].y, 0);

        // Check if the next position is walkable (not a wall)
        if (IsWalkable(nextPosition))
        {
            facing = randomDirection;
            Move(); // Move to the next position
        }
        else
        {
            Debug.Log("Blocked by wall, choosing a new direction");
        }
    }

    private bool IsWalkable(Vector3Int position)
    {
        // Check if the position is not a wall
        if (levelManager.level.GetWalls().HasTile(position))
        {
            return false; // Position is blocked by a wall
        }

        // Check if the position is not a rock
        if (levelManager.level.GetRocks().HasTile(position))
        {
            return false; // Position is blocked by a rock
        }

        // Check if the position is not an object
        if (levelManager.level.GetObjects().HasTile(position))
        {
            return false; // Position is blocked by an object
        }

        // Check if the position is not a chest
        if (levelManager.level.GetChests().HasTile(position))
        {
            return false; // Position is blocked by a chest
        }

        // Check if the position is not an enemy
        if (levelManager.level.GetEnemies().HasTile(position))
        {
            return false; // Position is blocked by an enemy
        }

        // If none of the above checks block the position, it is walkable
        return true;
    }
}