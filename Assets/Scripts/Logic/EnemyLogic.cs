using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyLogic
{
    public static async Task EnemyTurnStart(CombatDetails enemy, CombatDetails player)
    {
        // Reset enemy energy or any other properties if needed
        enemy.energy = enemy.energyMax;
        await CombatMechanics.ApplyStatusEffects(enemy, player);
        await Task.Delay(1000); // Simulate a delay for enemy turn start
        Debug.Log("Enemy turn started. Enemy health: " + enemy.health.ToString() + " Enemy energy: " + enemy.energy.ToString());

        if (enemy.deck.OfType<Accessory>().Any())
        {
            var accessory = enemy.deck.OfType<Accessory>().FirstOrDefault();
            if (accessory != null)
            {
                Card card = accessory as Card;
                await AnimationController.TriggerEnemyAction(card);
                await accessory.AccessoryEffect(enemy, player);
            }
        }
    }

    public static async Task SetEnemyDeck(CombatDetails entity)
    {
        Debug.Log("Setting up deck for enemy: " + entity.entityType.ToString());
        switch(entity.entityType)
        {
            case EntityType.Slime:
                // Slimes have 5 Slime Burst cards in their deck
                for (int i = 0; i < 5; i++)
                {
                    entity.deck.Add(new SlimeBurst()); // Add a Slime Burst card to the slime's deck
                }
                break;
            case EntityType.Goblin:
                entity.deck.Add(new HealthPotion()); // Add a health potion to the goblin's deck
                for (int i = 0; i < 3; i++)
                {
                    entity.deck.Add(new GoblinDagger()); // Add a goblin dagger to the goblin's deck
                }
                for (int i = 0; i < 10; i++)
                {
                    entity.deck.Add(new Stone()); // Add a stone to the goblin's deck
                }
                break;
            case EntityType.SkeletonArcher:
                for (int i = 0; i < 12; i++)
                {
                    entity.deck.Add(new SkeletonArrow()); // Add a skeleton arrow to the skeleton's deck
                }
                for (int i = 0; i < 2; i++)
                {
                    entity.deck.Add(new SkeletonPoisonArrow());
                }
                for (int i = 0; i < 24; i++)
                {
                    entity.deck.Add(new RibBone());
                }
                break;
            case EntityType.SkeletonSword:
                entity.deck.Add(new AdamantiteSword()); // Add a sword to the skeleton's deck
                entity.deck.Add(new IronSword()); // Add a sword to the skeleton's deck
                entity.gear[CombatDetails.Helmet] = new IronHelmet();
                entity.gear[CombatDetails.Shield] = new Shield();
                for (int i = 0; i < 24; i++)
                {
                    entity.deck.Add(new RibBone());
                }
                break;
            case EntityType.Vampire:
                entity.gear[CombatDetails.Chestpiece] = new VampiresRobe();
                entity.gear[CombatDetails.Boots] = new VampiresBoots();
                entity.deck.Add(new FlamingSword()); // Add a vampire bite to the vampire's deck
                entity.deck.Add(new GreatHealthPotion()); // Add a vampire bite to the vampire's deck
                break;
            case EntityType.Werewolf:
                // Add Werewolf specific cards to the deck
                entity.gear[CombatDetails.Helmet] = new WerewolfsMane();
                entity.gear[CombatDetails.Chestpiece] = new WerewolfsHide();
                entity.gear[CombatDetails.Boots] = new WerewolfsPursuit();
                entity.deck.Add(new HastePotion());
                break;
            case EntityType.Necromancer:
                // Add Necromancer specific cards to the deck
                entity.gear[CombatDetails.Helmet] = new NecromancersCrown();
                entity.gear[CombatDetails.Chestpiece] = new NecromancersCloak();
                entity.gear[CombatDetails.Boots] = new NecromancersSlippers();
                entity.gear[CombatDetails.Accessory] = new RingOfTheDead();
                entity.deck.Add(new ConjureArcaneBarrage()); // Add a necromancer card to the deck
                entity.deck.Add(new Fireball()); // Add a necromancer card to the deck
                entity.deck.Add(new Curse()); // Add a necromancer card to the deck
                entity.deck.Add(new Cleanse()); // Add a necromancer card to the deck
                entity.deck.Add(new MagicShield()); // Add a necromancer card to the deck
                entity.deck.Add(new NecroticTouch()); // Add a necromancer card to the deck
                entity.deck.Add(new GreatHealthPotion()); // Add a necromancer card to the deck
                entity.deck.Add(new GreatHealthPotion()); // Add a necromancer card to the deck
                entity.deck.Add(new GreatHealthPotion()); // Add a necromancer card to the deck
                entity.deck.Add(new SuperHealthPotion()); // Add a necromancer card to the deck
                break;
        }

        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        await pc.AdjustEnemyHand(entity);
    }

    public static async Task RewardPlayer(CombatManager cm)
    {
        // Reward the player with items or experience points
        Debug.Log("Rewarding player for defeating: " + cm.enemyDetails.entityType.ToString());
        List<Card> loot = new List<Card>();
        switch (cm.enemyDetails.entityType)
        {
            case EntityType.Slime:
                loot = LootGenerator.GenerateDrops(4, 2, 0, 0); // Generate loot for the player
                break;
            case EntityType.Goblin:
                loot = LootGenerator.GenerateDrops(5, 1, 0, 0); // Generate loot for the player
                break;
            case EntityType.SkeletonArcher:
                loot = LootGenerator.GenerateDrops(4, 3, 2, 0); // Generate loot for the player
                break;
            case EntityType.SkeletonSword:
                loot = LootGenerator.GenerateDrops(4, 3, 2, 0); // Generate loot for the player
                break;
            case EntityType.Vampire:
                loot = LootGenerator.GenerateDrops(5, 3, 2, 3); // Generate loot for the player
                break;
            case EntityType.Werewolf:
                loot = LootGenerator.GenerateDrops(7, 5, 2, 2); // Generate loot for the player
                break;
            case EntityType.Necromancer:
                loot = LootGenerator.GenerateDrops(10, 7, 5, 5); // Generate loot for the player
                StatTracker.SetNecromancerDefeated(true); // Set the necromancer defeated flag
                break;
        }

        foreach (Card card in loot)
        {
            // Add the loot to the player's inventory or deck
            await Task.Delay(100); // Simulate a delay for looting
            await AnimationController.DisplayCard(card); // Display the card animation
            for (int i = 0; i < card.count; i++)
            { cm.playerDetails.deck.Add(card); }
            StatTracker.CollectCard(card); // Track the collected card
        }
    }

    public static async Task SlimeLogic(CombatDetails slime, CombatDetails player)
    {
        // Slime logic goes here
        Debug.Log("Slime logic executed.");
        await EnemyTurnStart(slime, player);
        Card card = null;
        while (slime.energy > 0)
        {
            Debug.Log("Slime energy: " + slime.energy);
            if (slime.deck.OfType<SlimeBurst>().Any())
            {
                Debug.Log("Slime Burst card found in deck.");
                card = slime.deck.OfType<SlimeBurst>().FirstOrDefault();
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(slime, player); // Use the card effect
                CombatMechanics.UseEnergy(slime, 1); // Use 1 energy for the slime burst
            }
            if(card.Use())
            {
                slime.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(slime);
            }
            Debug.Log("Slime energy after card use: " + slime.energy);
        }
    }

    public static async Task GoblinLogic(CombatDetails goblin, CombatDetails player)
    {
        // Goblin logic goes here
        Debug.Log("Goblin logic executed.");
        await EnemyTurnStart(goblin, player);
        Card card = null;
        while (goblin.energy > 0)
        {
            await Task.Delay(1000);
            if((goblin.health <= 5) && goblin.deck.OfType<HealthPotion>().Any())
            {
                card = goblin.deck.OfType<HealthPotion>().FirstOrDefault();
            }
            else if (goblin.deck.OfType<GoblinDagger>().Any())
            {
                card = goblin.deck.OfType<GoblinDagger>().FirstOrDefault();
            }
            else if (goblin.deck.OfType<Stone>().Any())
            {
                card = goblin.deck.OfType<Stone>().FirstOrDefault();
            }

            if (card != null)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(goblin, player); // Use the card effect
                CombatMechanics.UseEnergy(goblin, 1); 
            }

            if(card.Use())
            {
                goblin.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(goblin);
            }
        }

    }

    public static async Task SkeletonArcherLogic(CombatDetails skeleton, CombatDetails player)
    {
        // Skeleton archer logic goes here
        Debug.Log("Skeleton archer logic executed.");
        await EnemyTurnStart(skeleton, player);
        Card card = null;
        while (skeleton.energy > 0)
        {
            await Task.Delay(1000);
            int randomIndex = Random.Range(0, 5); // 20% chance to use a poison arrow if available
            if (skeleton.deck.OfType<SkeletonPoisonArrow>().Any() && randomIndex == 0)
            {
                card = skeleton.deck.OfType<SkeletonPoisonArrow>().FirstOrDefault();
            }
            else if (skeleton.deck.OfType<SkeletonArrow>().Any())
            {
                card = skeleton.deck.OfType<SkeletonArrow>().FirstOrDefault();
            }
            else if (skeleton.deck.OfType<RibBone>().Any())
            {
                card = skeleton.deck.OfType<RibBone>().FirstOrDefault();
            }

            if (card != null)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1);
            }

            if(card.Use())
            {
                skeleton.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(skeleton);
            }
        }
    }

    public static async Task SkeletonSwordLogic(CombatDetails skeleton, CombatDetails player)
    {
        // Skeleton sword logic goes here
        Debug.Log("Skeleton sword logic executed.");
        await EnemyTurnStart(skeleton, player);
        Card card = null;

        while (skeleton.energy > 0)
        {
            await Task.Delay(1000);
            int randomIndex = Random.Range(0, 5); // 20% chance to shield if available
            if (skeleton.deck.OfType<Shield>().Any() && randomIndex <= 1)
            {
                card = skeleton.deck.OfType<Shield>().FirstOrDefault();
            }
            else if (skeleton.deck.OfType<AdamantiteSword>().Any())
            {
                card = skeleton.deck.OfType<AdamantiteSword>().FirstOrDefault();
            }
            else if (skeleton.deck.OfType<IronSword>().Any())
            {
                card = skeleton.deck.OfType<IronSword>().FirstOrDefault();
            }
            else if (skeleton.deck.OfType<RibBone>().Any())
            {
                card = skeleton.deck.OfType<RibBone>().FirstOrDefault();
            }

            if (card != null)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(skeleton, player); // Use the card effect
                CombatMechanics.UseEnergy(skeleton, 1); // Use 1 energy for the clawed slash
            }

            if(card.Use() && card.itemType != ItemType.Shield)
            {
                skeleton.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(skeleton);
            }
            
        }
    }

    public static async Task VampireLogic(CombatDetails vampire, CombatDetails player)
    {
        // Vampire logic goes here
        Debug.Log("Vampire logic executed.");
        await EnemyTurnStart(vampire, player);
        Card card = null;

        while (vampire.energy > 0)
        {
            await Task.Delay(1000);
            if (vampire.health <= 10 && vampire.deck.OfType<GreatHealthPotion>().Any())
            {
                card = vampire.deck.OfType<GreatHealthPotion>().FirstOrDefault();
            }
            else if (vampire.deck.OfType<Darkness>().Any())
            {
                card = vampire.deck.OfType<Darkness>().FirstOrDefault();
            }
            else if (vampire.deck.OfType<VampiricBite>().Any())
            {
                card = vampire.deck.OfType<VampiricBite>().FirstOrDefault();
            }
            else if (vampire.deck.OfType<FlamingSword>().Any())
            {
                card = vampire.deck.OfType<FlamingSword>().FirstOrDefault();
            }

            if (card != null)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(vampire, player); // Use the card effect
                CombatMechanics.UseEnergy(vampire, 1); // Use 1 energy for the clawed slash
            }

            if(card.Use())
            {
                vampire.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(vampire);
            }
            if (!vampire.deck.OfType<VampiricBite>().Any())
            {
                vampire.deck.Add(new VampiricBite()); // Add a new Vampiric Bite card to the deck
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(vampire);
            }

        }
    }

    public static async Task WerewolfLogic(CombatDetails werewolf, CombatDetails player)
    {
        // Werewolf logic goes here
        Debug.Log("Werewolf logic executed.");
        await EnemyTurnStart(werewolf, player);
        Card card = null;

        werewolf.deck.Add(new ClawedSlash());
        werewolf.deck.Add(new ClawedSlash());
        werewolf.deck.Add(new ClawedSlash());

        while (werewolf.energy > 0)
        {
            await Task.Delay(1000);
            int howlChance = Random.Range(0, 5); // 20% chance to howl if available
            if (werewolf.deck.OfType<Howl>().Any() && howlChance == 0)
            {
                card = werewolf.deck.OfType<Howl>().FirstOrDefault();
            }
            else if (werewolf.deck.OfType<HastePotion>().Any() && werewolf.health <= werewolf.healthMax / 2)
            {
                card = werewolf.deck.OfType<HastePotion>().FirstOrDefault();
            }
            else if (werewolf.deck.OfType<ClawedSlash>().Any())
            {
                card = werewolf.deck.OfType<ClawedSlash>().FirstOrDefault();
            }

            if (card != null)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(werewolf, player); // Use the card effect
                CombatMechanics.UseEnergy(werewolf, 1);
            }

            if(card.Use())
            {
                werewolf.deck.Remove(card); // Remove the card from the deck if it has been used
                PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
                await pc.AdjustEnemyHand(werewolf);
            }
        }
    }

    public static async Task NecromancerLogic(CombatDetails necromancer, CombatDetails player)
    {
        // Necromancer logic goes here
        Debug.Log("Necromancer logic executed.");
        await EnemyTurnStart(necromancer, player);

        Card card = null;
        // Add 2 random cards to the necromancer's deck
        for (int i = 0; i < necromancer.energyMax; i++){
            int newCard = Random.Range(0, 6);
            switch (newCard){
                case 1: necromancer.deck.Add(new NecroticTouch()); break;
                case 2: necromancer.deck.Add(new MagicShield()); break;
                case 3: necromancer.deck.Add(new Fireball()); break;
                case 4: necromancer.deck.Add(new Cleanse()); break;
                case 5: necromancer.deck.Add(new ConjureArcaneBarrage()); break;
                case 6: necromancer.deck.Add(new Curse()); break;
                default: break;
            }
            PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
            await pc.AdjustEnemyHand(necromancer);
        }

        while (necromancer.energy > 0)
        {
            Debug.Log("Necromancer energy: " + necromancer.energy);
            await Task.Delay(500);
            if (necromancer.health <= 25 && necromancer.deck.OfType<SuperHealthPotion>().Any())
            {
                card = necromancer.deck.OfType<GreatHealthPotion>().FirstOrDefault();
            }
            else if (necromancer.health <= 15 && necromancer.deck.OfType<GreatHealthPotion>().Any())
            {
                card = necromancer.deck.OfType<SuperHealthPotion>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<ArcaneMissile>().Any())
            {
                card = necromancer.deck.OfType<ArcaneMissile>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<ConjureArcaneBarrage>().Any())
            {
                card = necromancer.deck.OfType<ConjureArcaneBarrage>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<Cleanse>().Any() && necromancer.statusEffects.Count >= 3)
            {
                card = necromancer.deck.OfType<Curse>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<MagicShield>().Any() && necromancer.health <= necromancer.healthMax * (2/3))
            {
                card = necromancer.deck.OfType<MagicShield>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<Fireball>().Any())
            {
                card = necromancer.deck.OfType<NecroticTouch>().FirstOrDefault();
            }
            else if (necromancer.deck.OfType<NecroticTouch>().Any())
            {
                card = necromancer.deck.OfType<Fireball>().FirstOrDefault();
            }

            if (card != null && card is not ArcaneMissile)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(necromancer, player); // Use the card effect
                CombatMechanics.UseEnergy(necromancer, 1);
            }
            else if (card != null && card is ArcaneMissile)
            {
                await AnimationController.TriggerEnemyAction(card);
                await card.Effect(necromancer, player); // Use the card effect
            }

            if(card.Use())
            {
                necromancer.deck.Remove(card); // Remove the card from the deck if it has been used
            }
            PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
            await pc.AdjustEnemyHand(necromancer);
        }
    }
}
