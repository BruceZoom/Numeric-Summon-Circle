using UnityEngine;
using UnityEditor;
using NSC.Utils;

/// <summary>
/// Allows you to add '[ReadOnly]' before a variable so that it is shown but not editable in the inspector.
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label);
		GUI.enabled = true;
	}
}
