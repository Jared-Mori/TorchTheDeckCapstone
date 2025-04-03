using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Enemy : Entity
{
    public Queue<Direction> queue;
    public List<Direction> path;
    private float walkBuffer = 0.5f;
    private float walkCooldownTimer = 0f;
    public override void SetDefaults()
    {
        Debug.Log("Setting default enemy values");
        facing = Direction.Down;
        viewDistance = 5;
        queue = new Queue<Direction>();
        foreach (Direction dir in path)
        {
            queue.Enqueue(dir);
        }
    }
    public override void Interact()
    {
        Debug.Log("Attacking player");
        this.isAttacker = true;
        levelManager.SaveLevel();
        SceneManager.LoadScene("CombatScene");
    }
    public void Death()
    {
        Debug.Log("Enemy has died");
        levelManager.entities.Remove(this);
        levelManager.SaveLevel();
        Destroy(gameObject);
    }
    public void Update()
    {
        if (walkCooldownTimer > 0)
        {
            walkCooldownTimer -= Time.deltaTime;
        }

        if (walkCooldownTimer <= 0)
        {
            Walk();
            walkCooldownTimer = walkBuffer;
        }

        Entity target = CheckView();
        if (target == levelManager.playerInstance)
        {
            Interact();
        }
    }

    public void Walk()
    {
        // if queue is empty, refill it with path
        if (queue.Count == 0)
        {
            foreach (Direction dir in path)
            {
                queue.Enqueue(dir);
            }
        }

        Direction next = queue.Dequeue();
        facing = next;
        Move();
    }
}