using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PersonData))]
public class PersonDataPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float halfWidth = position.width * 0.5f;
        position.width = halfWidth;

        EditorGUI.PropertyField(position, property.FindPropertyRelative("Name"), GUIContent.none);

        position.x += halfWidth;

        EditorGUI.PropertyField(position, property.FindPropertyRelative("Role"), GUIContent.none);

    }
}
