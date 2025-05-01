using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tests
{
    public static async Task AddAllCardsToHand(CombatManager combatManager)
    {
        List<Card> newCards = new List<Card>();

        // Cards to be added to the hand
        newCards.Add(new Longbow());
        newCards.Add(new WoodArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new PoisonArrow());
        newCards.Add(new Stone());
        newCards.Add(new AdamantiteSword());
        newCards.Add(new ThornedShield());
        newCards.Add(new FalconSabatons());
        newCards.Add(new ArchmagesGall());
        newCards.Add(new DragonKingsCuriass());

        for (int i = 0; i < newCards.Count; i++)
        {
            await combatManager.pileController.AddCard(newCards[i]);
        }
    }

    public static async Task AddAllCardsToDeck(PileController pc)
    {
        List<Card> newCards = new List<Card>();

        // Cards to be added to the deck
        newCards.Add(new ThornedShield());
        newCards.Add(new FalconSabatons());
        newCards.Add(new ArchmagesGall());
        newCards.Add(new DragonKingsCuriass());
        newCards.Add(new DragonKingsScale());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new ObsidianBlade());
        newCards.Add(new CrimsonCutter());
        newCards.Add(new CrimsonCutter());
        newCards.Add(new CrimsonCutter());
        newCards.Add(new CrimsonCutter());
        newCards.Add(new VitalityDraught());
        newCards.Add(new VitalityDraught());
        newCards.Add(new VitalityDraught());
        newCards.Add(new VitalityDraught());
        newCards.Add(new VitalityDraught());
        newCards.Add(new VitalityDraught());
        newCards.Add(new StaminaDraught());
        newCards.Add(new StaminaDraught());
        newCards.Add(new StaminaDraught());
        newCards.Add(new StaminaDraught());

        for (int i = 0; i < newCards.Count; i++)
        {
            await pc.AddCard(newCards[i]);
        }
    }
}
