using UnityEngine;

/// <summary>
/// Necromancer Cards
/// /summary>

public interface NecromancerSpells
{
    public void enhanceSpell(CombatDetails user, CombatDetails target);
}
public class NecroticTouch : Card, EnemyCards, NecromancerSpells
{
    public int damage { get; set; } = 5; // Default damage value
    public NecroticTouch()
    {
        cardName = "Necrotic Touch";
        description = "Tendril's of dark energy reach out drain the lifeforce of the target.";
        tooltip = $"Reduces health and max health by {damage}.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Necrotic Touch effect executed.");
        CombatMechanics.Defend(target, user, damage); // deals 5 damage to target
        target.healthMax -= damage; // Reduces max health by 5
    }

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Necrotic Touch spell enhanced.");
        damage += 2; // Increases the damage of the spell
    }
}

public class MagicShield : Card, EnemyCards, NecromancerSpells
{
    public int shieldAmount { get; set; } = 0; // Default shield amount
    public MagicShield()
    {
        cardName = "Magic Shield";
        description = "A magical shield shimmers about the user.";
        tooltip = "Shields the target.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Magic Shield effect executed.");
        target.isShielded = true; // Sets the isShielded flag to true
        CombatMechanics.Heal(user, target, shieldAmount); // heals the user for 5 health
    }

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Magic Shield spell enhanced.");
        shieldAmount += 5; // Increases the shield amount
}

public class Fireball : Card, EnemyCards, NecromancerSpells
{
    public int damage { get; set; } = 10; // Default damage value
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

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Fireball spell enhanced.");
        damage += 5; // Increases the damage of the spell
    }
}

public class Cleanse : Card, EnemyCards, NecromancerSpells
{
    public int statusEffectCount { get; set; } = 3; // Default status effect count
    public Cleanse()
    {
        cardName = "Cleanse";
        description = "A cleansing light that removes all negative status effects.";
        tooltip = $"Removes {statusEffectCount} negative status effects from the user.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Cleanse effect executed.");
        for (int i = 0; i < statusEffectCount; i++)
        {
            if (user.statusEffects.Count > 0)
            {
                user.statusEffects.RemoveAt(0); // Removes the first negative status effect from the user
            }
        }
    }

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Cleanse spell enhanced.");
        statusEffectCount += 1; // Increases the number of status effects removed
    }
}

public class ConjureArcaneBarrage : Card, EnemyCards, NecromancerSpells
{
    public int missileCount { get; set; } = 1; // Default missile count
    public ConjureArcaneBarrage()
    {
        cardName = "Conjure Arcane Barrage";
        description = "A swarm of arcane.";
        tooltip = $"Grants {missileCount} Arcane Missile for each card in hand.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        foreach (Card card in user.deck){
            user.deck.Add(new ArcaneMissile()); // Adds a Arcane Missile card to the user's deck
        }
    }

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Conjure Arcane Barrage spell enhanced.");
        missileCount += 1; // Increases the number of missiles
    }
}

public class ArcaneMissile : Card, EnemyCards, NecromancerSpells
{
    public int damage { get; set; } = 1; // Default damage value
    public ArcaneMissile()
    {
        cardName = "Arcane Missile";
        description = "A missile of arcane energy.";
        tooltip = $"Deals {damage} damage. Costs no energy.";
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("Arcane Missile effect executed.");
        CombatMechanics.TakeDamage(target, user, damage); // deals 1 damage to target
    }

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Arcane Missile spell enhanced.");
        damage += 1; // Increases the number of uses of the spell
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

    public void enhanceSpell(CombatDetails user, CombatDetails target)
    {
        // Implement the logic to enhance the spell here
        Debug.Log("Curse spell enhanced.");
        target.statusEffects.Add(new Poison()); // applies poison status effect to target
        target.statusEffects.Add(new Burn()); // applies burn status effect to target
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
        foreach (Card card in user.deck)
        {
            if (card is NecromancerSpells)
            {
                NecromancerSpells spell = (NecromancerSpells)card;
                spell.enhanceSpell(user, target);
            }
        }
    }
}

public class NecromancersCloak : Card, Armor, EnemyCards
{
    public NecromancersCloak()
    {
        cardName = "Necromancer's Cloak";
        description = "A cloak that grants the wearer immense power.";
        tooltip = "Reduces the damage of Weapons or arrows in players hand. Reduces the healing of items in players hand.";
        uses = 10;
        rarity = Rarity.Legendary;
        isStackable = true;
        itemType = ItemType.Accessory;
    }

    public void ArmorEffect(CombatDetails user, CombatDetails target)
    {
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card is weapon)
            {
                Weapon weapon = (Weapon)card;
                weapon.damage -= 1;
            }
            else if (card is HealingItem)
            {
                HealingItem item = (HealingItem)card;
                item.healing -= 1; // Decreases the healing amount of the item
            }
            else if (card is Arrow)
            {
                Arrow arrow = (Arrow)card;
                arrow.damage -= 1; // Decreases the damage of the arrow
            }
        }
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
        foreach (Card card in user.deck)
        {
            if (card is HealingItem)
            {
                HealingItem item = (HealingItem)card;
                item.healing += 3; // Increases the healing amount of the item
            }
        }
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
        return;
    }
}
