using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager
{
    public static void CombatStart(CombatManager combatManager)
    {
        EnemyLogic.SetEnemyDeck(combatManager.enemyDetails);
        for (int i = 0; i < combatManager.playerDetails.gear.Length; i++)
        {
            if (combatManager.playerDetails.gear[i] != null)
            {
                combatManager.pileController.AddCard(combatManager.playerDetails.gear[i]);
                combatManager.playerDetails.gear[i] = null;
            }
            else
            {
                // Add a placeholder card if no gear is equipped in this slot
                Card placeholder = CreatePlaceholderForSlot(i);
                combatManager.pileController.AddCard(placeholder);
            }
        }
        PlayerLogic.PlayerTurnStart(combatManager);
    }

    public static void CombatEnd(CombatManager combatManager)
    {
        // Handle player victory logic here
        Debug.Log("Player has won the battle!");
        PlayerLogic.ReturnCards(combatManager);
        EnemyLogic.RewardPlayer(combatManager);
        Debug.Log("Player has received rewards!");
        foreach (Card card in combatManager.playerDetails.deck)
        {
            Debug.Log("Card in hand: " + card.cardName);
        }

        // Return to Exploration or next level
        combatManager.SaveLevel();
        DOTween.KillAll(); // Stop all tweens to prevent any lingering animations
        SceneManager.LoadScene("ExplorationScene"); // Load the Exploration scene or next level
    }

    public static void Defeat(CombatManager combatManager)
    {
        // Handle player defeat logic here
        Debug.Log("Player has been defeated!");
        SceneManager.LoadScene("GameOverScene"); // Load the Game Over scene
    }

    public static void StartEnemyTurn(CombatManager cm)
    {
        switch (cm.enemyDetails.entityType)
        {
            case EntityType.Slime:
                EnemyLogic.SlimeLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Goblin:
                EnemyLogic.GoblinLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.SkeletonArcher:
                EnemyLogic.SkeletonArcherLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.SkeletonSword:
                EnemyLogic.SkeletonSwordLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Vampire:
                EnemyLogic.VampireLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Werewolf:
                EnemyLogic.WerewolfLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Necromancer:
                EnemyLogic.NecromancerLogic(cm.enemyDetails, cm.playerDetails);
                break;
            default:
                Debug.LogError("Unknown enemy type: " + cm.enemyDetails.entityType);
                break;
        }
        EndTurn(cm);
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
            PlayerLogic.PlayerTurnStart(cm);
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
