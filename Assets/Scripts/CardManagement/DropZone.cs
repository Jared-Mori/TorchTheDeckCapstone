using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] RectTransform HandTransform;
    [SerializeField] PileController pc;

    public void OnDrop(PointerEventData eventData)
    {
        // Call the async logic without awaiting it
        _ = HandleDropAsync(eventData);
    }

    private async Task HandleDropAsync(PointerEventData eventData)
    {
        CardWrapper cardWrapper = eventData.pointerDrag.GetComponent<CardWrapper>();

        if (cardWrapper.parentAfterDrag != HandTransform)
        {
            Debug.Log("Dropped back into Deck: " + eventData.pointerDrag.name);
            await pc.AddCard(cardWrapper.card); // Await the async AddCard method
            cardWrapper.parentAfterDrag.GetComponent<InventorySlot>().ClearCard();
            cardWrapper.transform.SetParent(cardWrapper.parentAfterDrag, true); // Set the parent of the card display to the hand transform
            cardWrapper.transform.localScale = cardWrapper.baseScale; // Reset the scale of the card display
        }

        await pc.UpdateHandAsync(); // Await the async UpdateHand method
    }
}
