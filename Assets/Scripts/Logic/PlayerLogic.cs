using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerLogic
{
    public static void PlayerTurnStart(CombatManager combatManager)
    {
        combatManager.playerDetails.energy = combatManager.playerDetails.energyMax;
        Draw(combatManager);
    }

    public static void CombatStart(CombatManager combatManager)
    {
        for (int i = 0; i < combatManager.playerDetails.gear.Length; i++)
        {
            if (combatManager.playerDetails.gear[i] != null)
            {
                combatManager.pileController.AddCard(combatManager.playerDetails.gear[i]);
                combatManager.playerDetails.gear[i] = null;
            }
        }
        PlayerTurnStart(combatManager);
    }

    public static void CombatEnd(CombatManager combatManager)
    {
        // Handle player victory logic here
        Debug.Log("Player has won the battle!");
        // Give rewards, update inventory, etc.

        // Return to Exploration or next level
        combatManager.ReturnToLevel();
    }

    public static void Defeat(CombatManager combatManager)
    {
        // Handle player defeat logic here
        Debug.Log("Player has been defeated!");
        // Optionally, you can load a different scene or show a game over screen
        // SceneManager.LoadScene("GameOverScene");
    }

    public static void Draw(CombatManager combatManager){
        // For each consumable item type in players inventory, draw one
        List<Card> newCards = new List<Card>();
        for (int i = 0; i < combatManager.playerDetails.deck.Count; i++)
        {
            Card newCard = combatManager.playerDetails.deck[i];
            // Check if a card with the same name already exists in newCards
            if (newCard.itemType == ItemType.Item && !newCards.Exists(card => card.cardName == newCard.cardName))
            {
                combatManager.pileController.AddCard(newCard);
                newCards.Add(newCard);
            }
        }

        for (int i = 0; i < newCards.Count; i++)
        {
            combatManager.playerDetails.deck.Remove(newCards[i]);
        }
    }
}
