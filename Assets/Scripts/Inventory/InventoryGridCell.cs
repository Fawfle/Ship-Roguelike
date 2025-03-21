using UnityEngine;

namespace Inventory
{
    public class InventoryGridCell : MonoBehaviour
    {
        public InventoryItem item;
        public InventoryItemData.Cell itemDataCell;

        public Vector2Int position;

        public void ResetItem()
        {
            item = null;
            itemDataCell = null;
        }
    }
}