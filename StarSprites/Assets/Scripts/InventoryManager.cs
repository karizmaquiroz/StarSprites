using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // A list to store the collected items. In a more complex system, you might have a custom Item class.
    public List<GameObject> inventoryItems = new List<GameObject>();

    // Call this method to add an item to the inventory.
    public void AddItem(GameObject item)
    {
        inventoryItems.Add(item);
        Debug.Log("Added to inventory: " + item.name);
    }
}

