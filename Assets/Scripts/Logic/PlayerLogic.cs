using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerLogic
{
    public static void PlayerTurnStart(CombatManager combatManager)
    {
        // If player used shield but it was never triggered, regain durability.
        if(combatManager.playerDetails.isShielded)
        {
            combatManager.playerDetails.isShielded = false;
            PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
            var card = pc.GetEquippedCard(ItemType.Shield);
            card.card.uses++;
        }


        combatManager.playerDetails.energy = combatManager.playerDetails.energyMax;
        CombatMechanics.ApplyStatusEffects(combatManager.playerDetails, combatManager.enemyDetails);
        Draw(combatManager);

        // Accessory effect is applied at the start of the player's turn
        if (combatManager.playerDetails.gear[CombatDetails.Accessory] != null)
        {
            combatManager.playerDetails.gear[CombatDetails.Helmet].Effect(combatManager.playerDetails, combatManager.enemyDetails);
        }
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
