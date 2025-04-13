using UnityEngine;

public class EnemyLogic
{
    public static void EnemyTurnStart(CombatDetails playerDetails, CombatDetails enemyDetails)
    {
        // Reset enemy energy or any other properties if needed
        enemyDetails.energy = enemyDetails.energyMax;
        CombatMechanics.ApplyStatusEffects(enemyDetails, playerDetails);
    }
    public static void SlimeLogic(CombatDetails playerDetails, CombatDetails enemyDetails)
    {
        // Slime logic goes here
        Debug.Log("Slime logic executed.");
        EnemyTurnStart(playerDetails, enemyDetails);

        // Code for testing the cards
        Stone stone = new Stone();
        enemyDetails.deck.Add(stone);
        Debug.Log("Stone card added to enemy deck.");
        enemyDetails.deck.Add(stone);
        Debug.Log("Stone card added to enemy deck.");
        enemyDetails.deck.Add(stone);
        Debug.Log("Stone card added to enemy deck.");
        Debug.Log("Enemy deck count: " + enemyDetails.deck.Count);

        foreach (Card card in enemyDetails.deck)
        {
            Debug.Log("Using card: " + card.cardName);
            card.Effect(enemyDetails, playerDetails);
        }
        Debug.Log("Enemy deck count after using cards: " + enemyDetails.deck.Count);
    }

    public static void GoblinLogic(CombatDetails playerDetails, CombatDetails enemyDetails)
    {
        // Goblin logic goes here
        Debug.Log("Goblin logic executed.");
        EnemyTurnStart(playerDetails, enemyDetails);
    }
}
