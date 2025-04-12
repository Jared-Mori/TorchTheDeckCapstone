using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped back into Deck: " + eventData.pointerDrag.name);

        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();
        if (cardWrapper.parentAfterDrag.GetComponent<InventorySlot>() != null)
        {
            cardWrapper.pileController.AddCard(cardWrapper.card);
            cardWrapper.parentAfterDrag.GetComponent<InventorySlot>().ClearCard();
        }
    }
}
