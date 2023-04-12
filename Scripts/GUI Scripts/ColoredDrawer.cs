using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Colored))]
public class ColoredDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Colored attribute = (Colored)base.attribute;
        EditorGUI.BeginProperty(position, label, property);
        GUI.color = attribute.color;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.color = Color.white;
        EditorGUI.EndProperty();
    }
}