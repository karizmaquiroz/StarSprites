using System;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public FurnitureItem furnitureItem;
    // When something enters the trigger collider attached to this GameObject
    private void Start()
    {
        furnitureItem.inventoryIcon = gameObject.GetComponent<SpriteRenderer>().sprite;
        furnitureItem.itemName = gameObject.name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Get the InventoryManager component from the player
            InventoryManager inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>();
            Debug.Log(inventory);
            if (inventory != null)
            {
                // Add this item to the inventory
                inventory.AddItem(furnitureItem.inventoryIcon);
                // Optionally, disable the item so it’s no longer visible in the environment.
                // Alternatively, use Destroy(gameObject) if you don't need the reference later.
                gameObject.SetActive(false);
                Debug.Log(furnitureItem.itemName);
            }
        }
    }
}