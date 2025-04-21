using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public PileController pileController;
    public GameObject cardObject;
    public CardWrapper cardWrapper;
    public SpriteManager spriteManager;
    public ItemType slotType;
    public Vector3 baseScale;

    public void Start()
    {
        cardWrapper.SetCard(new DefaultCard()); // Initialize with a default card
        baseScale = cardObject.transform.localScale; // Store the base scale of the card object
    }
    public void OnDrop(PointerEventData eventData)
    {
        CardWrapper newWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();
        if (newWrapper.card.itemType == ItemType.Item)
        {
            Debug.Log("Dropped card is not an equipment.");
            return;
        }
        else if (newWrapper.card.itemType != slotType)
        {
            Debug.Log("Invalid equipment type for this slot. Expected: " + slotType + ", but got: " + newWrapper.card.itemType);
            return;
        } else
        {
            DropCard(newWrapper);
        }
    }

    public void DropCard(CardWrapper newWrapper)
    {
        if (cardWrapper.card.itemType == ItemType.Default)
        {
            Debug.Log("Slot is empty. Adding new card.");
            SetCard(newWrapper.card);
            int index = pileController.hand.IndexOf(newWrapper.card);
            pileController.RemoveCard(index);
        }
        else
        {
            Debug.Log("Swapping cards: " + cardWrapper.card.cardName + " with " + newWrapper.card.cardName);
            Card oldCard = cardWrapper.card;
            Card newCard = newWrapper.card;

            int index = pileController.hand.IndexOf(newWrapper.card);
            pileController.RemoveCard(index);
            pileController.AddCard(oldCard);
            SetCard(newCard);
        }
    }

    public void SetCard(Card newCard)
    {
        // Update the UI or other components to reflect the new card
        if (newCard == null)
        {
            Debug.LogWarning("Attempted to set a null card. Clearing the slot instead.");
            ClearCard();
        }else
        {
            Debug.Log("Setting card: " + newCard.cardName);
            cardWrapper.SetCard(newCard);
            PileController.SetCardDisplay(cardObject);
            cardObject.SetActive(true);
        }
    }

    public void ClearCard()
    {
        Debug.Log(cardWrapper);
        cardObject.SetActive(false);
        cardWrapper.SetCard(new DefaultCard()); // Clear the card reference
        cardObject.transform.localScale = baseScale; // Reset the scale to the base scale
    }
}
