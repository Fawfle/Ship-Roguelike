using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryGrid : MonoBehaviour
    {
        public Canvas canvas;

        public Vector2Int gridSize;
        public float cellSize = 25;
        public float spacing = 2;

        /// <summary>
        /// Size of cell and spacing
        /// </summary>
        public float width => effectiveCellSize * gridSize.x;
        public float height => effectiveCellSize * gridSize.y;

        [HideInInspector] public float effectiveCellSize;
        [HideInInspector] public Bounds bounds;

        public Transform gridParent;
        public GameObject cellPrefab;

        public InventoryGridCell[,] grid;

        private InventoryGridCell hoveredCell = null;

        public List<InventoryItem> items = new();
        private InventoryItem selected;

        private void Awake()
        {
            GenerateGrid();

            UpdateGridItems();
        }

        private void Update()
        {
            if (selected != null)
            {
                selected.transform.position = InputManager.PointerWorldPosition + selected.dragOffset;
            }

            InventoryGridCell cell = GetCellAtPointer();
            if (cell != null)
            {
                if (hoveredCell != null) hoveredCell.GetComponent<Image>().color = Color.white;
                hoveredCell = grid[cell.position.x, cell.position.y];
            }
            else
            {
                if (hoveredCell != null) hoveredCell.GetComponent<Image>().color = Color.white;
                hoveredCell = null;
            }

            if (hoveredCell != null) hoveredCell.GetComponent<Image>().color = Color.green;
        }

        public InventoryGridCell GetCellAtPointer()
        {
            return GetCellAtWorldPosition(InputManager.PointerWorldPosition);
        }

        public InventoryGridCell GetCellAtWorldPosition(Vector2 position)
        {
            Vector2 localPosition = position - (Vector2)bounds.center + (Vector2)bounds.extents;

            // out of bounds
            if (localPosition.x < 0f || localPosition.x > bounds.size.x ||
                localPosition.y < 0f || localPosition.y > bounds.size.y)
                return null;

            Vector2Int pos = Vector2Int.FloorToInt(localPosition / effectiveCellSize);
            return grid[pos.x, pos.y];
        }

        public void SelectInventoryItem(InventoryItem item)
        {
            selected = item;
        }

        public void DeselectInventoryItem(InventoryItem item)
        {
            if (selected != item) return;

            InventoryGridCell cell = GetCellAtWorldPosition(InputManager.PointerWorldPosition + item.dragOffset);

            if (cell != null && IsValidPosition(cell.position, item))
            {
                // update other stuff
                selected.SetGridPosition(grid[cell.position.x, cell.position.y]);
                UpdateGridItems();
            }
            else
            {
                selected.ResetPosition();
            }


            selected = null;
        }

        public bool IsValidPosition(Vector2Int position, InventoryItem item)
        {
            foreach (InventoryItemData.Cell cell in item.itemData.cells)
            {
                var pos = cell.position + position;

                if (pos.x < 0 || pos.y < 0 || pos.x >= gridSize.x || pos.y >= gridSize.y) return false;

                if (grid[pos.x, pos.y].item != null && grid[pos.x, pos.y].item != selected) return false;
            }

            return true;
        }

        public void GenerateGrid()
        {
            ClearGrid();

            grid = new InventoryGridCell[gridSize.x, gridSize.y];

            effectiveCellSize = cellSize + spacing;
            bounds = new Bounds(gridParent.position, (Vector2)(gridSize) * effectiveCellSize);

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var cell = Instantiate(cellPrefab, gridParent.transform).AddComponent<InventoryGridCell>();
                    cell.name += $" ({x},{y})";

                    grid[x, y] = cell;
                    cell.position = new Vector2Int(x, y);
                    cell.GetComponent<RectTransform>().sizeDelta = Vector2.one * cellSize;
                    cell.transform.localPosition = ((Vector2)cell.position - (gridSize - Vector2.one) / 2) * effectiveCellSize;
                }
            }

            UpdateGridItems();
        }

        public void ClearGrid()
        {
            for (int i = gridParent.transform.childCount - 1; i >= 0; i--)
            {
                if (!Application.isPlaying) DestroyImmediate(gridParent.transform.GetChild(i).gameObject);
                else Destroy(gridParent.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Updates the position of items on grid
        /// </summary>
        public void UpdateGridItems()
        {
            foreach (InventoryGridCell cell in grid)
            {
                cell.ResetItem();
            }

            foreach (InventoryItem inventoryItem in items)
            {
                inventoryItem.ResetBonuses();
                inventoryItem.transform.position = grid[inventoryItem.position.x, inventoryItem.position.y].transform.position;

                foreach (InventoryItemData.Cell cell in inventoryItem.itemData.cells)
                {
                    var gridCell = grid[inventoryItem.position.x + cell.position.x, inventoryItem.position.y + cell.position.y];
                    gridCell.item = inventoryItem;
                    gridCell.itemDataCell = cell;

                }
            }

            UpdateGridItemColumns();
            UpdateGridItemRows();
            UpdateGridItemAdjacent();
        }

        private void UpdateGridItemColumns()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                InventoryGridCell[] column = new InventoryGridCell[gridSize.y];

                for (int y = 0; y < gridSize.y; y++)
                {
                    if (grid[x, y].item == null) goto CheckNextColumn;
                    column[y] = grid[x, y];
                }

                foreach (InventoryGridCell cell in column)
                {
                    cell.item.inColumn.Add(cell.itemDataCell);
                }

            CheckNextColumn:;
            }
        }

        private void UpdateGridItemRows()
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                InventoryGridCell[] row = new InventoryGridCell[gridSize.x];

                for (int x = 0; x < gridSize.x; x++)
                {
                    if (grid[x, y].item == null) goto CheckNextRow;
                    row[x] = grid[x, y];
                }

                foreach (InventoryGridCell cell in row)
                {
                    cell.item.inRow.Add(cell.itemDataCell);
                }

            CheckNextRow:;
            }
        }

        private static Vector2Int[] ADJACENT_OFFSETS = new Vector2Int[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

        private void UpdateGridItemAdjacent()
        {
            foreach (InventoryGridCell cell in grid)
            {
                if (cell.item == null) continue;

                foreach (Vector2Int offset in ADJACENT_OFFSETS)
                {
                    Vector2Int adjacentPosition = cell.position + offset;

                    if (adjacentPosition.x < 0 || adjacentPosition.x >= gridSize.x || adjacentPosition.y < 0 || adjacentPosition.y >= gridSize.y) continue;

                    var adjacentItem = grid[adjacentPosition.x, adjacentPosition.y].item;
                    if (adjacentItem != null && adjacentItem != cell.item)
                    {
                        cell.item.adjacent.Add(cell.itemDataCell);
                        break;
                    }
                }
            }
        }
    }
}