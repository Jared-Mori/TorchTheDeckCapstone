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
        tooltip = "This arrow does nothing extra.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = Rarity.Common;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        AnimateDamageEffect(damage, target);
        CombatMechanics.TakeDamage(target, user, damage);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("CombatManager").GetComponent<CombatManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            if (bow is TempBow)
            {
                Debug.Log("You need a real bow to use this arrow!");
                pileController.AddCard(new WoodArrow());
                return;
            }
            
            CombatMechanics.UseEnergy(user, 1);
            ArrowEffect(user, target);
            bow.BowEffect(user, target);
            
            // Use the bow
            Card bowCard = bow as Card;
            bowCard.uses--;
            if (bowCard.uses <= 0)
            {
                int index = pileController.hand.IndexOf(bowCard);
                pileController.RemoveCard(index);
                pileController.AddCard(new TempBow());
            }
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
            pileController.AddCard(new WoodArrow());
        }
    }
}

[System.Serializable]
public class SteelArrow : Card, Arrow
{
    public int damage { get; set; } = 3; // Default damage value
    public SteelArrow()
    {
        cardName = "Steel Tipped Arrow";
        description = "A steel tipped arrow. It's sharper and more durable than a wooden arrow.";
        tooltip = "This arrow does nothing extra.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = Rarity.Uncommon;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        AnimateDamageEffect(damage, target);
        CombatMechanics.TakeDamage(target, user, damage);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("CombatManager").GetComponent<CombatManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            if (bow is TempBow)
            {
                Debug.Log("You need a real bow to use this arrow!");
                pileController.AddCard(new SteelArrow());
                return;
            }
            
            CombatMechanics.UseEnergy(user, 1);
            ArrowEffect(user, target);
            bow.BowEffect(user, target);
            
            // Use the bow
            Card bowCard = bow as Card;
            bowCard.uses--;
            if (bowCard.uses <= 0)
            {
                int index = pileController.hand.IndexOf(bowCard);
                pileController.RemoveCard(index);
                pileController.AddCard(new TempBow());
            }
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
            pileController.AddCard(new SteelArrow());
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
        tooltip = "This arrow applies poison to the target.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = Rarity.Rare;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        AnimateDamageEffect(damage, target);
        CombatMechanics.TakeDamage(target, user, damage);
        target.statusEffects.Add(status);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("CombatManager").GetComponent<CombatManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            if (bow is TempBow)
            {
                Debug.Log("You need a real bow to use this arrow!");
                pileController.AddCard(new PoisonArrow());
                return;
            }

            CombatMechanics.UseEnergy(user, 1);
            ArrowEffect(user, target);
            bow.BowEffect(user, target);

            // Use the bow
            Card bowCard = bow as Card;
            bowCard.uses--;
            if (bowCard.uses <= 0)
            {
                int index = pileController.hand.IndexOf(bowCard);
                pileController.RemoveCard(index);
                pileController.AddCard(new TempBow());
            }
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
            pileController.AddCard(new PoisonArrow());
        }
    }
}

[System.Serializable]
public class LightningArrow : Card, Arrow, IStatusEffect
{
    public int damage { get; set; } = 3; // Default damage value
    public Status status { get; set; } = new Paralysis(); // Default status effect
    public LightningArrow()
    {
        cardName = "Lightning Arrow";
        description = "An arrow infused with lightning. It deals damage and stuns the target.";
        tooltip = "This arrow applies paralysis to the target.";
        itemType = ItemType.Arrow;
        uses = 1;
        rarity = Rarity.Legendary;
    }

    public void ArrowEffect(CombatDetails user, CombatDetails target)
    {
        AnimateDamageEffect(damage, target);
        CombatMechanics.TakeDamage(target, user, damage);
        target.statusEffects.Add(status);
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        PileController pileController = GameObject.Find("CombatManager").GetComponent<CombatManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Bow bow)
        {
            if (bow is TempBow)
            {
                Debug.Log("You need a real bow to use this arrow!");
                pileController.AddCard(new LightningArrow());
                return;
            }

            CombatMechanics.UseEnergy(user, 1);
            ArrowEffect(user, target);
            bow.BowEffect(user, target);

            // Use the bow
            Card bowCard = bow as Card;
            bowCard.uses--;
            if (bowCard.uses <= 0)
            {
                int index = pileController.hand.IndexOf(bowCard);
                pileController.RemoveCard(index);
                pileController.AddCard(new TempBow());
            }
        }
        else
        {
            Debug.Log("You need a bow to use this arrow!");
            pileController.AddCard(new LightningArrow());
        }
    }
}
