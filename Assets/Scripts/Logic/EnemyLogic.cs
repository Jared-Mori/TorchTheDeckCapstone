using System.Linq;
using UnityEngine;

public class EnemyLogic
{
    public static void EnemyTurnStart(CombatDetails enemy, CombatDetails player)
    {
        // Reset enemy energy or any other properties if needed
        enemy.energy = enemy.energyMax;
        CombatMechanics.ApplyStatusEffects(enemy, player);
    }

    public static void SetEnemyDeck(CombatDetails entity)
    {
        switch(entity.entityType)
        {
            case EntityType.Slime:
                // Slimes have 5 Slime Burst cards in their deck
                for (int i = 0; i < 5; i++)
                {
                    SlimeBurst slime = new SlimeBurst();
                    entity.deck.Add(slime);
                }
                break;
            case EntityType.Goblin:
                HealthPotion healthPotion = new HealthPotion();
                entity.deck.Add(healthPotion); // Add a health potion to the goblin's deck
                for (int i = 0; i < 3; i++)
                {
                    GoblinDagger dagger = new GoblinDagger();
                    entity.deck.Add(dagger); // Add a goblin dagger to the goblin's deck
                }
                for (int i = 0; i < 10; i++)
                {
                    Stone stone = new Stone();
                    entity.deck.Add(stone); // Add a stone to the goblin's deck
                }
                break;
            case EntityType.SkeletonArcher:
                for (int i = 0; i < 12; i++)
                {
                    SkeletonArrow arrow = new SkeletonArrow();
                    entity.deck.Add(arrow);
                }
                for (int i = 0; i < 2; i++)
                {
                    SkeletonPoisonArrow sword = new SkeletonPoisonArrow();
                    entity.deck.Add(sword);
                }
                for (int i = 0; i < 24; i++)
                {
                    RibBone rib = new RibBone();
                    entity.deck.Add(rib);
                }
                break;
            case EntityType.SkeletonSword:
                AdamantiteSword adamantiteSword = new AdamantiteSword();
                entity.deck.Add(adamantiteSword); // Add a sword to the skeleton's deck
                IronSword ironSword = new IronSword();
                entity.deck.Add(ironSword); // Add a sword to the skeleton's deck
                entity.gear[CombatDetails.Helmet] = new IronHelmet();
                entity.gear[CombatDetails.Shield] = new Shield();
                break;
            case EntityType.Vampire:
                // Add Vampire specific cards to the deck
                break;
            case EntityType.Werewolf:
                // Add Werewolf specific cards to the deck
                break;
            case EntityType.Necromancer:
                // Add Necromancer specific cards to the deck
                break;
        }
    }

    public static void SlimeLogic(CombatDetails slime, CombatDetails player)
    {
        // Slime logic goes here
        Debug.Log("Slime logic executed.");
        EnemyTurnStart(slime, player);
        Card card = null;
        for (int i = 0; i < slime.energy; i++)
        {
            card = new SlimeBurst();
            card.Effect(slime, player); // Use the card effect
            if(card.Use())
            {
                slime.deck.Remove(card); // Remove the card from the deck if it has been used
            }
        }
    }

    public static void GoblinLogic(CombatDetails goblin, CombatDetails player)
    {
        // Goblin logic goes here
        Debug.Log("Goblin logic executed.");
        EnemyTurnStart(goblin, player);
        Card card = null;
        while (goblin.energy > 0)
        {
            if((goblin.healthMax - goblin.health >= 5) && goblin.deck.OfType<HealthPotion>().Any())
            {
                card = new HealthPotion();
                card.Effect(goblin, player); // Use the health potion effect
                CombatMechanics.UseEnergy(goblin, 1); // Use 1 energy for the potion
            }
            else if (goblin.deck.OfType<GoblinDagger>().Any())
            {
                card = goblin.deck.OfType<GoblinDagger>().FirstOrDefault();
                card.Effect(goblin, player);
                CombatMechanics.UseEnergy(goblin, 1);
            }
            else if (goblin.deck.OfType<Stone>().Any())
            {
                card = goblin.deck.OfType<Stone>().FirstOrDefault();
                card.Effect(goblin, player);
                CombatMechanics.UseEnergy(goblin, 1);
            }
            else
            {
                Debug.Log("Goblin has no valid cards to play.");
                break; // Exit the loop if no valid cards are available
            }
            if(card.Use())
            {
                goblin.deck.Remove(card); // Remove the card from the deck if it has been used
            }
        }

    }

    public static void SkeletonArcherLogic(CombatDetails skeleton, CombatDetails player)
    {
        // Skeleton archer logic goes here
        Debug.Log("Skeleton archer logic executed.");
        EnemyTurnStart(skeleton, player);
        Card card = null;
        for (int i = 0; i < skeleton.energy; i++)
        {
            int randomIndex = Random.Range(0, 5); // 20% chance to use a poison arrow if available
            if (skeleton.deck.OfType<SkeletonPoisonArrow>().Any() && randomIndex == 0)
            {
                card = skeleton.deck.OfType<SkeletonPoisonArrow>().FirstOrDefault();
                card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the poison arrow
            }
            else if (skeleton.deck.OfType<SkeletonArrow>().Any())
            {
                card = skeleton.deck.OfType<SkeletonArrow>().FirstOrDefault();
                card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the arrow
            }
            else if (skeleton.deck.OfType<RibBone>().Any())
            {
                card = skeleton.deck.OfType<RibBone>().FirstOrDefault();
                card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the rib bone
            }
            else
            {
                Debug.Log("Skeleton archer has no valid cards to play.");
                break; // Exit the loop if no valid cards are available
            }
            if(card.Use())
            {
                skeleton.deck.Remove(card); // Remove the card from the deck if it has been used
            }
        }
    }

    public static void SkeletonSwordLogic(CombatDetails skeleton, CombatDetails player)
    {
        // Skeleton sword logic goes here
        Debug.Log("Skeleton sword logic executed.");
        EnemyTurnStart(skeleton, player);
        Card card = null;
        for (int i = 0; i < skeleton.energy; i++)
        {
            int randomIndex = Random.Range(0, 5); // 20% chance to shield if available
            if (skeleton.deck.OfType<Shield>().Any() && randomIndex <= 1)
            {
                card = skeleton.deck.OfType<Shield>().FirstOrDefault();
                card.Effect(skeleton, player);
                CombatMechanics.UseEnergy(skeleton, 1);
            }
            else if (skeleton.deck.OfType<AdamantiteSword>().Any())
            {
                card = skeleton.deck.OfType<AdamantiteSword>().FirstOrDefault();
                card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the arrow
            }
            else if (skeleton.deck.OfType<IronSword>().Any())
            {
                card = skeleton.deck.OfType<IronSword>().FirstOrDefault();
                card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the rib bone
            }
            else
            {
                Debug.Log("Skeleton archer has no valid cards to play.");
                break; // Exit the loop if no valid cards are available
            }
            if(card.Use() && card.itemType != ItemType.Shield)
            {
                skeleton.deck.Remove(card); // Remove the card from the deck if it has been used
            }
        }
    }

    public static void VampireLogic(CombatDetails vampire, CombatDetails player)
    {
        // Vampire logic goes here
        Debug.Log("Vampire logic executed.");
        EnemyTurnStart(vampire, player);
    }

    public static void WerewolfLogic(CombatDetails werewolf, CombatDetails player)
    {
        // Werewolf logic goes here
        Debug.Log("Werewolf logic executed.");
        EnemyTurnStart(werewolf, player);
    }

    public static void NecromancerLogic(CombatDetails necromancer, CombatDetails player)
    {
        // Necromancer logic goes here
        Debug.Log("Necromancer logic executed.");
        EnemyTurnStart(necromancer, player);
    }
}
