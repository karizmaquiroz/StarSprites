using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Tooltip("Array of inventory UI buttons representing each slot.")]
    public Button[] inventorySlots;

    [Tooltip("Sprite to display if a slot is empty.")]
    public Sprite blankSprite;

    [Tooltip("Reference to the player's InventoryManager.")]
    public InventoryManager inventoryManager;

    // Call this method to refresh the inventory UI. 
    // You may call it after adding or removing an item.
    public void RefreshUI()
    {
        // Loop over all the inventory slot buttons.
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Get the Image component from the button.
            Image slotImage = inventorySlots[i].GetComponent<Image>();

            // Check if there is an item in this inventory slot index.
            if (i < inventoryManager.inventoryItems.Count)
            {
                // Retrieve the item from the player's inventory.
                GameObject itemGO = inventoryManager.inventoryItems[i];
                FurnitureItem furnitureItem = itemGO.GetComponent<FurnitureItem>();

                // If the item has an assigned icon, set it; otherwise, use the blank sprite.
                if (furnitureItem != null && furnitureItem.inventoryIcon != null)
                {
                    slotImage.sprite = furnitureItem.inventoryIcon;
                }
                else
                {
                    slotImage.sprite = blankSprite;
                }
            }
            else
            {
                // If there's no item at this slot, set to blank.
                slotImage.sprite = blankSprite;
            }
        }
    }
}