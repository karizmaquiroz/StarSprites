using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    // A list to store the collected items. In a more complex system, you might have a custom Item class.
    public List<GameObject> inventoryItems = new List<GameObject>();
    public InventoryUI inventoryUI;

    // Call this method to add an item to the inventory.

    private void Awake()
    {
        // Ensure there's only one instance of InventoryManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(GameObject item)
    {
        inventoryItems.Add(item);
        inventoryUI.RefreshUI();
        Debug.Log("Added to inventory: " + item.name);
    }
}

