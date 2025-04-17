using UnityEngine;

public interface EnemyCards
{
    
}

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

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Slime Burst effect executed.");
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, user.health); // deals damage equal to users health
        CombatMechanics.TakeDamage(user, target, 1); // deals 1 damage to user
    }
}

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

    public void WeaponEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This dagger does nothing extra!");
    }
}

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

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Skeleton Arrow effect executed.");
        CombatMechanics.UseEnergy(user, 1);
        int damage = Random.Range(1, 3); // Random damage between 1 and 2
        CombatMechanics.TakeDamage(target, user, damage); // deals 1 damage to target
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

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Skeleton Poison Arrow effect executed.");
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.TakeDamage(target, user, 1); // deals 1 damage to target
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

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        // Implement the effect of the card here
        Debug.Log("Rib Bone effect executed.");
        CombatMechanics.UseEnergy(user, 1);
        CombatMechanics.Defend(target, user, 1); // deals 1 damage to target
        CombatMechanics.TakeDamage(user, target, 1); // deals 1 damage to user
    }
}