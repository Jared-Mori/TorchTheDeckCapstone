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
        Debug.Log("Target: " + target);
        if (target == levelManager.playerInstance)
        {
            Interact();
        }
    }

    public void Walk()
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
}