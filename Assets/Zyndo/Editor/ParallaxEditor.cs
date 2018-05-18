using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Parallax))]
public class ParallaxEditor : Editor
{

	SerializedProperty parallaxFactor;
    SerializedProperty parallaxAxisMultiplier;
    SerializedProperty parallaxRotation;
    void OnEnable()
    {
		parallaxFactor = serializedObject.FindProperty("parallaxFactor");
        parallaxAxisMultiplier = serializedObject.FindProperty("parallaxAxisMultiplier");
        parallaxRotation = serializedObject.FindProperty("parallaxRotation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
		EditorGUILayout.PropertyField(parallaxFactor);
        EditorGUILayout.PropertyField(parallaxAxisMultiplier);
        EditorGUILayout.PropertyField(parallaxRotation);
        serializedObject.ApplyModifiedProperties();
    }
}
