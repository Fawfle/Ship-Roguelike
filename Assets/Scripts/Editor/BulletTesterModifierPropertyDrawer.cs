
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BulletTesting;

[CustomPropertyDrawer(typeof(BulletTester.BulletTesterModifier))]
public class BulletTesterModifierPropertyDrawer : PropertyDrawer
{
	static readonly Dictionary<int, string[]> PARAMATERS = new()
	{
		{ (int)BulletTester.BulletModifierType.Direction, new string[] { "speed", "direction" } },
		{ (int)BulletTester.BulletModifierType.TowardsPoint, new string[] { "speed", "point" } },
		{ (int)BulletTester.BulletModifierType.AwayPoint, new string[] { "speed", "point" } },
		{ (int)BulletTester.BulletModifierType.ChangingDirection, new string[] { "speed", "rotateSpeed", "angle" } },
		{ (int)BulletTester.BulletModifierType.ChangingDirectionPoint, new string[] { "speed", "rotateSpeed", "point" } },
		{ (int)BulletTester.BulletModifierType.ChangingDirectionAwayPoint, new string[] { "speed", "rotateSpeed", "point" } },
		{ (int)BulletTester.BulletModifierType.Circle, new string[] { "speed", "radius" } },
		{ (int)BulletTester.BulletModifierType.AroundPoint, new string[] { "speed", "point" } },
		{ (int)BulletTester.BulletModifierType.AroundPointWithRadius, new string[] { "speed", "point", "radius" } },
		{ (int)BulletTester.BulletModifierType.Ellipse, new string[] { "speedX", "speedY", "size" } },
		{ (int)BulletTester.BulletModifierType.AroundPointEllipse, new string[] { "speedX", "speedY", "point", "size" } },
		{ (int)BulletTester.BulletModifierType.Target, new string[] { "speed", "rotateSpeed", "target", "angle" } },
	};

	//private static readonly float PROP_HEIGHT = 20f;
	private static readonly float PADDING = 2f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		PARAMATERS.TryGetValue(property.FindPropertyRelative("type").enumValueIndex, out string[] paramaters);

		EditorGUI.BeginProperty(position, label, property);

		float yPosition = position.y;

		var typeProp = property.FindPropertyRelative("type");
		//var typeHeight = EditorGUI.GetPropertyHeight(typeProp, false);
		AddProperty(typeProp);

		if (paramaters != null)
		{
			foreach (string param in paramaters)
			{
				var prop = property.FindPropertyRelative(param);
				//var propHeight = EditorGUI.GetPropertyHeight(prop, false);

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
		float height = base.GetPropertyHeight(property, label) + PADDING;

		//height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("type"));

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