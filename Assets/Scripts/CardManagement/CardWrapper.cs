using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardWrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    public Vector2 baseScale;
    const float SCALEFACTOR = 1.1f;
    public UnityEngine.UI.Image image;
    public RectTransform parentAfterDrag;
    public PileController pileController;
    public Card card;
    public TooltipManager tooltipManager;
    int siblingIndex;

    InputAction tooltipAction;
    bool isPointerOver = false;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        pileController = GameObject.Find("Deck").GetComponent<PileController>();
        tooltipManager = GameObject.Find("UI").GetComponent<TooltipManager>();
        image = GetComponent<UnityEngine.UI.Image>();

        tooltipAction = InputSystem.actions.FindAction("Crouch");
    }

    public void SetCard(Card newCard)
    {
        card = newCard;
        // Update the UI or other components to reflect the new card
    }

    public void Update()
    {
        if (isPointerOver) // Check if the tooltip action is triggered while the pointer is over the card
        {
            if (tooltipAction.IsPressed())
            {
                tooltipManager.ShowTooltip(); // Show the tooltip when the action is triggered
            }
            else
            {
                tooltipManager.HideTooltip(); // Hide the tooltip when the action is released
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered card wrapper");
        tooltipManager.SetTooltipText(card.tooltip); // Set the tooltip text to the card's description
        isPointerOver = true; // Set the flag to true
        baseScale = rectTransform.localScale; // Store the base scale
        siblingIndex = rectTransform.GetSiblingIndex(); // Store the sibling index
        rectTransform.localScale = baseScale * SCALEFACTOR;
        rectTransform.SetAsLastSibling(); // Bring the card to the front
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited card wrapper");
        isPointerOver = false; // Set the flag to false
        rectTransform.localScale = baseScale;
        rectTransform.SetSiblingIndex(siblingIndex); // Reset the sibling index to its original value
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = rectTransform.parent as RectTransform;

        Canvas canvas = GetComponentInParent<Canvas>();
        rectTransform.SetParent(canvas.GetComponent<RectTransform>(), true); // Set the parent to the canvas
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Update the card's position to match the cursor
        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        // Reset the parent to the original one
        rectTransform.SetParent(parentAfterDrag);
        pileController.UpdateHand(); // Update the hand display after dragging
    }

    public void ResetPosition()
    {

    }
}
