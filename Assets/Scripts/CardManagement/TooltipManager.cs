using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltip; // The tooltip panel
    public GameObject tooltipText; // The text component for the tooltip
    public TextMeshProUGUI text;
    private RectTransform tooltipRect;

    public void Start()
    {
        tooltipRect = tooltip.GetComponent<RectTransform>();
        text = tooltipText.GetComponent<TextMeshProUGUI>();
        HideTooltip();
    }

    private void Update()
    {
        if (tooltip.activeSelf)
        {
            Vector2 localPoint;
            RectTransform canvasRect = tooltip.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                canvasRect.GetComponent<Canvas>().worldCamera,
                out localPoint
            );

            // Clamp the tooltip position to stay within the canvas bounds
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(localPoint.x, canvasRect.rect.xMin, canvasRect.rect.xMax),
                Mathf.Clamp(localPoint.y, canvasRect.rect.yMin, canvasRect.rect.yMax)
            );

            tooltipRect.localPosition = clampedPosition;
        }
    }
    public void SetTooltipText(string text)
    {
        Debug.Log("Setting tooltip text: " + text);
        this.text.text = text;
    }
    public void ShowTooltip()
    {
        Debug.Log("Showing tooltip: " + text.text);
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        Debug.Log("Hiding tooltip");
        tooltip.SetActive(false);
    }
}
