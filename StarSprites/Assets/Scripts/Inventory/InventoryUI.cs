using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    [Tooltip("Array of inventory UI buttons representing each slot.")]
    public Button[] inventorySlots;

    [Tooltip("Sprite to display if a slot is empty.")]
    public Sprite blankSprite;

    [Tooltip("Reference to the player's InventoryManager.")]
    public InventoryManager inventoryManager;

    [Header("Editor Toggle")]
    [Tooltip("Check to make the inventory UI visible.")]
    public bool isVisible = false;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Ensure initial visibility matches the Inspector toggle
        ApplyVisibility();
    }

    void OnValidate()
    {
        // Called whenever you change a serialized field in the Inspector
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        ApplyVisibility();
    }

    /// <summary>
    /// Refreshes the inventory icons. Call this after adding/removing items.
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Image slotImage = inventorySlots[i].GetComponent<Image>();

            if (i < inventoryManager.inventoryItems.Count)
            {
                var itemGO = inventoryManager.inventoryItems[i];
                var furnitureItem = itemGO.GetComponent<FurnitureItem>();
                slotImage.sprite = (furnitureItem != null && furnitureItem.inventoryIcon != null)
                    ? furnitureItem.inventoryIcon
                    : blankSprite;
            }
            else
            {
                slotImage.sprite = blankSprite;
            }
        }
    }

    /// <summary>
    /// Shows the UI and makes it interactable.
    /// </summary>
    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isVisible = true;
    }

    /// <summary>
    /// Hides the UI and makes it non-interactable.
    /// </summary>
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isVisible = false;
    }

    /// <summary>
    /// Toggles between Show and Hide at runtime.
    /// </summary>
    public void Toggle()
    {
        if (isVisible) Hide();
        else Show();
    }

    /// <summary>
    /// Applies the current value of isVisible.
    /// </summary>
    private void ApplyVisibility()
    {
        if (isVisible) Show();
        else Hide();
    }
}
