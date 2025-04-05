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
}
