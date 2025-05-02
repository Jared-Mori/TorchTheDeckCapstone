using System.Threading.Tasks;
using UnityEngine;

public interface EnemyCards
{
    
}

/// <summary>
/// Slime Cards
/// /summary>

public class SlimeBurst : Card, EnemyCards
{
    public SlimeBurst()
    {
        cardName = "Slime Burst";
        description = "The slime bursts, sending caustic acid flying. The slime seems to be slightly smaller now.";
        tooltip = "Deals damage equal to your health to the target. Take 1 damage.";
        uses = 1;
        rarity = Rarity.Common;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Slime Burst effect executed.");
        await CombatMechanics.Defend(target, user, user.health); // deals damage equal to users health
        await CombatMechanics.TakeDamage(user, target, 1); // deals 1 damage to user
    }
}

/// <summary>
/// Goblin Cards
/// </summary>
public class GoblinDagger : Card, Weapon
{
    public int damage { get; set; } = 1; // Default damage value
    public GoblinDagger()
    {
        cardName = "Goblin Dagger";
        description = "A small dagger used by goblins. It's not very powerful, but it's quick.";
        tooltip = "This dagger does nothing extra.";
        itemType = ItemType.Weapon;
        uses = 7;
        rarity = Rarity.Common;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Goblin Dagger effect executed.");
        await CombatMechanics.Defend(target, user, damage);
    }
}

/// <summary>
///  Skeleton Cards
/// </summary>

public class SkeletonArrow : Card, EnemyCards
{
    public SkeletonArrow()
    {
        cardName = "Skeleton Arrow";
        description = "A rusty arrow used by skeletons. It's not very powerful, but it's quick.";
        tooltip = "This arrow deals 1-2 damage to the target.";
        uses = 1;
        rarity = Rarity.Common;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Skeleton Arrow effect executed.");
        int damage = Random.Range(1, 3); // Random damage between 1 and 2
        await CombatMechanics.TakeDamage(target, user, damage); // deals 1 damage to target
    }
}

public class SkeletonPoisonArrow : Card, EnemyCards, IStatusEffect
{
    public Status status { get; set; } = new Poison(); // Default status effect
    public SkeletonPoisonArrow()
    {
        cardName = "Skeleton Poison Arrow";
        description = "A rusty arrow used by skeletons. It's enchanted to poison its target.";
        tooltip = "Applies poison to the target and deals 1 damage.";
        uses = 1;
        rarity = Rarity.Uncommon;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Skeleton Poison Arrow effect executed.");
        await CombatMechanics.TakeDamage(target, user, 1); // deals 1 damage to target
        target.statusEffects.Add(status); // applies poison status effect to target
    }
}

public class RibBone : Card, EnemyCards
{
    public RibBone()
    {
        cardName = "Rib Bone";
        description = "A bone from the skeleton's ribs. Its weaker than an arrow, but it can still hurt.";
        tooltip = "Sacrifice 1 health for 1 damage.";
        uses = 1;
        rarity = Rarity.Common;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Rib Bone effect executed.");
        await CombatMechanics.Defend(target, user, 1); // deals 1 damage to target
        await CombatMechanics.TakeDamage(user, target, 1); // deals 1 damage to user
    }
}

/// <summary>
/// Vampire Cards
/// </summary>

public class VampiricBite : Card, EnemyCards
{
    public int damage = 5; // Default damage value
    public VampiricBite()
    {
        cardName = "Vampiric Bite";
        description = "A bite from a vampire. It drains the target's health and gives it to the user.";
        tooltip = "Deals 5 damage and heals for 5 health.";
        uses = 20;
        rarity = Rarity.Rare;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Vampiric Bite effect executed.");
        await CombatMechanics.Defend(target, user, damage); // deals 1 damage to target
        CombatMechanics.Heal(user, damage); // heals user for 1 health
    }
}

public class VampiresRobe : Card, Armor, EnemyCards
{
    public VampiresRobe()
    {
        cardName = "Vampire's Robe";
        description = "A robe worn by vampires. It improves the potency of the Vampire's bite.";
        tooltip = "Increases the damage of all Vampiric Bite cards by 2 when hit.";
        itemType = ItemType.Chestpiece;
        uses = 5;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        foreach (Card card in user.deck){
            if (card is VampiricBite){
                VampiricBite bite = (VampiricBite)card;
                bite.damage += 2;
            }
        }
        return Task.CompletedTask; // No need to await anything here
    }
}

public class VampiresBoots : Card, Armor, EnemyCards
{
    public VampiresBoots()
    {
        cardName = "Vampire's Boots";
        description = "A robe worn by vampires. Shadows seem to cling to it.";
        tooltip = "Gives the user 1 Darkness card when hit.";
        itemType = ItemType.Chestpiece;
        uses = 5;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        user.deck.Add(new Darkness()); // Adds a shield card to the user's deck
        return Task.CompletedTask; // No need to await anything here
    }
}

public class Darkness : Card, EnemyCards
{
    public Darkness()
    {
        cardName = "Darkness";
        description = "A dark mist that surrounds the user. It seems to drain the light from the air.";
        tooltip = "Deals 1 damage for each card in enemy hand.";
        uses = 1;
        rarity = Rarity.Rare;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Darkness effect executed.");
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        int damage = pc.hand.Count; // Damage equal to the number of cards in hand
        await CombatMechanics.Defend(target, user, damage); // deals damage to target
    }
}


/// <summary>
/// Werewolf Cards
/// </summary>
public class Howl : Card, IStatusEffect, EnemyCards
{
    public Status status { get; set; } = new Exhausted(); // Default status effect
    public Howl()
    {
        cardName = "Howl";
        description = "A loud howl that echoes through the air. It seems to drain the light from the air.";
        tooltip = "Exhausts the target.";
        uses = 1;
        rarity = Rarity.Rare;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        target.statusEffects.Add(status); // applies paralysis status effect to target
        return Task.CompletedTask; // No need to await anything here
    }
}

public class ClawedSlash : Card, Weapon, EnemyCards
{
    public int damage { get; set; } = 3; // Default damage value
    public ClawedSlash()
    {
        cardName = "Clawed Slash";
        description = "A slash from a clawed hand. It seems to drain the light from the air.";
        tooltip = $"Deals {damage} damage.";
        uses = 1;
        rarity = Rarity.Rare;
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Clawed Slash effect executed.");
        await CombatMechanics.Defend(target, user, damage); // deals 1 damage to target
    }
}

public class WerewolfsMane : Card, Armor, EnemyCards
{
    public WerewolfsMane()
    {
        cardName = "Werewolf's Mane";
        description = "A mane from a werewolf. It seems to drain the light from the air.";
        tooltip = "Add a Clawed Slash and Howl card to hand.";
        itemType = ItemType.Helmet;
        uses = 6;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Werewolf's Mane effect executed.");
        user.deck.Add(new Howl()); // Adds a Clawed Slash card to the user's deck
        user.deck.Add(new ClawedSlash()); // Adds a Clawed Slash card to the user's deck
        return Task.CompletedTask; // No need to await anything here
    }
}

public class WerewolfsHide : Card, Armor, EnemyCards
{
    public WerewolfsHide()
    {
        cardName = "Werewolf's Hide";
        description = "A hide from a werewolf. It seems to drain the light from the air.";
        tooltip = "Increases the damage of held Clawed Slash cards by 2.";
        itemType = ItemType.Chestpiece;
        uses = 6;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        foreach (Card card in user.deck){
            if (card is ClawedSlash){
                ClawedSlash slash = (ClawedSlash)card;
                slash.damage += 2;
            }
        }
        return Task.CompletedTask; // No need to await anything here
    }
}

public class WerewolfsPursuit : Card, Armor, EnemyCards
{
    public WerewolfsPursuit()
    {
        cardName = "Werewolf's Pursuit";
        description = "A pursuit from a werewolf. It seems to drain the light from the air.";
        tooltip = "Increases the maximum energy of the wearer by 1.";
        itemType = ItemType.Boots;
        uses = 6;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        user.energyMax += 1; // Increases the user's max energy by 1
        return Task.CompletedTask; // No need to await anything here
    }
}
