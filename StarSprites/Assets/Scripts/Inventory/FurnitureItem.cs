using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureItem", menuName = "Scripts/Inventory/FurnitureItem", order = 1)]
public class FurnitureItem : ScriptableObject
{
    [Tooltip("Icon sprite to display in the inventory UI.")]
    public Sprite inventoryIcon;
    public string itemName;
}