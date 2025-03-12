using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Scriptable Objects/Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
	public GameObject cellPrefab;
	
	public List<Vector2Int> cells = new();
}
