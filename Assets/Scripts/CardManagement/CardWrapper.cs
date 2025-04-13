using UnityEngine;
using UnityEngine.EventSystems;

public class CardWrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    public Vector2 baseScale;
    const float SCALEFACTOR = 1.1f;
    public UnityEngine.UI.Image image;
    [HideInInspector] public RectTransform parentAfterDrag;
    public PileController pileController;
    public Card card;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        baseScale = rectTransform.localScale;
        pileController = GameObject.Find("Deck").GetComponent<PileController>();
        image = GetComponent<UnityEngine.UI.Image>();
    }
    public void SetCard(Card newCard)
    {
        card = newCard;
        // Update the UI or other components to reflect the new card
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = baseScale * SCALEFACTOR;
        rectTransform.SetAsLastSibling(); // Bring the card to the front
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = baseScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = rectTransform.parent as RectTransform;

        Canvas canvas = GetComponentInParent<Canvas>();
        rectTransform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Method 2
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Update the card's position to match the cursor
        rectTransform.localPosition = localPoint;
        rectTransform.localScale = Vector3.one * 3; // Reset scale to 1 for dragging
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        // Reset the parent to the original one
        rectTransform.SetParent(parentAfterDrag);
        rectTransform.localPosition = UnityEngine.Vector3.zero;

        // Optionally, update the pile or other logic
        pileController.UpdatePile();
    }
}
