using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    PileController pileController;
    public GameObject cardObject;
    public CardWrapper cardWrapper;
    public SpriteManager spriteManager;
    public ItemType slotType;

    public void Start()
    {
        pileController = GameObject.Find("Deck").GetComponent<PileController>();
        spriteManager = GameObject.Find("Sprite Manager").GetComponent<SpriteManager>();
        cardWrapper = cardObject.GetComponent<CardWrapper>();
        cardWrapper.card = null; // Initialize with no card
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
        if (cardWrapper.card == null)
        {
            SetCard(newWrapper.card);
            int index = pileController.hand.IndexOf(newWrapper.card);
            pileController.RemoveCard(index);
        }
        else
        {
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
            Debug.Log("New card is null, cannot set card.");
            return;
        }
        cardWrapper.SetCard(newCard);
        PileController.SetCardDisplay(cardObject);
        cardObject.SetActive(true);
    }

    public void ClearCard()
    {
        cardObject.SetActive(false);
        cardWrapper.card = null; // Clear the card reference
    }
}
