using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
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
        }
        else
        {
            Debug.Log("Invalid card type or no card dropped.");
        }
    }
}
