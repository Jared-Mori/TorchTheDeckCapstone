using UnityEngine;
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
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public Card card;
    public SpriteManager sm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void getSpriteManager()
    {
        sm = GameObject.Find("Sprite Manager").GetComponent<SpriteManager>();
    }

    public void SetCard(Card newCard)
    {
        if (sm == null)
        {
            getSpriteManager();
        }
        
        card = newCard;
        image = GetComponent<UnityEngine.UI.Image>();
        image.sprite = sm.GetSprite(card.cardName);
    }
}
