using UnityEngine;
using UnityEngine.EventSystems;

public class PlayZone : MonoBehaviour, IDropHandler
{
    public CombatManager combatManager;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped on PlayZone: " + eventData.pointerDrag.name);

        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();
        if (cardWrapper != null)
        {
            cardWrapper.parentAfterDrag = transform as RectTransform;  
        }
    }

    public void EquipmentOptions(CardWrapper cardWrapper)
    {
        // If Equipment is played, Player can choose to use equipment's effect,
        // or equip a different piece of equipment in that slot
    }

    public void PlayCard(CardWrapper cardWrapper)
    {
        // If a card is played, it will be removed from the player's hand and added to the play zone
        // The card will then be activated and its effect will be applied
        cardWrapper.card.Effect(combatManager.playerDetails, combatManager.enemyDetails);
        
        
    }

    public void EquipCard(CardWrapper cardWrapper)
    {
        // If a card is equipped, it will be removed from the player's hand and added to the play zone
        // The card will then be activated and its effect will be applied
    }
}
