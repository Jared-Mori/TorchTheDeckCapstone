using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Armor classes
/// </summary>
public interface Armor
{
    Task ArmorEffect(CombatDetails user, CombatDetails target);
}

[System.Serializable]
public class IronHelmet : Card, Armor
{
    public IronHelmet()
    {
        cardName = "Iron Helmet";
        description = "A sturdy helmet. It's seen better days, but it'll protect your head.";
        tooltip = "This helmet does nothing extra.";
        this.itemType = ItemType.Helmet;
        uses = 3;
        rarity = Rarity.Common;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class IronChestpiece : Card, Armor
{
    public IronChestpiece()
    {
        cardName = "Iron Chestpiece";
        description = "A heavy chestpiece. It's a bit rusty, but it'll protect your torso.";
        tooltip = "This chestpiece does nothing extra.";
        itemType = ItemType.Chestpiece;
        uses = 3;
        rarity = Rarity.Common;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class IronBoots : Card, Armor
{
    public IronBoots()
    {
        cardName = "Iron Boots";
        description = "A pair of armored boots. They're a bit scuffed, but they'll protect your feet.";
        tooltip = "These boots do nothing extra.";
        itemType = ItemType.Boots;
        uses = 3;
        rarity = Rarity.Common;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class MythrilHelmet : Card, Armor
{
    public MythrilHelmet()
    {
        cardName = "Mythril Helmet";
        description = "A lightweight helmet. They're made of a rare metal that is light yet durable.";
        tooltip = "This helmet does nothing extra.";
        itemType = ItemType.Helmet;
        uses = 6;
        rarity = Rarity.Uncommon;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class MythrilChestpiece : Card, Armor
{
    public MythrilChestpiece()
    {
        cardName = "Mythril Chestpiece";
        description = "A lightweight chestpiece. They're made of a rare metal that is light yet durable.";
        tooltip = "This chestpiece does nothing extra.";
        itemType = ItemType.Chestpiece;
        uses = 6;
        rarity = Rarity.Uncommon;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class MythrilBoots : Card, Armor
{
    public MythrilBoots()
    {
        cardName = "Mythril Boots";
        description = "A pair of lightweight boots. They're made of a rare metal that is light yet durable.";
        tooltip = "These boots do nothing extra.";
        itemType = ItemType.Boots;
        uses = 6;
        rarity = Rarity.Uncommon;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class RamHelm : Card, Armor
{
    public RamHelm()
    {
        cardName = "Ram's Horn Helm";
        description = "A helmet adorned with ram's horns. It looks intimidating.";
        tooltip = "This helmet increases your max health by 2 when hit.";
        itemType = ItemType.Helmet;
        uses = 10;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        user.healthMax += 2; // Increase max health by 2
        user.health += 2; // Heal the user by 2
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class InvigoratingChestplate : Card, Armor, HealingItem
{
    public int healing { get; set; } = 3; // Default healing value
    public InvigoratingChestplate()
    {
        cardName = "Invigorating Chestplate";
        description = "A chestplate that invigorates the wearer. It glows with a faint light.";
        tooltip = $"This chestpiece heals you for {healing} when hit.";
        itemType = ItemType.Chestpiece;
        uses = 10;
        rarity = Rarity.Rare;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        CombatMechanics.Heal(user, healing); // Heal the user
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class FrogskinBoots : Card, Armor
{
    public int Ribbets { get; set; } = 0; // Default Ribbets value
    public FrogskinBoots()
    {
        cardName = "Frogz kin Boots";
        description = "A pair of boots made from frog skin. A low ribbet can be heard with each step.";
        tooltip = $"This armor gives off a low ribbet when hit. Something special may happen in {5 - Ribbets} ribbets.";
        itemType = ItemType.Boots;
        uses = 15;
        rarity = Rarity.Rare;
    }

    public async Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        Ribbets++; // Increment Ribbets count
        if (Ribbets == 5)
        {
            Debug.Log(cardName + " armor effect triggered!");
            Ribbets = 0; // Reset Ribbets count
            await CombatMechanics.Defend(user, target, Ribbets * 3); // Apply the effect to the user
            CombatMechanics.Heal(user, Ribbets * 3); // Heal the user by 3
        }
        else 
        {
            Debug.Log("Ribbets count: " + Ribbets);
        }
    }
}

[System.Serializable]
public class BoneLordHelmet : Card, Armor
{
    public BoneLordHelmet()
    {
        cardName = "Bone Lord's Helmet";
        description = "The helmet of a long-dead Bone Lord. It radiates a dark energy.";
        tooltip = "This helmet extends the duration of status effects on the attacker when full Bone Lord set is worn. Burn and poison deal additional damage.";
        itemType = ItemType.Helmet;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        if (pc.hand.OfType<Card>().Any(c => c is BoneLordBreastplate || c is BoneLordGreaves))
        {
            Debug.Log("Bone Lord's armor set bonus activated!");
            // Apply set bonus effect here
            foreach (Status status in target.statusEffects){
                status.AmplifyStatus(target, user); // Amplify status effects on the target
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class BoneLordBreastplate : Card, Armor
{
    public BoneLordBreastplate()
    {
        cardName = "Bone Lord's Breastplate";
        description = "The breastplate of a long-dead Bone Lord. It radiates a dark energy.";
        tooltip = "This breastplate extends the duration of status effects on the attacker when full Bone Lord set is worn. Burn and poison deal additional damage.";
        itemType = ItemType.Chestpiece;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        if (pc.hand.OfType<Card>().Any(c => c is BoneLordHelmet || c is BoneLordGreaves))
        {
            Debug.Log("Bone Lord's armor set bonus activated!");
            // Apply set bonus effect here
            foreach (Status status in target.statusEffects){
                status.AmplifyStatus(target, user); // Amplify status effects on the target
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class BoneLordGreaves : Card, Armor
{
    public BoneLordGreaves()
    {
        cardName = "Bone Lord's Greaves";
        description = "The greaves of a long-dead Bone Lord. They radiate a dark energy.";
        tooltip = "This greaves extends the duration of status effects on the attacker when full Bone Lord set is worn. Burn and poison deal additional damage.";
        itemType = ItemType.Boots;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        if (pc.hand.OfType<Card>().Any(c => c is BoneLordBreastplate || c is BoneLordHelmet))
        {
            Debug.Log("Bone Lord's armor set bonus activated!");
            // Apply set bonus effect here
            foreach (Status status in target.statusEffects){
                status.AmplifyStatus(target, user); // Amplify status effects on the target
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class ArchmagesGall : Card, Armor
{
    public ArchmagesGall()
    {
        cardName = "Archmage's Gall";
        description = "A pointed hat worn by archmages. It looks ancient and powerful.";
        tooltip = "This helmet empowers potions when hit. Healing items heal for 2 more, haste items increase duration by 1, stamina and vitality items increase by 1.";
        itemType = ItemType.Helmet;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card is HealthPotion || card is GreatHealthPotion || card is SuperHealthPotion)
            {
                Debug.Log("Archmage's Gall effect triggered!");
                // Apply effect to the healing item here
                HealingItem healingItem = (HealingItem)card;
                healingItem.healing += 2; // Increase healing by 2
            }
            else if (card is HastePotion)
            {
                Debug.Log("Archmage's Gall effect triggered!");
                // Apply effect to the haste item here
                HastePotion hasteItem = (HastePotion)card;
                hasteItem.status.AmplifyStatus(target, user); // Increase duration by 1 turn
            }
            else if (card is StaminaDraught)
            {
                Debug.Log("Archmage's Gall effect triggered!");
                // Apply effect to the stamina item here
                StaminaDraught staminaItem = (StaminaDraught)card;
                staminaItem.stamina += 1; // Increase stamina boost by 1
            }
            else if (card is VitalityDraught)
            {
                Debug.Log("Archmage's Gall effect triggered!");
                // Apply effect to the vitality item here
                VitalityDraught vitalityItem = (VitalityDraught)card;
                vitalityItem.vitality += 1; // Increase vitality boost by 1
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class DragonKingsCuriass : Card, Armor
{
    public DragonKingsCuriass()
    {
        cardName = "Dragon King's Curiass";
        description = "A chestpiece adorned with dragon scales. It looks heavy and powerful.";
        tooltip = "This chestpiece increases the damage of your weapons by 4 when hit.";
        itemType = ItemType.Chestpiece;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card is Weapon && card is not Stone)
            {
                Debug.Log("Dragon King's Curiass effect triggered!");
                // Apply set bonus effect here
                Weapon weapon = (Weapon)card;
                weapon.damage += 4; // Increase damage by 2
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class FalconSabatons : Card, Armor
{
    public FalconSabatons()
    {
        cardName = "Falcon Sabatons";
        description = "A pair of boots adorned with falcon feathers. They look light and agile.";
        tooltip = $"This armor increases the damage of your arrows by 2 when hit.";
        itemType = ItemType.Boots;
        uses = 14;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        PileController pc = GameObject.Find("Deck").GetComponent<PileController>();
        foreach (Card card in pc.hand)
        {
            if (card is Arrow)
            {
                Debug.Log("Falcon Sabatons effect triggered!");
                // Apply set bonus effect here
                Arrow arrow = (Arrow)card;
                arrow.damage += 2; // Increase damage by 2
            }
        }
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

/// <summary>
/// Shield classes
/// </summary>
[System.Serializable]
public class Shield : Card, Armor
{
    public Shield()
    {
        cardName = "Shield";
        description = "A wooden shield. It's cracked in places, but it'll protect you from attacks.";
        tooltip = "This shield does nothing extra.";
        itemType = ItemType.Shield;
        uses = 3;
        rarity = Rarity.Common;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }

    public override bool Use(){

        return base.Use();
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        user.isShielded = true;
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class IronShield : Card, Armor
{
    public IronShield()
    {
        cardName = "Reinforced Shield";
        description = "A metal reinforced shield. It's heavy, but still holds together.";
        tooltip = "This shield does nothing extra.";
        itemType = ItemType.Shield;
        uses = 6;
        rarity = Rarity.Uncommon;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        // Armor Effect Triggers when the armor blocks damage.
        return Task.CompletedTask; // Return a completed task for async compatibility
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        user.isShielded = true;
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class ThornedShield : Card, Armor
{
    public int damage { get; set; } = 3; // Default damage value
    public ThornedShield()
    {
        cardName = "Thorned Shield";
        description = "A shield with spikes. It hurts to touch.";
        tooltip = $"This shield does {damage} damage to the attacker when hit.";
        itemType = ItemType.Shield;
        uses = 6;
        rarity = Rarity.Rare;
    }

    public async Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        await CombatMechanics.TakeDamage(target, user, damage); // Damage to attacker
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        user.isShielded = true;
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}

[System.Serializable]
public class DivineShield : Card, Armor
{
    public DivineShield()
    {
        cardName = "Divine Shield";
        description = "A holy shield. It glows with a divine light.";
        tooltip = "This shield shields the user until the start of their next turn.";
        itemType = ItemType.Shield;
        uses = 10;
        rarity = Rarity.Legendary;
    }

    public Task ArmorEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " armor effect triggered!");
        user.isShielded = true; // Grants shield to user
        return Task.CompletedTask; // Return a completed task for async compatibility
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        user.isShielded = true;
        return Task.CompletedTask; // Return a completed task for async compatibility
    }
}
