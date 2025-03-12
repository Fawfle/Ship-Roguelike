using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory/Inventory")]
public class InventoryData : ScriptableObject
{
    public List<Item> items = new();

    [System.Serializable]
    public class Item
    {
        public InventoryItemData data;
        public Vector2Int position;
    }
    
}
