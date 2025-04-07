using UnityEngine;

public class PlayerLogic
{
    public static void PlayerTurnStart(CombatManager combatManager)
    {
        // For each consumable item type in players inventory, draw one
        for (int i = 0; i < combatManager.playerDetails.deck.Count; i++)
        {
            if (combatManager.playerDetails.deck[i].isStackable && combatManager.playerDetails.deck[i].count > 0)
            {
                combatManager.pileController.AddCard(combatManager.playerDetails.deck[i]);
                combatManager.playerDetails.deck[i].RemoveCard(combatManager.playerDetails.deck);
            }
        }
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
    }

    public static void CombatEnd(CombatManager combatManager)
    {
        for (int i = 0; i < combatManager.pileController.hand.Count; i++)
        {
            if (combatManager.pileController.hand[i].itemType == ItemType.Item)
            {
                combatManager.pileController.hand[i].AddToDeck(combatManager.playerDetails.deck);
            }
            else
            {
                Card equipmentCard = combatManager.pileController.hand[i];
                switch (equipmentCard.itemType)
                {
                    case ItemType.Helmet:
                        combatManager.playerDetails.gear[CombatDetails.Helmet] = equipmentCard;
                        break;
                    case ItemType.Chestpiece:
                        combatManager.playerDetails.gear[CombatDetails.Chestpiece] = equipmentCard;
                        break;
                    case ItemType.Boots:
                        combatManager.playerDetails.gear[CombatDetails.Boots] = equipmentCard;
                        break;
                    case ItemType.Shield:
                        combatManager.playerDetails.gear[CombatDetails.Shield] = equipmentCard;
                        break;
                    case ItemType.Accessory:
                        combatManager.playerDetails.gear[CombatDetails.Accessory] = equipmentCard;
                        break;
                    case ItemType.Weapon:
                        combatManager.playerDetails.gear[CombatDetails.Weapon] = equipmentCard;
                        break;
                    case ItemType.Bow:
                        combatManager.playerDetails.gear[CombatDetails.Bow] = equipmentCard;
                        break;
                    default:
                        Debug.LogError("Default Item type: " + equipmentCard.itemType);
                        break;
                }
            }
        }
    }
}
