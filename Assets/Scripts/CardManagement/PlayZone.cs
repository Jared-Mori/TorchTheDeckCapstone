using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayZone : MonoBehaviour, IDropHandler
{
    public CombatManager combatManager;
    public GameObject optionsPanel; // Reference to the options panel
    public GameObject EquipOptionsPanel; // Reference to the equip options panel
    public GameObject EquipOptionPrefab, cardPrefab; // Reference to the play options panel
    public Button playButton;       // Reference to the "Play Card" button
    public Button equipButton;      // Reference to the "Equip Card" button

    private CardWrapper currentCardWrapper; // Store the card being interacted with

    private void Start()
    {
        // Ensure the options panel is hidden at the start
        optionsPanel.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();

        if (combatManager.playerDetails.energy <= 0)
        {
            Debug.Log("Not enough energy to play a card.");
            return; // Exit if the player has no energy
        }
        
        if (combatManager.isPlayerTurn && cardWrapper != null)
        {
            currentCardWrapper = cardWrapper; // Store the card for later use
            DisplayOptions(currentCardWrapper);     // Show the options panel
        }
    }

    public void DisplayOptions(CardWrapper cardWrapper)
    {
        Card card = cardWrapper.card;

        // Automatically play the card if it's an item
        if (card.itemType == ItemType.Item || card.itemType == ItemType.Arrow)
        {
            Debug.Log("Playing item card: " + card.cardName);
            PlayCard(cardWrapper);
            return;
        }

        // if the card is a weapon, bow, or shield, show the options panel
        if (card.itemType == ItemType.Bow || card.itemType == ItemType.Weapon || card.itemType == ItemType.Shield)
        {
            // Check if the card is a temporary card
            // if it is, it can only be used for the equip option
            if (card is TempBow || card is TempWeapon || card is TempShield)
            {
                EquipCard(cardWrapper);
                return;
            }

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
            EquipCard(cardWrapper);
        }
    }

    public void PlayCard(CardWrapper cardWrapper)
    {
        cardWrapper.card.Effect(combatManager.playerDetails, combatManager.enemyDetails);

        // Remove the card from the player's hand
        if (cardWrapper.card.Use())
        {
            ItemType itemType = cardWrapper.card.itemType;
            int index = cardWrapper.pileController.hand.IndexOf(cardWrapper.card);
            Debug.Log("Card used up: " + cardWrapper.card.cardName);
            cardWrapper.pileController.RemoveCard(index);
            switch (itemType)
            {
                case ItemType.Weapon:
                    combatManager.pileController.AddCard(new TempWeapon());
                    break;
                case ItemType.Bow:
                    combatManager.pileController.AddCard(new TempBow());
                    break;
                default:
                    break;
            }
        }

        // Hide the options panel
        optionsPanel.SetActive(false);
    }

    public void EquipCard(CardWrapper cardWrapper)
    {
        CombatMechanics.UseEnergy(combatManager.playerDetails, 1); // Use 1 energy for equipping a card
        // Hide the options panel
        optionsPanel.SetActive(false);

        // Clear any existing options in the EquipOptionsPanel
        foreach (Transform child in EquipOptionsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Get the ItemType of the card to be equipped
        ItemType itemType = cardWrapper.card.itemType;

        // Search the player's deck for cards of the same ItemType
        foreach (Card card in combatManager.playerDetails.deck)
        {
            if (card.itemType == itemType)
            {
                // Instantiate the EquipOptionPrefab as a child of EquipOptionsPanel
                GameObject equipOption = Instantiate(EquipOptionPrefab, EquipOptionsPanel.transform);

                // Instantiate the cardPrefab as a child of the equipOption
                GameObject cardObject = Instantiate(cardPrefab, equipOption.transform);

                // Find the CardWrapper component in the cardPrefab and set its card
                CardWrapper optionCardWrapper = cardObject.GetComponent<CardWrapper>();
                if (optionCardWrapper != null)
                {
                    optionCardWrapper.SetCard(card);

                    // Get the Image component of the cardPrefab and set raycastTarget to false
                    Image cardImage = cardObject.GetComponent<Image>();
                    if (cardImage != null)
                    {
                        cardImage.raycastTarget = false;
                    }
                }

                // Set the card display for the cardObject
                PileController.SetCardDisplay(cardObject);

                // Add a listener to the button to equip the selected card
                Button equipButton = equipOption.GetComponent<Button>();
                if (equipButton != null)
                {
                    equipButton.onClick.AddListener(() =>{ OnEquip(card);});
                }
            }
        }

        // Show the EquipOptionsPanel
        EquipOptionsPanel.SetActive(true);
    }

    public void OnEquip(Card card)
    {
        PileController pileController = GameObject.Find("Deck").GetComponent<PileController>();

        // Check if a card of the same ItemType exists in the hand
        ItemType itemType = card.itemType;
        Card cardInHand = null;

        foreach (Card handCard in pileController.hand)
        {
            if (handCard.itemType == itemType)
            {
                cardInHand = handCard;
                break;
            }
        }

        // If a card of the same ItemType exists in the hand, move it to the deck
        if (cardInHand != null)
        {
            Debug.Log($"Found card of the same ItemType in hand: {cardInHand.cardName}. Moving it to the deck.");
            int index = pileController.hand.IndexOf(cardInHand);
            pileController.RemoveCard(index);
            combatManager.playerDetails.deck.Add(cardInHand);
        }

        // Add the passed card to the pileController hand
        Debug.Log($"Adding card to hand: {card.cardName}");
        pileController.AddCard(card);

        // Remove the equipped card from the deck
        combatManager.playerDetails.deck.Remove(card);
        PlayerLogic.PurgeTempCards(combatManager); // Call the method to purge temporary cards

        // Hide the EquipOptionsPanel
        EquipOptionsPanel.SetActive(false);
    }
}
