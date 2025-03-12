
using UnityEditor;
using UnityEngine;
using BulletTesting;

[CustomEditor(typeof(BulletTester))]
public class BulletTesterInspector : Editor
{
	public override void OnInspectorGUI()
	{
		BulletTester tester = (BulletTester)target;

		if (GUILayout.Button("Test Bullets"))
		{
			tester.TestBullets();
		}

		if (GUILayout.Button("Clear Bullets"))
		{
			tester.ClearBullets();
		}

		base.OnInspectorGUI();
	}
}