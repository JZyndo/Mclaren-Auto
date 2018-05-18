using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PanelFade))]
[CanEditMultipleObjects]
public class FadeView : Editor
{

    private ReorderableList list;
    SerializedProperty duration;
    SerializedProperty curve;
    SerializedProperty rank;
    SerializedProperty ignorePage;
    private void OnEnable()
    {
        //find the properties
        duration = serializedObject.FindProperty("duration");
        curve = serializedObject.FindProperty("curve");
        rank = serializedObject.FindProperty("rank");
        ignorePage = serializedObject.FindProperty("ignoreEmptyPage");

        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("objectsToFade"),
                true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Objects To Fade");
        };

        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            var halfWidth = rect.width / 2.0f;
            var width = (int)((halfWidth / 5.0f));
            var offset = (int)(width + 10);

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, halfWidth/2.1f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("objectToFade"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth / 2.1f + 10f, rect.y, halfWidth / 2.1f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("type"), GUIContent.none);
            EditorGUI.LabelField(
               new Rect(rect.x + halfWidth, rect.y, width, EditorGUIUtility.singleLineHeight), "Alpha:");
            EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth + offset, rect.y, width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("targetAlpha"), GUIContent.none);
            EditorGUI.LabelField(
               new Rect(rect.x + halfWidth + 2 * offset, rect.y, width, EditorGUIUtility.singleLineHeight), "Delay:");
            EditorGUI.PropertyField(
                new Rect(rect.x + halfWidth + 3 * offset, rect.y, width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("delay"), GUIContent.none);

        };
    }

    public override void OnInspectorGUI()
    {
        PanelFade panel = (PanelFade)target;
        serializedObject.Update();
        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(rank);
        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(curve);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Linked Event: ");
        panel.linkedEvent = (LinkedEvent)EditorGUILayout.EnumPopup(panel.linkedEvent);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(ignorePage);

        EditorGUILayout.Separator();


        EditorGUILayout.LabelField("Attach objects to Fade: ");
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
