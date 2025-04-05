using UnityEngine;

public class TurnManager
{
    public static void StartPlayerTurn(CombatManager combatManager)
    {
        // Player's turn logic
        Debug.Log("Player's turn started.");
        PlayerLogic.PlayerTurnStart(combatManager);
    }

    public static void StartEnemyTurn(CombatManager cm)
    {
        switch (cm.enemyDetails.entityType)
        {
            case EntityType.Slime:
                EnemyLogic.SlimeLogic(cm.playerDetails, cm.enemyDetails);
                break;
            case EntityType.Goblin:
                EnemyLogic.GoblinLogic(cm.playerDetails, cm.enemyDetails);
                break;
            default:
                Debug.LogError("Unknown enemy type: " + cm.enemyDetails.entityType);
                break;
        }
    }

    public static void EndTurn(CombatManager cm)
    {
        if (cm.isPlayerTurn)
        {
            cm.isPlayerTurn = false;
            StartEnemyTurn(cm);
        }
        else
        {
            cm.isPlayerTurn = true;
            StartPlayerTurn(cm);
        }
    }
}
