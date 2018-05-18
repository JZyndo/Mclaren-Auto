using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(PropertyFade))]
[CanEditMultipleObjects]
public class ParamEventView : Editor
{

    private ReorderableList list;
    SerializedProperty duration;
    SerializedProperty curve;
    SerializedProperty delay;
    SerializedProperty rank;
    SerializedProperty ignorePage;
    SerializedProperty linkedEvent;

    private void OnEnable()
    {
        //init serialized props
        rank = serializedObject.FindProperty("rank");
        ignorePage = serializedObject.FindProperty("ignoreEmptyPage");
        duration = serializedObject.FindProperty("duration");
        curve = serializedObject.FindProperty("curve");
        delay = serializedObject.FindProperty("delay");
        linkedEvent = serializedObject.FindProperty("linkedEvent");

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
                var width = (int)((halfWidth / 4.0f));
                var offset = (int)(width + 10);

                var propFade = (PropertyFade)target;
                var data = propFade.targetData[index];

                //use reflection to select a component
                string[] componentNames = new string[] { "" };
                string[] paramNames = new string[] { "" };
                var attachedComponents = new Component[] { null };
                if (data.targetObject != null)
                {
                    attachedComponents = data.targetObject.GetComponents(typeof(MonoBehaviour));
                    componentNames = attachedComponents.Select(x => x.GetType().ToString()).ToArray();

                    if (data.targetComponent != null)
                    {
                        Debug.Log("component type: " + data.targetComponent.GetType().ToString());
                        var fields = data.targetComponent.GetType().GetFields(
                            BindingFlags.Instance | BindingFlags.Public).Where(x => x.FieldType == typeof(System.Single));
                        paramNames = fields.Select(x => x.Name).ToArray();
                    }
                }

                //draw the properties
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, offset, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("targetObject"), GUIContent.none);

                data.componentId = EditorGUI.Popup(
                    new Rect(rect.x + offset, rect.y, width, EditorGUIUtility.singleLineHeight),
                    data.componentId, componentNames);

                data.paramId = EditorGUI.Popup(
                    new Rect(rect.x + 2.0f * offset, rect.y, width, EditorGUIUtility.singleLineHeight),
                    data.paramId, paramNames);

                data.targetComponent = attachedComponents[data.componentId] as MonoBehaviour;
                data.targetParam = paramNames[data.paramId];

                EditorGUI.PropertyField(
                    new Rect(rect.x + 3.0f * offset, rect.y, 0.5f * width, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("targetValue"), GUIContent.none);
            };
    }

    public override void OnInspectorGUI()
    {
        PropertyFade pFade = (PropertyFade)target;
        serializedObject.Update();

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(rank);
        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(curve);
        EditorGUILayout.PropertyField(linkedEvent);
        EditorGUILayout.PropertyField(ignorePage);
        EditorGUILayout.Separator();

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
