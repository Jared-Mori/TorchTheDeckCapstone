using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayZone : MonoBehaviour, IDropHandler
{
    public CombatManager combatManager;
    public GameObject optionsPanel; // Reference to the options panel
    public Button playButton;       // Reference to the "Play Card" button
    public Button equipButton;      // Reference to the "Equip Card" button

    private CardWrapper currentCardWrapper; // Store the card being interacted with

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped on PlayZone: " + eventData.pointerDrag.name);

        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();
        if (cardWrapper != null)
        {
            currentCardWrapper = cardWrapper; // Store the card for later use
            DisplayOptions(currentCardWrapper);     // Show the options panel
        }
    }

    public void DisplayOptions(CardWrapper cardWrapper)
    {
        Card card = cardWrapper.card;

        // Automatically play the card if it's an item
        if (card.itemType == ItemType.Item)
        {
            PlayCard(cardWrapper);
            return;
        }

        // if the card is a weapon, bow, or shield, show the options panel
        if (card.itemType == ItemType.Bow || card.itemType == ItemType.Weapon || card.itemType == ItemType.Shield)
        {
            // Show the options panel for equipping or using the card
            optionsPanel.SetActive(true);

            // Assign button click listeners
            playButton.onClick.RemoveAllListeners();
            equipButton.onClick.RemoveAllListeners();

            playButton.onClick.AddListener(() => PlayCard(cardWrapper));
            equipButton.onClick.AddListener(() => EquipCard(cardWrapper));
        }
        else
        {
            // If the card is not a weapon, bow, shield or item, offer to equip another
            EquipCard(cardWrapper);
        }
    }

    public void PlayCard(CardWrapper cardWrapper)
    {
        cardWrapper.card.Effect(combatManager.playerDetails, combatManager.enemyDetails);

        // Remove the card from the player's hand
        if (cardWrapper.card.Use())
        {
            int index = cardWrapper.pileController.hand.IndexOf(cardWrapper.card);
            cardWrapper.pileController.RemoveCard(index);
        }

        // Hide the options panel
        optionsPanel.SetActive(false);
    }

    public void EquipCard(CardWrapper cardWrapper)
    {
        // Logic for equipping the card
        Debug.Log("Equipping card: " + cardWrapper.card.cardName);

        // Hide the options panel
        optionsPanel.SetActive(false);
    }
}
