using System.Collections.Generic;
using UnityEngine;

public class Tests
{
    public static void AddAllCardsToHand(CombatManager combatManager)
    {
        List<Card> newCards = new List<Card>();

        // Cards to be added to the hand
        newCards.Add(new Longbow());
        newCards.Add(new WoodArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new Stone());

        for (int i = 0; i < newCards.Count; i++)
        {
            combatManager.pileController.AddCard(newCards[i]);
        }
    }
}
