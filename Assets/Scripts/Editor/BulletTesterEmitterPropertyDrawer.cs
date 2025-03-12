using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BulletTesting;

[CustomPropertyDrawer(typeof(BulletTester.BulletTesterEmitter))]
public class BulletTesterEmitterPropertyDrawer : PropertyDrawer
{
	static readonly Dictionary<int, string[]> PARAMATERS = new()
	{
		{ (int)BulletTester.BulletTesterEmitterType.Point, new string[] { "count", "origin" } },
		{ (int)BulletTester.BulletTesterEmitterType.Circle, new string[] { "count", "origin", "radius" } },
		{ (int)BulletTester.BulletTesterEmitterType.Arc, new string[] { "count", "origin", "radius", "startAngle", "endAngle" } },
		{ (int)BulletTester.BulletTesterEmitterType.Line, new string[] { "count", "startPosition", "endPosition" } },
	};

	//private static readonly float PROP_HEIGHT = 20f;
	private static readonly float PADDING = 2f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		PARAMATERS.TryGetValue(property.FindPropertyRelative("type").enumValueIndex, out string[] paramaters);

		EditorGUI.BeginProperty(position, label, property);

		EditorGUI.PrefixLabel(position, new(property.name));

		float yPosition = position.y + 20f;

		var typeProp = property.FindPropertyRelative("type");
		var prefabProp = property.FindPropertyRelative("prefab");
		//var typeHeight = EditorGUI.GetPropertyHeight(typeProp, false);
		AddProperty(typeProp);
		AddProperty(prefabProp);

		if (paramaters != null)
		{
			foreach (string param in paramaters)
			{
				var prop = property.FindPropertyRelative(param);

				AddProperty(prop);
			}
		}

		EditorGUI.EndProperty();

		void AddProperty(SerializedProperty prop)
		{
			float height = EditorGUI.GetPropertyHeight(prop);
			EditorGUI.PropertyField(new Rect(position.x, yPosition, position.width - PADDING, height), prop, true);
			yPosition += height + PADDING;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = base.GetPropertyHeight(property, label) + PADDING + 20f;

		height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("type")) + PADDING;

		if (PARAMATERS.TryGetValue(property.FindPropertyRelative("type").enumValueIndex, out string[] paramaters)) 
		{
			foreach (string param in paramaters)
			{
				height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative(param)) + PADDING;
			}
		}

		return height;
	}
}
