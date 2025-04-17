using UnityEngine;

public class TurnManager
{
    public static void CombatStart(CombatManager combatManager)
    {
        // for (int i = 0; i < combatManager.playerDetails.gear.Length; i++)
        // {
        //     if (combatManager.playerDetails.gear[i] != null)
        //     {
        //         combatManager.pileController.AddCard(combatManager.playerDetails.gear[i]);
        //         combatManager.playerDetails.gear[i] = null;
        //     }
        //     else
        //     {
        //         // Add a placeholder card if no gear is equipped in this slot
        //         Card placeholder = CreatePlaceholderForSlot(i);
        //         combatManager.pileController.AddCard(placeholder);
        //     }
        // }
        // PlayerTurnStart(combatManager);
        Tests.AddAllCardsToHand(combatManager);
        EnemyLogic.SetEnemyDeck(combatManager.enemyDetails);
    }

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

    private static Card CreatePlaceholderForSlot(int slotIndex)
    {
        switch (slotIndex)
        {
            case CombatDetails.Helmet:
                return new TempHelm();
            case CombatDetails.Chestpiece:
                return new TempChest();
            case CombatDetails.Boots:
                return new TempBoots();
            case CombatDetails.Shield:
                return new TempShield();
            case CombatDetails.Accessory:
                return new TempAccessory(); // Placeholder for accessory
            case CombatDetails.Weapon:
                return new TempWeapon();
            case CombatDetails.Bow:
                return new TempBow();
            default:
                Debug.LogWarning($"Unknown gear slot index: {slotIndex}");
                return null;
        }
    }
}
