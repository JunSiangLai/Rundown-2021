using UnityEngine;
using UnityEditor;


public class ReadOnlyWhenPlayingAttribute : PropertyAttribute { }
[CustomPropertyDrawer(typeof(ReadOnlyDuringPlayTime))]
public class ReadOnlyDuringPlayTime : PropertyDrawer
{
    // Necessary since some properties tend to collapse smaller than their content
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // Draw a disabled property field
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Application.isPlaying)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}


public class EditableOnlyWhenNotPlayTimeAttribute : PropertyAttribute { }
[CustomPropertyDrawer(typeof(EditableOnlyWhenNotPlayTimeAttribute))]
public class EditableOnlyWhenNotPlayTimeAttributeDrawer : PropertyDrawer
{
    // Necessary since some properties tend to collapse smaller than their content
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // Draw a disabled property field
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = !Application.isPlaying;
        if (GUI.enabled)
            EditorGUI.PropertyField(position, property, label, true);
    }
}