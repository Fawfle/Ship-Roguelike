using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Scriptable Objects/Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
	public GameObject cellPrefab;
	
	public List<Cell> cells = new();

	[System.Serializable]
	public class Cell
	{
		public Vector2Int position;
	}
}
