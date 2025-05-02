using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    [SerializeField] RectTransform HandTransform;
    public PileController pileController;
    public void OnDrop(PointerEventData eventData)
    {
        _ = OnDropAsync(eventData); // Call the async method without awaiting it
    }

    public async Task OnDropAsync(PointerEventData eventData)
    {
        CardWrapper droppedCard = eventData.pointerDrag.GetComponent<CardWrapper>();
        if (droppedCard != null && droppedCard.card.itemType != ItemType.Default)
        {
            int index = pileController.hand.IndexOf(droppedCard.card);
            await pileController.RemoveCard(index);
            await pileController.UpdateHandAsync();
        }
        else
        {
            Debug.Log("Invalid card type or no card dropped.");
        }

        if (droppedCard.parentAfterDrag != HandTransform)
        {
            droppedCard.parentAfterDrag.GetComponent<InventorySlot>().ClearCard();
            droppedCard.transform.SetParent(droppedCard.parentAfterDrag, true); // Set the parent of the card display to the hand transform
            droppedCard.transform.localScale = droppedCard.baseScale; // Reset the scale of the card display
        }
    }
}
