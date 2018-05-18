using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(OnOffEvent))]
[CanEditMultipleObjects]
public class OnOffEventView : Editor
{
    private ReorderableList list;

    SerializedProperty rank;
    SerializedProperty ignorePage;
   
    private void OnEnable()
    {
        //init serialized props
        rank = serializedObject.FindProperty("rank");
        ignorePage = serializedObject.FindProperty("ignoreEmptyPage");

        //intialize the ui list
        list = new ReorderableList(serializedObject,
            serializedObject.FindProperty("targetData"),
            true, true, true, true);

        //draw the header
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Targeted Components");
        };

        //draw the line items
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                //some drawing parameters
                var halfWidth = rect.width / 1.0f;
                var width = (int)((halfWidth / 2.0f));
                var offset = (int)(width + 10);

                var onOffEvent = (OnOffEvent)target;
                var data = onOffEvent.targetData[index];
                string[] componentNames = new string[] { "" };
                var attachedComponents = new Component[] { null };
                if (data.targetObject != null)
                {
                    attachedComponents = data.targetObject.GetComponents(typeof(MonoBehaviour));
                    componentNames = attachedComponents.Select(x => x.GetType().ToString()).ToArray();
                }

                //draw the properties
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, halfWidth / 2.1f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("targetObject"), GUIContent.none);

                data.id = EditorGUI.Popup(
                    new Rect(rect.x + halfWidth / 2.1f + 10f, rect.y, halfWidth / 2.1f, EditorGUIUtility.singleLineHeight),
                    data.id, componentNames);

                data.targetComponent = attachedComponents[data.id] as MonoBehaviour;
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(rank);
        EditorGUILayout.PropertyField(ignorePage);
        EditorGUILayout.Separator();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
