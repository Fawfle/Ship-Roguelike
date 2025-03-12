using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private InventoryGrid grid;

    public InventoryItemData itemData;

	/// <summary>
	/// Position of the relative (0,0) of the item
	/// </summary>
	public Vector2Int position;

	// clicked cell of item
	[HideInInspector] public Vector2Int dragCell = Vector2Int.zero;
	[HideInInspector] public Vector2 dragOffset = Vector2.zero;

	private Vector2 originalPosition;

	private void Start()
	{
		grid = GetComponentInParent<InventoryGrid>();

		originalPosition = transform.position;

		GenerateCells();
	}

	public void GenerateCells()
    {
		ClearGrid();

		foreach (Vector2Int cell in itemData.cells)
		{
			var c = Instantiate(itemData.cellPrefab, transform);

			c.transform.localPosition = grid.effectiveCellSize * (Vector2)cell;
			c.GetComponent<RectTransform>().sizeDelta = grid.cellSize * Vector2.one;
		}
    }

	public void ClearGrid()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			if (!Application.isPlaying) DestroyImmediate(transform.GetChild(i).gameObject);
			else Destroy(transform.GetChild(i).gameObject);
		}
	}

	public void SetGridPosition(InventoryGridCell cell)
	{
		this.position = cell.position;
		transform.position = cell.transform.position;

		originalPosition = transform.position;
	}

	public void ResetPosition()
	{
		transform.position = originalPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		dragOffset = (Vector2)transform.position - InputManager.PointerWorldPosition;
		dragCell = Vector2Int.RoundToInt(dragOffset / grid.effectiveCellSize);

		grid.SelectInventoryItem(this);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		grid.DeselectInventoryItem(this);
	}
}
