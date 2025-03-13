using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Enemy : Entity
{
    public Player player;
    public override void SetDefaults()
    {
        Debug.Log("Setting default enemy values");
        player = GameObject.Find("Player").GetComponent<Player>();
        facing = Vector2Int.down;
        viewDistance = 5;
    }
    public override void Interact()
    {
        Debug.Log("Attacking player");
        levelManager.attacker = this;
        levelManager.SaveLevel();
        SceneManager.LoadScene("CombatScene");
    }
    public void Death()
    {
        Debug.Log("Enemy has died");
        // Remove enemy from game.
    }
    public void Update()
    {
        Debug.Log("Updating enemy");
        // Update enemy position and state.
        Entity target = CheckView();
        if (target == player)
        {
            Interact();
        }
    }
}
