using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLogic
{
    public static async Task PlayerTurnStart(CombatManager combatManager)
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
        await CombatMechanics.ApplyStatusEffects(combatManager.playerDetails, combatManager.enemyDetails);
        await Draw(combatManager);

        // Accessory effect is applied at the start of the player's turn
        if (combatManager.pileController.hand.OfType<Accessory>().Any())
        {
            var accessory = combatManager.pileController.hand.OfType<Accessory>().FirstOrDefault();
            if (accessory != null && accessory is not TempAccessory)
            {
                Card card = accessory as Card;
                await AnimationController.TriggerPlayerAction(card);
                await accessory.AccessoryEffect(combatManager.playerDetails, combatManager.enemyDetails);
            }
        }
    }

    public static void ReturnCards(CombatManager combatManager)
    {
        // Return all cards in the player's hand to the deck
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card.itemType == ItemType.Item || card.itemType == ItemType.Arrow)
            {
                combatManager.playerDetails.deck.Add(card);
            }
            else
            {
                switch (card.itemType)
                {
                    case ItemType.Shield:
                        combatManager.playerDetails.gear[CombatDetails.Shield] = card;
                        break;
                    case ItemType.Helmet:
                        combatManager.playerDetails.gear[CombatDetails.Helmet] = card;
                        break;
                    case ItemType.Chestpiece:
                        combatManager.playerDetails.gear[CombatDetails.Chestpiece] = card;
                        break;
                    case ItemType.Boots:
                        combatManager.playerDetails.gear[CombatDetails.Boots] = card;
                        break;
                    case ItemType.Accessory:
                        combatManager.playerDetails.gear[CombatDetails.Accessory] = card;
                        break;
                    case ItemType.Bow:
                        combatManager.playerDetails.gear[CombatDetails.Bow] = card;
                        break;
                    default:
                        Debug.LogWarning("Unknown item type: " + card.itemType);
                        break;
                }
            }
        }

        PurgeTempCards(combatManager); // Remove temporary cards from the player's deck
    }

    public static void PurgeTempCards(CombatManager combatManager)
    {
        foreach (Card card in combatManager.playerDetails.deck)
        {
            if (
                card is TempAccessory || 
                card is TempWeapon    || 
                card is TempBow       || 
                card is TempShield    ||
                card is TempHelm      ||
                card is TempChest     ||
                card is TempBoots
                )
            {
                // Remove the temporary card from the player's deck
                Debug.Log($"Removing temporary card: {card.cardName}");
                combatManager.playerDetails.deck.Remove(card);
                break; // Exit the loop after removing the card
            }
        }

        for (int i = 0; i < combatManager.playerDetails.gear.Length; i++)
        {
            Card card = combatManager.playerDetails.gear[i];
            if (card != null && 
                card is TempAccessory || 
                card is TempWeapon    || 
                card is TempBow       || 
                card is TempShield    ||
                card is TempHelm      ||
                card is TempChest     ||
                card is TempBoots
                )
            {
                // Remove the temporary card from the player's gear
                Debug.Log($"Removing temporary gear: {card.cardName}");
                combatManager.playerDetails.gear[i] = null; // Set the gear slot to null
            }
        }
    }

    public static async Task Draw(CombatManager combatManager){
        // For each consumable item type in players inventory, draw one
        List<Card> newCards = new List<Card>();
        for (int i = 0; i < combatManager.playerDetails.deck.Count; i++)
        {
            Card newCard = combatManager.playerDetails.deck[i];
            // Check if a card with the same name already exists in newCards
            if (newCard.itemType == ItemType.Item && !newCards.Exists(card => card.cardName == newCard.cardName))
            {
                await combatManager.pileController.AddCard(newCard);
                newCards.Add(newCard);
            }
            else if (newCard.itemType == ItemType.Arrow && !newCards.Exists(card => card.cardName == newCard.cardName))
            {
                await combatManager.pileController.AddCard(newCard);
                newCards.Add(newCard);
            }
        }

        for (int i = 0; i < newCards.Count; i++)
        {
            combatManager.playerDetails.deck.Remove(newCards[i]);
        }
    }
}
