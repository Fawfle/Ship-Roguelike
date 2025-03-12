using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public GridLayoutGroup gridLayout;
    public GameObject cellPrefab;

	public InventoryGridCell[,] grid;

    private InventoryGridCell hoveredCell = null;

    public List<InventoryItem> items = new();
    private InventoryItem selected;

	private void Awake()
	{
        UpdateGridSize();

        GenerateGrid();
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

        if (cell != null && IsValidPosition(cell.position, selected.itemData.cells))
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

    public bool IsValidPosition(Vector2Int position, List<Vector2Int> cells)
    {
        foreach (Vector2Int cell in cells)
        {
            var pos = cell + position;

			if (pos.x < 0 || pos.y < 0 || pos.x >= gridSize.x || pos.y >= gridSize.y) return false;

			if (grid[pos.x, pos.y].item != null && grid[pos.x, pos.y].item != selected) return false;
        }

        return true;
    }

	public void GenerateGrid()
    {
        ClearGrid();

        gridLayout.constraintCount = gridSize.x;

        grid = new InventoryGridCell[gridSize.x, gridSize.y];

		for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            var cell = Instantiate(cellPrefab, gridLayout.transform).AddComponent<InventoryGridCell>();
            int x = i % gridSize.x;
			int y = (gridSize.y - 1) - i / gridSize.y;

			grid[x, y] = cell;
            cell.position = new Vector2Int(x, y);

		}

        UpdateGridItems();
    }

    public void ClearGrid()
    {
        for (int i = gridLayout.transform.childCount - 1; i >= 0; i--)
        {
            if (!Application.isPlaying) DestroyImmediate(gridLayout.transform.GetChild(i).gameObject);
            else Destroy(gridLayout.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateGridSize()
    {
		gridLayout.cellSize = cellSize * Vector2.one;
		gridLayout.spacing = spacing * Vector2.one;

		effectiveCellSize = cellSize + spacing;
		bounds = new Bounds(transform.position, (Vector2)(gridSize) * effectiveCellSize);
	}

    /// <summary>
    /// Updates the position of items on grid
    /// </summary>
    public void UpdateGridItems()
    {
        foreach (InventoryGridCell cell in grid)
        {
            cell.item = null;
        }

        foreach (InventoryItem inventoryItem in items)
        {
            foreach (Vector2Int cell in inventoryItem.itemData.cells)
            {
                grid[inventoryItem.position.x + cell.x, inventoryItem.position.y + cell.y].item = inventoryItem;
            }
        }
    }
}
