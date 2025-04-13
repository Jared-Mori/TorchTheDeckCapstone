using UnityEngine;

/// <summary>
/// Bow classes
/// </summary>

public interface Bow
{
    void BowEffect(CombatDetails user, CombatDetails target);
}
[System.Serializable]
public class Longbow : Card
{
    public Longbow()
    {
        cardName = "Longbow";
        description = "A simple bow. It's not very powerful, but it's better than nothing.";
        itemType = ItemType.Bow;
        uses = 3;
        rarity = 1;
    }

    public void BowEffect(CombatDetails user, CombatDetails target)
    {
        Debug.Log("This bow does nothing extra!");
    }

    public override void Effect(CombatDetails user, CombatDetails target)
    {
        Debug.Log(cardName + " effect triggered!");
        CombatMechanics.UseEnergy(user, 1);
        PileController pileController = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pileController;
        var card = pileController.GetEquippedCard(ItemType.Bow);
        if (card != null && card.card is Arrow arrow)
        {
            arrow.ArrowEffect(user, target);
            BowEffect(user, target);
            
        }
        else
        {
            Debug.Log("You need an arrow to use this bow!");
        }
    }
}
