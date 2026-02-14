using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverAmount = 0.1f;      // how much bigger
    public float shrinkAmount = 0.05f;    // how much smaller
    public float scaleSpeed = 8f;

    private static ButtonHoverScaler currentHovered;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * scaleSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentHovered = this;

        foreach (ButtonHoverScaler button in FindObjectsByType<ButtonHoverScaler>(FindObjectsSortMode.None))
        {
            if (button == this)
                button.targetScale = button.originalScale + Vector3.one * hoverAmount;
            else
                button.targetScale = button.originalScale - Vector3.one * shrinkAmount;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (ButtonHoverScaler button in FindObjectsByType<ButtonHoverScaler>(FindObjectsSortMode.None))
        {
            button.targetScale = button.originalScale;
        }
    }
}
