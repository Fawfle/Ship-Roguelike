using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UpdatableFloat))]
public class UpdatableFloatPropertyDrawer : PropertyDrawer
{
	private static readonly float PADDING = 2f;

	private static readonly float VALUE_WIDTH = 280f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		float yPosition = position.y;

		//EditorGUI.PrefixLabel(position, new(property.name));

		var valueProp = property.FindPropertyRelative("value");
		var onProp = property.FindPropertyRelative("updatable");
		//var typeHeight = EditorGUI.GetPropertyHeight(typeProp, false);
		var valueHeight = EditorGUI.GetPropertyHeight(valueProp) + PADDING;


		// draw value and onprop on same line, dont even ask
		EditorGUI.PropertyField(new Rect(position.x, yPosition, VALUE_WIDTH, valueHeight), valueProp, new GUIContent(property.name), false);
		EditorGUI.PropertyField(new Rect(position.x + 200f + PADDING, yPosition, 0, valueHeight), onProp, new GUIContent("                         Upd"), false);

		yPosition += valueHeight;
		EditorGUI.indentLevel++;

		if (onProp.boolValue)
		{
			var addProp = property.FindPropertyRelative("add");
			var multiProp = property.FindPropertyRelative("multiplier");

			AddProperty(addProp);
			AddProperty(multiProp);
		}

		EditorGUI.indentLevel--;

		EditorGUI.EndProperty();

		void AddProperty(SerializedProperty prop)
		{
			float height = EditorGUI.GetPropertyHeight(prop);
			EditorGUI.PropertyField(new Rect(position.x, yPosition, VALUE_WIDTH, height), prop, true);
			yPosition += height + PADDING;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = base.GetPropertyHeight(property, label) + PADDING;
		var onProp = property.FindPropertyRelative("updatable");

		if (onProp.boolValue)
		{
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("add")) + PADDING;
			height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("multiplier")) + PADDING;
		}

		return height;
	}
}
