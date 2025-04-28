using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Bow classes
/// </summary>

public interface Bow
{
    Task BowEffect(CombatDetails user, CombatDetails target);
}
[System.Serializable]
public class Longbow : Card, Bow
{
    public Longbow()
    {
        cardName = "Longbow";
        description = "A simple bow. It's not very powerful, but it's better than nothing.";
        tooltip = "This bow does nothing extra!";
        itemType = ItemType.Bow;
        uses = 3;
        rarity = Rarity.Common;
    }

    public Task BowEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This bow does nothing extra!");
        return Task.CompletedTask;
    }

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("Deck").GetComponent<PileController>();
        var card = pileController.GetEquippedCard(ItemType.Arrow);
        if (card != null && card.card is Arrow arrow)
        {
            CombatMechanics.UseEnergy(user, 1);
            arrow.ArrowEffect(user, target);
            BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
        return Task.CompletedTask;
    }
}

[System.Serializable]
public class Crossbow : Card, Bow
{
    public Crossbow()
    {
        cardName = "Crossbow";
        description = "A powerful crossbow. It can shoot arrows with great force.";
        tooltip = "This Crossbow does nothing extra";
        itemType = ItemType.Bow;
        uses = 7;
        rarity = Rarity.Uncommon;
    }

    public async Task BowEffect(CombatDetails user, CombatDetails target)
    {
        await CombatMechanics.TakeDamage(target, user, 1);
        Debug.Log("This Crossbow does 1 extra damage!");
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("Deck").GetComponent<PileController>();
        var card = pileController.GetEquippedCard(ItemType.Arrow);
        if (card != null && card.card is Arrow arrow)
        {
            CombatMechanics.UseEnergy(user, 1);
            await arrow.ArrowEffect(user, target);
            await BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
    }
}

[System.Serializable]
public class ChargeBow : Card, Bow
{
    public ChargeBow()
    {
        cardName = "Arcane Charged Bow";
        description = "An enchanted bow that charges its arrows with energy. It can shoot arrows with great force.";
        tooltip = "This bow consumes all of the users energy. Deals 2 damage per energy spent.";
        itemType = ItemType.Bow;
        uses = 5;
        rarity = Rarity.Rare;
    }

    public async Task BowEffect(CombatDetails user, CombatDetails target)
    {
        for (int i = 0; i < user.energy; i++)
        {
            await CombatMechanics.TakeDamage(target, user, 2);
            CombatMechanics.UseEnergy(user, 1);
        }
    }

    public override async Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("Deck").GetComponent<PileController>();
        var card = pileController.GetEquippedCard(ItemType.Arrow);
        if (card != null && card.card is Arrow arrow)
        {
            CombatMechanics.UseEnergy(user, 1);
            await arrow.ArrowEffect(user, target);
            await BowEffect(user, target);
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
    }
}

[System.Serializable]
public class Stratus : Card, Bow
{
    public Stratus()
    {
        cardName = "Stratus";
        description = "A legendary bow that multiplies arrows it fires. It is said to be imbued with the power of the sky.";
        tooltip = "This bow duplicates the arrows it fires 2 additional times.";
        itemType = ItemType.Bow;
        uses = 5;
        rarity = Rarity.Legendary;
    }

    public Task BowEffect(CombatDetails user, CombatDetails target) {return Task.CompletedTask;}

    public override Task Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("Deck").GetComponent<PileController>();
        var card = pileController.GetEquippedCard(ItemType.Arrow);
        if (card != null && card.card is Arrow arrow)
        {
            CombatMechanics.UseEnergy(user, 1);
            // Fire 3 arrows
            arrow.ArrowEffect(user, target);
            arrow.ArrowEffect(user, target);
            arrow.ArrowEffect(user, target);
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
        return Task.CompletedTask;
    }
}