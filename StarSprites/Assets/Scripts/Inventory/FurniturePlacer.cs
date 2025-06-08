using UnityEngine;
using UnityEngine.EventSystems;

 public class FurniturePlacer : MonoBehaviour
 {
     [Tooltip("Reference to the player's InventoryManager")]
     public InventoryManager inventoryManager;

     // The furniture item currently selected for placement
     private Sprite currentFurniture;
     
     private GameObject draggableFurniture;

     void Start()
     {
         // Ensure the InventoryManager is set
         inventoryManager = InventoryManager.Instance;
         if (inventoryManager == null)
         {
             Debug.LogError("InventoryManager reference not set on FurniturePlacer.");
         }
     }

     void Update()
     {
         // If an item is selected, update its position to follow the mouse
         if (currentFurniture != null)
         {
             UpdateFurniturePosition();

             // When the left mouse button is clicked (and not over UI), finalize placement
             if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
             {
                 // Optionally, you can add further logic here (e.g., snapping to grid or playing a sound)
                 currentFurniture = null;
             }
         }
     }

     // Call this method (for example, via an inventory UI button) to select and place an item from the inventory.
     public void SelectFurnitureFromInventory(int index)
     {
         if (inventoryManager == null)
         {
             Debug.LogError("InventoryManager reference not set on FurniturePlacer.");
             return;
         }

         if (index < 0 || index >= inventoryManager.inventoryItems.Count)
         {
             Debug.LogError("Invalid index for inventory.");
             return;
         }

         // Retrieve the selected item from the inventory.
         currentFurniture = inventoryManager.inventoryItems[index];
         // Remove the item from the inventory so it can’t be placed again.
         inventoryManager.inventoryItems.RemoveAt(index);

         draggableFurniture = new GameObject("Furniture");
         SpriteRenderer spriteRenderer = draggableFurniture.AddComponent<SpriteRenderer>();
         spriteRenderer.sprite = currentFurniture;
         draggableFurniture.AddComponent<BoxCollider2D>();
     }

     // Makes the currently selected furniture follow the mouse position using a raycast.
     void UpdateFurniturePosition()
     {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;

         // Assumes there's a collider (like on the floor) to determine the placement point.
         if (Physics.Raycast(ray, out hit))
         {
             draggableFurniture.transform.position = hit.point;
         }
         SaveFurniturePlacement();
     }

     // Prevents placing the item when clicking UI elements.
     bool IsPointerOverUI()
     {
         return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
     }

     void SaveFurniturePlacement()
     {
         var placement = new GameSaveData.FurniturePlacement
         {
             furnitureID = draggableFurniture.name,
             x = draggableFurniture.transform.position.x,
             y = draggableFurniture.transform.position.y,
             z = draggableFurniture.transform.position.z
         };
         SaveManager.Instance.currentData.furniturePlacements.Add(placement);
         SaveManager.Instance.currentData.furnitureCollected = SaveManager.Instance.currentData.furniturePlacements.Count;
         SaveManager.Instance.SaveGame();
     }
}
