using UnityEngine;
using UnityEditor;
using BulletTesting;

[CustomEditor(typeof(InventoryGrid))]
public class InventoryGridInspector : Editor
{
	public override void OnInspectorGUI()
	{
		InventoryGrid tester = (InventoryGrid)target;

		if (GUILayout.Button("Test Grid"))
		{
			tester.UpdateGridSize();
			tester.GenerateGrid();
		}
		if (GUILayout.Button("Clear Grid"))
		{
			tester.ClearGrid();
		}

		base.OnInspectorGUI();
	}
}
