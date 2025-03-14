using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Inventory Drag and Drop functionality
    public UnityEngine.UI.Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        transform.SetParent(parentAfterDrag);
    }

    public Card card;
    public void SetCard(Card newCard)
    {
        Debug.Log("Setting card: " + newCard.cardName);
        Debug.Log("Card Sprite: " + newCard.artwork);
        card = newCard;
        image = GetComponent<UnityEngine.UI.Image>();
        image.sprite = card.artwork;
    }
}
