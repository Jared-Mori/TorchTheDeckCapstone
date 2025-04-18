using UnityEngine;

/// <summary>
/// Necromancer Cards
/// /summary>

public interface NecromancerSpells
{
    
}
public class NecroticTouch : Card, EnemyCards, NecromancerSpells
{
    public NecroticTouch()
    {
        cardName = "Necrotic Touch";
        description = "Tendril's of dark energy reach out drain the lifeforce of the target.";
        tooltip = "Reduces health and max health by 5.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Necrotic Touch effect executed.");
        CombatMechanics.Defend(target, user, 5); // deals 5 damage to target
        target.healthMax -= 5; // Reduces max health by 5
    }
}

public class MagicShield : Card, EnemyCards, NecromancerSpells
{
    public MagicShield()
    {
        cardName = "Magic Shield";
        description = "A magical shield shimmers about the user.";
        tooltip = "Reduces damage taken by 1.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Magic Shield effect executed.");
        target.isShielded = true; // Sets the isShielded flag to true
    }
}

public class Fireball : Card, EnemyCards, NecromancerSpells
{
    public Fireball()
    {
        cardName = "Fireball";
        description = "A ball of fire that explodes on impact.";
        tooltip = "Deals 10 damage. Applies burn status effect.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Fireball effect executed.");
        CombatMechanics.Defend(target, user, 10); // deals 5 damage to target
        target.statusEffects.Add(new Burn()); // applies burn status effect to target
    }
}

public class Cleanse : Card, EnemyCards, NecromancerSpells
{
    public Cleanse()
    {
        cardName = "Cleanse";
        description = "A cleansing light that removes all negative status effects.";
        tooltip = "Removes 3 negative status effects from the user.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Cleanse effect executed.");
        for (int i = 0; i < 3; i++)
        {
            if (user.statusEffects.Count > 0)
            {
                user.statusEffects.RemoveAt(0); // Removes the first negative status effect from the user
            }
        }
    }
}

public class ConjureArcaneBarrage : Card, EnemyCards, NecromancerSpells
{
    public ConjureArcaneBarrage()
    {
        cardName = "Conjure Arcane Barrage";
        description = "A swarm of arcane.";
        tooltip = "Grants 1 Arcane Missile for each card in hand.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        foreach (Card card in user.deck){
            user.deck.Add(new ArcaneMissile()); // Adds a Arcane Missile card to the user's deck
        }
    }
}

public class ArcaneMissile : Card, EnemyCards, NecromancerSpells
{
    public ArcaneMissile()
    {
        cardName = "Arcane Missile";
        description = "A missile of arcane energy.";
        tooltip = "Deals 1 damage. Costs no energy.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Arcane Missile effect executed.");
        CombatMechanics.TakeDamage(target, user, 1); // deals 1 damage to target
    }
}

public class Curse : Card, EnemyCards, NecromancerSpells, IStatusEffect
{
    public Status status { get; set; } = new Poison(); // Default status effect
    public Curse()
    {
        cardName = "Curse";
        description = "A curse that blights the target with numerous maladies.";
        tooltip = "Applies Poison, Burn, and Exhausted to the target.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Curse effect executed.");
        target.statusEffects.Add(new Poison()); // applies poison status effect to target
        target.statusEffects.Add(new Burn()); // applies burn status effect to target
        target.statusEffects.Add(new Exhausted()); // applies exhausted status effect to target
    }
}

public class NecromancersCrown : Card, Armor, EnemyCards
{
    public NecromancersCrown()
    {
        cardName = "Necromancer's Crown";
        description = "A crown that grants the wearer immense power.";
        tooltip = "Grants +1 damage and durability to all equipped weapons.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        throw new System.NotImplementedException();
    }
}

public class NecromancersCloak : Card, Armor, EnemyCards
{
    public NecromancersCloak()
    {
        cardName = "Necromancer's Cloak";
        description = "A cloak that grants the wearer immense power.";
        tooltip = "Grants +1 damage and durability to all equipped weapons.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        throw new System.NotImplementedException();
    }
}

public class NecromancersSlippers : Card, Armor, EnemyCards
{
    public NecromancersSlippers()
    {
        cardName = "Necromancer's Slippers";
        description = "A pair of slippers that grants the wearer immense power.";
        tooltip = "Grants +1 damage and durability to all equipped weapons.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        throw new System.NotImplementedException();
    }
}

public class RingOfTheDead : Card, Accessory, EnemyCards
{
    public RingOfTheDead()
    {
        cardName = "Ring of the Dead";
        description = "A ring that grants the wearer immense power.";
        tooltip = "Grants +1 damage and durability to all equipped weapons.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void AccessoryEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in user.deck)
        {
            if (card is Weapon)
            {
                card.uses += 1;
                Weapon weapon = (Weapon)card;
                weapon.damage += 1;
            }
        }
    }
}
