using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(CanvasGroup))]
public class HUDVisibility : MonoBehaviour
{
    [Header("HUD Visibility")]
    [Tooltip("Toggle to show or hide the HUD immediately in both Edit and Play modes.")]
    public bool isVisible = true;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // grab (or add) the CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        ApplyVisibility();
    }

    void OnValidate()
    {
        // called whenever 'isVisible' is flipped in the Inspector
        canvasGroup = GetComponent<CanvasGroup>();
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        // alpha controls render; interactable + blocksRaycasts control input
        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
}
