using UnityEngine;
using UnityEngine.EventSystems;

public class FurniturePlacer : MonoBehaviour
{
    [Tooltip("Assign your furniture prefabs here")]
    public GameObject[] furniturePrefabs;

    private GameObject currentFurniture;

    void Update()
    {
        // If an item is currently selected, update its position
        if (currentFurniture != null)
        {
            UpdateFurniturePosition();

            // When the left mouse button is pressed (and not clicking on UI), place the item
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                currentFurniture = null; // Item is now placed; you may add further logic here (e.g., saving state)
            }
        }
    }

    // Call this method from your UI (e.g., button click) to select a furniture item.
    public void SelectFurniture(int index)
    {
        if (index < 0 || index >= furniturePrefabs.Length)
        {
            Debug.LogError("Invalid furniture index selected.");
            return;
        }

        // If an item is already selected, remove it
        if (currentFurniture != null)
        {
            Destroy(currentFurniture);
        }

        // Instantiate the selected furniture at a default position
        currentFurniture = Instantiate(furniturePrefabs[index], Vector3.zero, Quaternion.identity);
    }

    // Updates the current furniture's position to follow the mouse
    void UpdateFurniturePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Assumes there is a collider on the floor/environment
        if (Physics.Raycast(ray, out hit))
        {
            currentFurniture.transform.position = hit.point;
        }
    }

    // Checks if the mouse is over a UI element to prevent interfering with placement
    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}