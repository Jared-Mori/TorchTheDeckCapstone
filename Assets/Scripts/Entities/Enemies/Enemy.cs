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
    private int spawnProtection = 20; // Number of frames to wait before allowing CheckView to be called

    public override void SetDefaults()
    {
        Debug.Log("Setting default enemy values");
        facing = Direction.Down;
        viewDistance = 5;
        spawnProtection = 20; // Set spawn protection to 20 frames
    }

    public override void Interact()
    {
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

        if (spawnProtection > 0)
        {
            spawnProtection--;
            return; // Skip CheckView if spawn protection is active
        }
        Entity target = CheckView();
        if (target == levelManager.playerInstance)
        {
            // Interact();
        }
    }

    public virtual void Walk()
    {
        List<Vector2> possibleDirections = new List<Vector2>
        {
        new Vector2(0, 1),  // Up
        new Vector2(0, -1), // Down
        new Vector2(-1, 0), // Left
        new Vector2(1, 0),  // Right
        new Vector2(-1, 1), // Up-Left
        new Vector2(1, 1),  // Up-Right
        new Vector2(-1, -1), // Down-Left
        new Vector2(1, -1)  // Down-Right
        };

        Move(possibleDirections[Random.Range(0, possibleDirections.Count)]); // Move to the next position
    }

    public override Entity CheckView()
    {
        // Get the direction the entity is facing
        Vector2 direction = Directions[facing];

        // Offset the boxcast origin slightly in the direction of the ray
        Vector2 boxSize = new Vector2(1f, 1f); // Adjust to match the player's size
        Vector2 boxOrigin = (Vector2)transform.position + direction * 0.5f; // Offset by half the box size

        // Define a layer mask to exclude the Player and Walls layers
        int layerMask = ~LayerMask.GetMask("Enemy", "Wall");

        // Perform the boxcast
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, direction, viewDistance, layerMask);

        // Debug the boxcast
        Vector2 boxCorner1 = boxOrigin - boxSize / 2;
        Vector2 boxCorner2 = boxOrigin + boxSize / 2;
        Debug.DrawLine(boxCorner1, new Vector2(boxCorner1.x, boxCorner2.y), Color.green, 0.1f); // Left edge
        Debug.DrawLine(boxCorner1, new Vector2(boxCorner2.x, boxCorner1.y), Color.green, 0.1f); // Bottom edge
        Debug.DrawLine(boxCorner2, new Vector2(boxCorner1.x, boxCorner2.y), Color.green, 0.1f); // Top edge
        Debug.DrawLine(boxCorner2, new Vector2(boxCorner2.x, boxCorner1.y), Color.green, 0.1f); // Right edge

        // Check if the boxcast hit something
        if (hit.collider != null)
        {

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
        return null; // No entity detected
    }
}