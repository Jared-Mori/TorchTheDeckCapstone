using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class TurnManager
{
    public static async Task CombatStart(CombatManager combatManager)
    {
        await EnemyLogic.SetEnemyDeck(combatManager.enemyDetails);
        for (int i = 0; i < combatManager.playerDetails.gear.Length; i++)
        {
            if (combatManager.playerDetails.gear[i] != null)
            {
                await combatManager.pileController.AddCard(combatManager.playerDetails.gear[i]);
                combatManager.playerDetails.gear[i] = null;
            }
            else
            {
                // Add a placeholder card if no gear is equipped in this slot
                Card placeholder = CreatePlaceholderForSlot(i);
                await combatManager.pileController.AddCard(placeholder);
            }
        }
        await PlayerLogic.PlayerTurnStart(combatManager);
    }

    public static void CombatEnd(CombatManager combatManager)
    {
        // Handle player victory logic here
        Debug.Log("Player has won the battle!");
        PlayerLogic.ReturnCards(combatManager);
        EnemyLogic.RewardPlayer(combatManager);
        Debug.Log("Player has received rewards!");
        StatTracker.IncrementEnemiesKilled(); // Increment the enemies killed

        // Return to Exploration or next level
        combatManager.SaveLevel();
        DOTween.KillAll(); // Stop all tweens to prevent any lingering animations
        SceneManager.LoadScene("ExplorationScene"); // Load the Exploration scene or next level
    }

    public static void Defeat(CombatManager cm)
    {
        // Handle player defeat logic here
        Debug.Log("Player has been defeated!");
        int xPos = 0;
        int yPos = 0;

        foreach (EntityData entity in cm.entityDataArray)
        {
            if (entity.entityType == EntityType.Player)
            {
                xPos = entity.xPos;
                yPos = entity.yPos;
                break;
            }
        }
        // Create a BonePile at the player's last position
        BonePile bonePile = new BonePile(cm.level, xPos, yPos, cm.playerDetails.deck, cm.playerDetails.gear);
        
        string json = JsonConvert.SerializeObject(bonePile, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() },
            NullValueHandling = NullValueHandling.Include // Include null values
        });

        string path = Application.dataPath + "/bonepile.json";
        File.WriteAllText(path, json);
        DOTween.KillAll(); // Stop all tweens to prevent any lingering animations
        Image fadeImage = GameObject.Find("FadeToBlack").GetComponent<Image>();
        fadeImage.DOFade(1, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("GameOverScene"); // Load the Game Over scene
        });

        
    }

    public static async Task StartEnemyTurn(CombatManager cm)
    {
        switch (cm.enemyDetails.entityType)
        {
            case EntityType.Slime:
                await EnemyLogic.SlimeLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Goblin:
                await EnemyLogic.GoblinLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.SkeletonArcher:
                await EnemyLogic.SkeletonArcherLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.SkeletonSword:
                await EnemyLogic.SkeletonSwordLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Vampire:
                await EnemyLogic.VampireLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Werewolf:
                await EnemyLogic.WerewolfLogic(cm.enemyDetails, cm.playerDetails);
                break;
            case EntityType.Necromancer:
                await EnemyLogic.NecromancerLogic(cm.enemyDetails, cm.playerDetails);
                break;
            default:
                Debug.LogError("Unknown enemy type: " + cm.enemyDetails.entityType);
                break;
        }
        await EndTurn(cm);
    }

    public static async Task EndTurn(CombatManager cm)
    {
        if (cm.isPlayerTurn)
        {
            cm.isPlayerTurn = false;
            await StartEnemyTurn(cm);
        }
        else
        {
            cm.isPlayerTurn = true;
            await AnimationController.TurnStartAnimation();
            await PlayerLogic.PlayerTurnStart(cm);
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
