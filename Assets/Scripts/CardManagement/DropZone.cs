using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] RectTransform HandTransform;
    [SerializeField] PileController pc;
    public void OnDrop(PointerEventData eventData)
    {
        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();
        
        if (cardWrapper.parentAfterDrag != HandTransform){
            Debug.Log("Dropped back into Deck: " + eventData.pointerDrag.name);
            pc.AddCard(cardWrapper.card);
            cardWrapper.parentAfterDrag.GetComponent<InventorySlot>().ClearCard();
            cardWrapper.transform.SetParent(cardWrapper.parentAfterDrag, true); // Set the parent of the card display to the hand transform
            cardWrapper.transform.localScale = cardWrapper.baseScale; // Reset the scale of the card display
        }

        pc.UpdateHand(); // Update the hand display
    }
}
