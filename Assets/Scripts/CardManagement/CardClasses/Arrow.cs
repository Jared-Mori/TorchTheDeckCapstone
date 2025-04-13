using UnityEngine;

/// <summary>
/// Arrow classes
/// </summary>
public interface Arrow
{
    int damage { get; set; }
    void ArrowEffect(CombatDetails user, CombatDetails target);
}

[System.Serializable]
public class WoodArrow : Card, Arrow
{
    public int damage { get; set; } = 1; // Default damage value
    public WoodArrow()
    {
        cardName = "Wooden Arrow";
        description = "A simple arrow. It's not very powerful, but it's better than nothing.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = 0;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.TakeDamage(target, user, damage);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.UseEnergy(user, 1);
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            ArrowEffect(user, target);
            bow.BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
        }
    }
}

[System.Serializable]
public class PoisonArrow : Card, Arrow, IStatusEffect
{
    public int damage { get; set; } = 1; // Default damage value
    public Status status { get; set; } = new Poison(); // Default status effect
    public PoisonArrow()
    {
        cardName = "Poison Arrow";
        description = "An arrow coated in poison. It deals damage over time.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = 2;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        CombatMechanics.TakeDamage(target, user, damage);
        target.statusEffects.Add(status);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.UseEnergy(user, 1);
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            ArrowEffect(user, target);
            bow.BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
        }
    }
}
